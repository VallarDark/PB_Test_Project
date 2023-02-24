using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public static class RepositoryExtensions
    {
        public static IRepository<T>? GetRepository<T>(
            this IEnumerable<IRepository<T>> repositories,
            RepositoryType repositoryType) where T : EntityBase
        {
            bool TryGetRepositoryAttribute(IRepository<T> repository)
            {
                try
                {
                    var attribute = Attribute.GetCustomAttribute(
                        repository.GetType(),
                        typeof(RepositoryTypeAttribute));

                    var repositoryTypeAttribute = attribute as RepositoryTypeAttribute;

                    if (repositoryTypeAttribute == null
                        || repositoryTypeAttribute.Type != repositoryType)
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }

                return true;
            }

            return repositories.FirstOrDefault(TryGetRepositoryAttribute);
        }
    }
}
