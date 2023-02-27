using Contracts;
using Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Agregates.UserAgregate
{
    public class UserRole : ValueObject, IComparable<UserRoleType>
    {
        private ICollection<User> users;

        public UserRoleType RoleType { get; private set; }

        public ICollection<User> Users => users.ToList();

        public UserRole(UserRoleType role, ICollection<User> users)
        {
            RoleType = role;
            this.users = EnsuredUtils.EnsureNotNull(users);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return RoleType;
        }

        public int CompareTo(UserRoleType other)
        {
            return ((int)RoleType).CompareTo((int)other);
        }
    }
}
