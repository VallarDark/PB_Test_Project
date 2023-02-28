using Contracts;
using Domain.Agregates.UserAgregate;
using System;
using System.Collections.Generic;

namespace Persistence.Entities
{
    public class UserRoleEntity : IEntityBase
    {
        public string Id { get; set; }

        public UserRoleType RoleType { get; set; }

        public virtual ICollection<UserEntity> Users { get; set; }

        public UserRoleEntity() : base()
        {
            Id = Guid.NewGuid().ToString();
            RoleType = UserRoleType.User;
            Users = new List<UserEntity>();
        }

        public UserRoleEntity(UserRole role) : this()
        {
            Id = role.Id;
            RoleType = role.RoleType;
        }
    }
}
