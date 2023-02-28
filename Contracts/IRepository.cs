namespace Contracts
{
    public interface IRepository<T> : IReadeableRepository<T>, IWriteableRepository<T>, IRemoveableRepository<T> where T : class, IEntity
    {
    }
}
