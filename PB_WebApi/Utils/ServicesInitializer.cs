﻿using AutoMapper.Extensions.ExpressionMapping;
using Contracts;
using Domain.Aggregates.ProductAggregate;
using Domain.Aggregates.UserAggregate;
using Domain.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using PB_WebApi.Authorization;
using Persistence.Dapper.Context;
using Persistence.EntityFramework.Context;
using Services;
using Services.ProductAggregate;
using Services.UserAggregate;
using Services.Utils;
using System.Reflection;

namespace PB_WebApi.Utils
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
        public static IServiceCollection AddRepositoryResolver(
            this IServiceCollection services)
        {
            services.AddScoped<IRepositoryResolver, RepositoryResolver>();

            return services;
        }

        /// <summary>
        /// Add all services to Service collection
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddServices(
            this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserTokenProvider, UserTokenProvider>();

            return services;
        }

        /// <summary>
        /// Add all repositories and DbContext to Service collection
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            var assembly = Assembly.Load(nameof(Persistence));

            var allAssemblyTypes = assembly.GetTypes();

            services.AddAutoMapper(cfg => cfg.AddExpressionMapping(), assembly);

            services.AddDbContext<PbDbContext>();
            services.AddSingleton<DapperContext>();

            services.AddTransient<IUserRepository>(allAssemblyTypes);
            services.AddTransient<IUserRoleRepository>(allAssemblyTypes);
            services.AddTransient<IProductRepository>(allAssemblyTypes);
            services.AddTransient<IProductCategoryRepository>(allAssemblyTypes);

            return services;
        }

        /// <summary>
        /// Add JWT AddAuthentication to Service collection
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Project configuration</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddConfiguredAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var scheme = JwtBearerDefaults.AuthenticationScheme;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = scheme;
                options.DefaultChallengeScheme = scheme;
                options.DefaultScheme = scheme;
            })
            .AddScheme<JwtBearerOptions, UserJwtAuthenticationHandler>(
            scheme,
            scheme,
            o =>
            {
                o.SaveToken = true;

                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (EncodingUtils.AltDataEncoding.GetBytes(configuration["JWT:Key"] ?? string.Empty)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

            return services;
        }

        /// <summary>
        /// Add Authorization
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddConfiguredAuthorization(
            this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.AddPolicy(
                    "AdminRole",
                    p => p.AddRequirements(new UserRoleRequirement(UserRoleType.Admin)));

                options.AddPolicy(
                    "UserRole",
                    p => p.AddRequirements(new UserRoleRequirement(UserRoleType.User)));
            });

            services.AddScoped<IAuthorizationHandler, UserAuthorizationHandler>();

            return services;
        }

        /// <summary>
        /// Add Controllers with authorization
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddConfiguredCors(
            this IServiceCollection services)
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

