using Microsoft.FSharp.Core;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRemoveableRepository<T> where T : EntityBase
    {
        Task<Unit> Delete(string id);
    }
}
