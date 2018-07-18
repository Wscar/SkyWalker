using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IdentityServer.Dtos;
using IdentityServer.Infrastructure;
using Resilience.HttpInterface;
using Microsoft.AspNetCore.Http;
using Resilience;

namespace IdentityServer
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
            services.AddIdentityServer()
                 .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.GetApiResource())
                .AddInMemoryIdentityResources(Config.GetIdentityResource())
                .AddInMemoryClients(Config.GetClients())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddProfileService<ProfileService>()
                .AddCorsPolicyService<CorsPolicyService>();
            services.AddScoped<IAccountService, AccountService>();
            //获取配置文件中的服务发现
            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            services.AddSingleton(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ResilienceHttpClient>>();
                var httpContextAccesor = sp.GetRequiredService<IHttpContextAccessor>();
                int retryCount = 3;
                var exceptionCountAllowBeforeBreaker = 3;
                var factory = new ResilienceClientFactory(logger, httpContextAccesor, retryCount, exceptionCountAllowBeforeBreaker);
                return factory;
            });
            //注册全局单例IHttpClient
            services.AddSingleton<IHttpClient>(sp =>
            {
               return sp.GetRequiredService<ResilienceClientFactory>().GetResilienceHttpClient();
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseIdentityServer();
            app.UseCors(buider =>
            {
                buider.WithOrigins("http://localhost:8080")
                .AllowAnyHeader();
            });
            app.UseMvc();
        }
    }
}
