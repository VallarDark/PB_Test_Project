using Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Services;

namespace WebApi.Utils
{
    /// <summary>
    /// Startup initializer
    /// </summary>
    public static class ServicesInitializer
    {

        /// <summary>
        /// Add configured Swagger to Service collection
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(string.Format(@"{0}\OnionArchitecture.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "OnionArchitecture",
                });
            });

            return services;
        }

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
        /// Use configured Swagger in application builder
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <returns>Application builder</returns>
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnionArchitecture");
            });

            return app;
        }
    }
}
