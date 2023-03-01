using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Utils
{
    public static class InitializationUtils
    {
        public static IEnumerable<Type> GetAllInstances<T>(this IEnumerable<Type> types)
        {
            return types.Where(t => t.GetInterfaces().Contains(typeof(T)));
        }

        public static IServiceCollection AddTransient<T>(
            this IServiceCollection services,
            IEnumerable<Type> assemblyTypes)
        {
            var instances = assemblyTypes.GetAllInstances<T>();

            foreach (var instance in instances)
            {
                services.Add(
                    new ServiceDescriptor(
                        typeof(T),
                        instance,
                        ServiceLifetime.Transient));
            }

            return services;
        }
    }
}
