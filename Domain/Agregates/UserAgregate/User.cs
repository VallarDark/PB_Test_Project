using Contracts;
using Domain.Utils;
using Microsoft.FSharp.Core;

namespace Domain.Agregates.UserAgregate
{
    public class User : EntityBase
    {
        private const int MIN_LENGHT = 6;
        private const int MAX_LENGTH = 20;

        public PersonalData PersonalData { get; private set; }

        public string NickName { get; private set; }

        public UserRole Role { get; private set; }

        public Unit UpdateRole(UserRole role)
        {
            EnsuredUtils.EnsureNotNull(role);
            var result = EnsuredUtils.EnsureNewValueIsNotSame(Role, role);

            Role = role;

            return result;
        }

        public User(PersonalData personalData, string nickName, UserRole role)
        {
            PersonalData = EnsuredUtils.EnsureNotNull(personalData);
            NickName = EnsuredUtils.EnsureStringLengthIsCorrect(nickName, MIN_LENGHT, MAX_LENGTH);
            Role = EnsuredUtils.EnsureNotNull(role);
        }
    }
}
