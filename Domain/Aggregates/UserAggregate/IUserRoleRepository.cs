using Contracts;

namespace Domain.Aggregates.UserAggregate
{
    public interface IUserRoleRepository : IReadableRepository<UserRole>, IResolvable
    {
    }
}
