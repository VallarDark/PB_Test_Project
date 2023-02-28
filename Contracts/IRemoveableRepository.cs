using Microsoft.FSharp.Core;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRemoveableRepository<T> where T : class, IEntity
    {
        Task<Unit> Delete(string id);
    }
}
