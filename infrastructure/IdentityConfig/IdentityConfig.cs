﻿using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using persistent;
using persistent.Context;

namespace infrastructure.IdentityConfig
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services,IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AdvancedConnection");
            services.AddDbContext<IdentityDatabaseContext>(option => option.UseSqlServer(connectionString));
           
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<IdentityDatabaseContext>()
                .AddDefaultTokenProviders().AddRoles<IdentityRole>().AddErrorDescriber<CustomIdentityErrors>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            });

            return services;
        }
    }
}
