using Contracts;

namespace Domain.Aggregates.UserAggregate
{
    public interface IUserRepository : IRepository<User>, IResolvable
    {
    }
}
