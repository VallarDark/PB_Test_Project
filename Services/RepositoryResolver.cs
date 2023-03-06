using Contracts;
using Domain.Utils;
using Microsoft.Extensions.DependencyInjection;
using Services.Extensions;
using System;

namespace Services
{
    public class RepositoryResolver : IRepositoryResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public RepositoryResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = EnsuredUtils.EnsureNotNull(serviceProvider);
        }

        public IReadableRepository<Y>? GetReadableRepository<T, Y>(
            RepositoryType repositoryType)
            where T : class, IReadableRepository<Y>, IResolvable
            where Y : class, IEntity
        {
            return _serviceProvider.GetServices<T>()
                .GetReadableRepositoryOrDefault<T, Y>(repositoryType);
        }

        public IRemoveableRepository<Y>? GetRemoveableRepository<T, Y>(
            RepositoryType repositoryType)
            where T : class, IRemoveableRepository<Y>, IResolvable
            where Y : class, IEntity
        {
            return _serviceProvider.GetServices<T>()
                .GetRemoveableRepositoryOrDefault<T, Y>(repositoryType);
        }

        public IRepository<Y>? GetRepository<T, Y>(
            RepositoryType repositoryType)
            where T : class, IRepository<Y>, IResolvable
            where Y : class, IEntity
        {
            return _serviceProvider.GetServices<T>()
                .GetRepositoryOrDefault<T, Y>(repositoryType);
        }

        public IWriteableRepository<Y>? GetWriteableRepository<T, Y>(
            RepositoryType repositoryType)
            where T : class, IWriteableRepository<Y>, IResolvable
            where Y : class, IEntity
        {
            return _serviceProvider.GetServices<T>()
                .GetWriteableRepositoryOrDefault<T, Y>(repositoryType);
        }
    }
}
