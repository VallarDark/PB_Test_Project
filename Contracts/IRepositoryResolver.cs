namespace Contracts
{
    public interface IRepositoryResolver
    {
        IRepository<Y>? GetRepository<T, Y>(RepositoryType repositoryType) where T : class, IRepository<Y>, IResolvable where Y : EntityBase;

        IReadeableRepository<Y>? GetReadeableRepository<T, Y>(RepositoryType repositoryType) where T : class, IReadeableRepository<Y>, IResolvable where Y : EntityBase;

        IWriteableRepository<Y>? GetWriteableRepository<T, Y>(RepositoryType repositoryType) where T : class, IWriteableRepository<Y>, IResolvable where Y : EntityBase;

        IRemoveableRepository<Y>? GetRemoveableRepository<T, Y>(RepositoryType repositoryType) where T : class, IRemoveableRepository<Y>, IResolvable where Y : EntityBase;
    }
}
