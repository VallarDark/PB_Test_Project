using Contracts;
using System.Collections.Generic;

namespace Services.Extensions
{
    public static class RepositoryExtensions
    {
        public static T GetRepositoryOrDefault<T, Y>(
            this IEnumerable<T> repositories,
            RepositoryType repositoryType) where T : class, IRepository<Y>, IResolvable where Y : class, IEntity
        {
            foreach (var repository in repositories)
            {
                if (repository is IResolvable resolvable && resolvable.ServiceType == repositoryType.ToString())
                {
                    return repository;
                }
            }

            return default;
        }

        public static T GetReadeableRepositoryOrDefault<T, Y>(
           this IEnumerable<T> repositories,
           RepositoryType repositoryType) where T : IReadeableRepository<Y>, IResolvable where Y : class, IEntity
        {
            foreach (var repository in repositories)
            {
                if (repository is IResolvable resolvable && resolvable.ServiceType == repositoryType.ToString())
                {
                    return repository;
                }
            }

            return default;
        }

        public static T GetWriteableRepositoryOrDefault<T, Y>(
           this IEnumerable<T> repositories,
           RepositoryType repositoryType) where T : IWriteableRepository<Y>, IResolvable where Y : class, IEntity
        {
            foreach (var repository in repositories)
            {
                if (repository is IResolvable resolvable && resolvable.ServiceType == repositoryType.ToString())
                {
                    return repository;
                }
            }

            return default;
        }

        public static T GetRemoveableRepositoryOrDefault<T, Y>(
           this IEnumerable<T> repositories,
           RepositoryType repositoryType) where T : IRemoveableRepository<Y>, IResolvable where Y : class, IEntity
        {
            foreach (var repository in repositories)
            {
                if (repository is IResolvable resolvable && resolvable.ServiceType == repositoryType.ToString())
                {
                    return repository;
                }
            }

            return default;
        }
    }
}
