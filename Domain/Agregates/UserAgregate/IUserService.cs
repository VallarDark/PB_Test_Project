using Contracts;
using Microsoft.FSharp.Core;

namespace Domain.Agregates.UserAgregate
{
    public interface IUserService
    {
        Unit Create(User item);

        User Get(string id);

        PaginatedCollectionBase<User> GetAll(int pageNumber);
    }
}
