namespace Contracts
{
    public interface IRepositoryResolver
    {
        IRepository<Y>? GetRepository<T, Y>(RepositoryType repositoryType)
            where T : class, IRepository<Y>, IResolvable
            where Y : class, IEntity;

        IReadableRepository<Y>? GetReadableRepository<T, Y>(RepositoryType repositoryType)
            where T : class, IReadableRepository<Y>, IResolvable
            where Y : class, IEntity;

        IWriteableRepository<Y>? GetWriteableRepository<T, Y>(RepositoryType repositoryType)
            where T : class, IWriteableRepository<Y>, IResolvable
            where Y : class, IEntity;

        IRemoveableRepository<Y>? GetRemoveableRepository<T, Y>(RepositoryType repositoryType)
            where T : class, IRemoveableRepository<Y>, IResolvable
            where Y : class, IEntity;
    }
}
