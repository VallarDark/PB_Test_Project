namespace Contracts
{
    public interface IRepository<T> :
        IReadableRepository<T>,
        IWriteableRepository<T>,
        IRemoveableRepository<T>

        where T : class, IEntity
    {
    }
}
