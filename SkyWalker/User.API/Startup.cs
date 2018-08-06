using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SkyWalker.Dal.DBContext;
using IdentityServer4;
using Microsoft.AspNetCore.Http;
using User.API.IdentityServerValidator;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Reflection;
using SkyWalker.Dal;
using MySql.Data.MySqlClient;
using User.API.Filters;

using User.API.ConfigOptions;
using Consul;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace User.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            //添加数据配置
            services.AddDbContext<SkyWalkerDbContext>(
                options =>
                {
                    options.UseMySql(Configuration.GetConnectionString("MySqlConnectionString"),
                        sql => { sql.MigrationsAssembly(migrationAssembly); }
                        );
                });
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.GetApiResource())
                .AddInMemoryIdentityResources(Config.GetIdentityResource())
                .AddInMemoryClients(Config.GetClients())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddProfileService<ProfileService>()
                ;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Audience = "user_api";
                    options.Authority = "http://localhost:56454";
                    options.SaveToken = true;
                });
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));
                
            });
            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            services.ConfigRepository();
            services.AddSingleton<IConsulClient>(p => new ConsulClient(consul =>
            {
                var serviceConfigation = p.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;
                if (!string.IsNullOrEmpty(serviceConfigation.Consul.HttpEndpoint))
                {
                    consul.Address = new Uri(serviceConfigation.Consul.HttpEndpoint);
                }
            }));
            services.AddScoped(c => new MySqlConnection(Configuration.GetConnectionString("MySqlConnectionString")));
            services.Configure<AppSetting>(Configuration.GetSection("ConnectionStrings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IApplicationLifetime applicationLifetime, IOptions<ServiceDiscoveryOptions> options, IConsulClient consul)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(buider =>
            {
                buider.WithOrigins("http://localhost:8080")
                .AllowAnyHeader();
            });
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "default",
                   template: "{controller=Home}/{action=Index}/{id?}");
            });
            //注册服务
            applicationLifetime.ApplicationStarted.Register(() => RegisterService(app, options, consul));
            applicationLifetime.ApplicationStopped.Register(() => { DeRegisterService(app, options, consul); });
        }
        #region 服务发现
        public void RegisterService(IApplicationBuilder app,IOptions<ServiceDiscoveryOptions>  serviceOptions,IConsulClient consulClient)
        {
            //获取本地服务地址
            var features=app.Properties["server.Features"] as FeatureCollection;
            var address = features.Get<IServerAddressesFeature>().Addresses.Select(x => new Uri(x));
            foreach(var addr in address)
            {
                var serverId = $"{serviceOptions.Value.ServiceName}_{addr.Host}:{addr.Port}";
                var httpCheck = new AgentServiceCheck
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(30),
                    HTTP = new Uri(addr, "healthCheck").OriginalString
                };
                var registration = new AgentServiceRegistration
                {
                    Checks = new[] { httpCheck },
                    Address =$"{addr.Scheme}://{addr.Port}",
                    ID = serverId,
                    Name = serviceOptions.Value.ServiceName,
                    Port = addr.Port,
                    Tags = new[] { "skyWalke_user_api" }


                };
                consulClient.Agent.ServiceRegister(registration).Wait();
            }
        }
        public void DeRegisterService(IApplicationBuilder app, IOptions<ServiceDiscoveryOptions> ServiceOptions,
           IConsulClient consul)
        {
            //获取本地服务地址
            var features = app.Properties["server.Features"] as FeatureCollection;
            var address = features.Get<IServerAddressesFeature>()
                .Addresses.Select(x => new Uri(x));
            foreach (var addr in address)
            {
                var serverId = $"{ ServiceOptions.Value.ServiceName}_{addr.Host}:{addr.Port}";
                consul.Agent.ServiceDeregister(serverId).GetAwaiter().GetResult();
            }
        }
        #endregion
    }
}
