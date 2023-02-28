using Contracts;
using Domain.Utils;
using System;

namespace Domain.Agregates.UserAgregate
{
    public class UserRole : IEntity, IComparable<UserRoleType>
    {
        private string id = Guid.NewGuid().ToString();

        public string Id => id;

        public UserRoleType RoleType { get; private set; }

        public UserRole(UserRoleType role)
        {
            RoleType = role;
        }

        public UserRole(UserRoleDto roleDto)
        {
            id = EnsuredUtils.EnsureStringIsNotEmpty(roleDto.Id);

            RoleType = roleDto.RoleType;
        }

        public int CompareTo(UserRoleType other)
        {
            return ((int)RoleType).CompareTo((int)other);
        }

        public override bool Equals(object other)
        {
            if (other is UserRole otherRole)
            {
                if (otherRole.RoleType == RoleType)
                {
                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, RoleType);
        }
    }
}
