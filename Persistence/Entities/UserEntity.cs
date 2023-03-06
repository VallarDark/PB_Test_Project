using Contracts;
using Domain.Aggregates.UserAggregate;
using System;

namespace Persistence.Entities
{
    public class UserEntity : IEntity
    {
        public string Id { get; set; }

        public string NickName { get; set; }

        public string Password { get; set; }

        public virtual UserRoleEntity Role { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string? SessionToken { get; set; }

        public RepositoryType RepositoryType { get; set; }


        public UserEntity()
        {
            Id = Guid.NewGuid().ToString();
            NickName = string.Empty;
            Password = string.Empty;
            Role = new UserRoleEntity();
            Email = string.Empty;
            Name = string.Empty;
            LastName = string.Empty;
            SessionToken = string.Empty;
            RepositoryType = RepositoryType.EntityFramework;
        }

        public UserEntity(User user)
        {
            Id = user.Id;
            NickName = user.NickName;
            Password = user.Password;
            Role = new UserRoleEntity(user.Role);
            Email = user.PersonalData.Email;
            Name = user.PersonalData.Name;
            LastName = user.PersonalData.LastName;
            SessionToken = user.SessionToken;
            RepositoryType = user.RepositoryType;
        }

        public Guid Update(User user)
        {
            NickName = user.NickName;
            Password = user.Password;
            Email = user.PersonalData.Email;
            Name = user.PersonalData.Name;
            LastName = user.PersonalData.LastName;
            SessionToken = user.SessionToken;
            RepositoryType = user.RepositoryType;

            return default;
        }
    }
}
