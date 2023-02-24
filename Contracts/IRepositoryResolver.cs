namespace Contracts
{
    public interface IRepositoryResolver
    {
        IRepository<T>? GetRepositoryByType<T>(RepositoryType repositoryType) where T : EntityBase;
    }
}
