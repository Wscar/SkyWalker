using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using SkyWalker.Dal.Entities;
using SkyWalker.Dal.Repository;

namespace SkyWalker.Dal
{
   public static class AddRepository
    {
        public static void ConfigRepository(this IServiceCollection services)
        {
            services.AddTransient<AppUser>();
            services.AddTransient<Story>();
            services.AddTransient<Comment>();
            services.AddTransient<Book>();
            services.AddScoped<IRepository<AppUser>, UserRepository>();
            services.AddScoped<IStoryRepository, StoryRepository>();
        }
    }
}
