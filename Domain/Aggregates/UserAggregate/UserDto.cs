using Contracts;

namespace Domain.Aggregates.UserAggregate
{
    public class UserDto
    {
        public string Id { get; set; }

        public string NickName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string SessionToken { get; private set; }

        public UserRoleDto Role { get; set; }

        public RepositoryType RepositoryType { get; set; }
    }
}
