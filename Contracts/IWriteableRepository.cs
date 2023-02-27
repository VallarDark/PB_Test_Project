using Microsoft.FSharp.Core;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IWriteableRepository<T> where T : EntityBase
    {
        Task<Unit> Create(T item);

        Task<Unit> Update(T item);
    }
}
