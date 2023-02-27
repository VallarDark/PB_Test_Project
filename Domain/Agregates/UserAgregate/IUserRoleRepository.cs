using Contracts;

namespace Domain.Agregates.UserAgregate
{
    public interface IUserRoleRepository : IReadeableRepository<UserRole>, IResolvable
    {
    }
}
