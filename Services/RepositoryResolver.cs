using Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Services
{
    public class RepositoryResolver : IRepositoryResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public RepositoryResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IRepository<T>? GetRepositoryByType<T>(RepositoryType repositoryType) where T : EntityBase
        {
            return _serviceProvider?.GetServices<IRepository<T>>()
                ?.GetRepository(repositoryType);
        }
    }
}
