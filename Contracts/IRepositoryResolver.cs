namespace Contracts
{
    public interface IRepositoryResolver
    {
        IRepository<Y>? GetRepository<T, Y>(RepositoryType repositoryType) where T : class, IRepository<Y>, IResolvable where Y : class, IEntityBase;

        IReadeableRepository<Y>? GetReadeableRepository<T, Y>(RepositoryType repositoryType) where T : class, IReadeableRepository<Y>, IResolvable where Y : class, IEntityBase;

        IWriteableRepository<Y>? GetWriteableRepository<T, Y>(RepositoryType repositoryType) where T : class, IWriteableRepository<Y>, IResolvable where Y : class, IEntityBase;

        IRemoveableRepository<Y>? GetRemoveableRepository<T, Y>(RepositoryType repositoryType) where T : class, IRemoveableRepository<Y>, IResolvable where Y : class, IEntityBase;
    }
}
