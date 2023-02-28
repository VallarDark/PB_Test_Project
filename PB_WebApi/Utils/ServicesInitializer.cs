using AutoMapper.Extensions.ExpressionMapping;
using Contracts;
using Domain.Agregates.UserAgregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using PB_WebApi.Authorization;
using Persistence.EntityFramework.Context;
using Services;
using Services.UserAgregate;
using Services.Utils;
using System.Reflection;
using System.Text;

namespace WebApi.Utils
{
    /// <summary>
    /// Startup initializer
    /// </summary>
    public static class ServicesInitializer
    {
        /// <summary>
        /// Add repository resolver to Service collection
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddRepositoryResolver(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryResolver, RepositoryResolver>();

            return services;
        }

        /// <summary>
        /// Add all services to Service collection
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserTokenProvider, UserTokenProvider>();

            return services;
        }

        /// <summary>
        /// Add all repositories and DbContext to Service collection
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var assembly = Assembly.Load(nameof(Persistence));

            var allAsssemblyTypes = assembly.GetTypes();

            services.AddAutoMapper(cfg => cfg.AddExpressionMapping(), assembly);

            services.AddDbContext<PbDbContext>();

            services.AddTransient<IUserRepository>(allAsssemblyTypes);
            services.AddTransient<IUserRoleRepository>(allAsssemblyTypes);

            return services;
        }

        /// <summary>
        /// Add Jwt AddAuthentication to Service collection
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Project configuration</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddConfiguredAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddScheme<JwtBearerOptions, UserJwtAuthenticationHandler>(
            JwtBearerDefaults.AuthenticationScheme,
            JwtBearerDefaults.AuthenticationScheme,
            o =>
            {
                o.SaveToken = true;

                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            })
            .AddCookie(options =>
             {
                 options.LoginPath = "/Account/Unauthorized/";
             });

            return services;
        }

        /// <summary>
        /// Add Authorization
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddConfiguredAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            //services.AddScoped<IAuthorizationHandler, UserJwtAuthorizationHandler>();

            return services;
        }

        /// <summary>
        /// Add Controllers with authorization
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddConfiguredControllers(this IServiceCollection services)
        {
            services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            return services;
        }

        /// <summary>
        /// Add Controllers with authorization
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "CrudPolicy",
                    policy =>
                    {
                        policy.WithMethods("PUT", "POST", "DELETE", "GET");
                    });
            });

            return services;
        }
    }
}

