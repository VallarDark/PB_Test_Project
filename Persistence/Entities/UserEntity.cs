﻿using Contracts;
using Domain.Agregates.UserAgregate;
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

        public string? SessionToken { get; private set; }

        public UserEntity() : base()
        {
            Id = Guid.NewGuid().ToString();
            NickName = string.Empty;
            Password = string.Empty;
            Role = null;
            Email = string.Empty;
            Name = string.Empty;
            LastName = string.Empty;
            SessionToken = string.Empty;
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
        }

        public Guid Update(User user)
        {
            NickName = user.NickName;
            Password = user.Password;
            Email = user.PersonalData.Email;
            Name = user.PersonalData.Name;
            LastName = user.PersonalData.LastName;
            SessionToken = user.SessionToken;

            return default;
        }
    }
}