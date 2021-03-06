﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using SkyWalker.Dal.Entities;
using SkyWalker.Dal.Repository;
using SkyWalker.Dal.Service;
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
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IRepository<Book>, BookRepository>();
        }
    }
}
