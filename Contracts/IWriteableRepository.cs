using Microsoft.FSharp.Core;

namespace Contracts
{
    public interface IWriteableRepository<T> where T : EntityBase
    {
        Unit Create(T item);

        Unit Update(T item);
    }
}
