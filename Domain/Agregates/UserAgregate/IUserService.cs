using Contracts;
using System.Threading.Tasks;

namespace Domain.Agregates.UserAgregate
{
    public interface IUserService
    {
        User? CurrentUser { get; }

        Task<string> RegisterCasualUser(UserDto item);

        Task<string> RegisterAdminUser(UserDto item);

        Task<string> LoginUser(UserDto item);

        Task<User?> VerifyUserByPersonalData(PersonalData personalData);

        Task<PaginatedCollectionBase<User>> GetAllUsers(int pageNumber);
    }
}
