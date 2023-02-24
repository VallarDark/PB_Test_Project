using Microsoft.FSharp.Core;

namespace Contracts
{
    public interface IRemoveableRepository<T> where T : EntityBase
    {
        Unit Delete(string id);
    }
}
