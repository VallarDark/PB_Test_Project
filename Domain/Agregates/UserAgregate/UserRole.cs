using Contracts;
using Domain.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Agregates.UserAgregate
{
    public class UserRole : ValueObject
    {
        private ICollection<User> users;

        public UserRoleType Role { get; private set; }

        public ICollection<User> Users => users.ToList();

        public UserRole(UserRoleType role, ICollection<User> users)
        {
            Role = role;
            this.users = EnsuredUtils.EnsureNotNull(users);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Role;
        }
    }
}
