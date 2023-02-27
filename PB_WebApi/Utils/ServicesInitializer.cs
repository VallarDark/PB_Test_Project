using Contracts;
using Domain.Agregates.UserAgregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
            services.AddSingleton<IRepositoryResolver, RepositoryResolver>();

            return services;
        }

        /// <summary>
        /// Add all services to Service collection
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserTokenProvider, UserTokenProvider>();

            return services;
        }

        /// <summary>
        /// Add all repositories to Service collection
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var allAsssemblyTypes = Assembly.Load(nameof(Persistence)).GetTypes();

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
        public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });

            return services;
        }
    }
}
