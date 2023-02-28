using Contracts;
using Domain.Utils;
using Microsoft.FSharp.Core;
using System;
using System.Text.RegularExpressions;

namespace Domain.Agregates.UserAgregate
{
    public class User : IEntityBase
    {
        private const string PASSWORD_PATTERN = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{3,}$";
        private const int MIN_LENGHT = 6;
        private const int MAX_LENGTH = 20;
        private const int PASSWORD_MIN_LENGHT = 8;
        private const int PASSWORD_MAX_LENGTH = 50;

        private readonly static Regex _passswordPattern;

        private string id = Guid.NewGuid().ToString();

        public string Id => id;

        static User()
        {
            _passswordPattern = new Regex(PASSWORD_PATTERN);
        }

        public PersonalData PersonalData { get; private set; }

        public string NickName { get; private set; }

        public string Password { get; private set; }

        public UserRole Role { get; private set; }

        public Unit UpdateRole(UserRole role)
        {
            EnsuredUtils.EnsureNotNull(role);

            var result = EnsuredUtils.EnsureNewValueIsNotSame(Role, role);

            Role = role;

            return result;
        }

        public Unit ChangeNickName(string nickName)
        {
            EnsuredUtils.EnsureNewValueIsNotSame(NickName, nickName);

            NickName = EnsuredUtils.EnsureStringLengthIsCorrect(
                nickName,
                MIN_LENGHT,
                MAX_LENGTH);

            return default;
        }

        public Unit ResetPassword(string password)
        {
            EnsuredUtils.EnsureNewValueIsNotSame(Password, password);

            Password = EnsuredUtils.EnsurePasswordIsCorrect(
                password,
                PASSWORD_MIN_LENGHT,
                PASSWORD_MAX_LENGTH,
                _passswordPattern);

            return default;
        }

        public bool VerifyPassword(string password)
        {
            return Password.Equals(password);
        }

        public User(PersonalData personalData, string nickName, UserRole role, string password)
        {
            PersonalData = EnsuredUtils.EnsureNotNull(personalData);

            NickName = EnsuredUtils.EnsureStringLengthIsCorrect(
                nickName,
                MIN_LENGHT,
                MAX_LENGTH);

            Role = EnsuredUtils.EnsureNotNull(role);

            Password = EnsuredUtils.EnsurePasswordIsCorrect(
                password,
                PASSWORD_MIN_LENGHT,
                PASSWORD_MAX_LENGTH,
                _passswordPattern);
        }

        public User(UserDto userDto)
        {
            id = EnsuredUtils.EnsureStringIsNotEmpty(userDto.Id);

            var personalData = new PersonalData(
                userDto.Email,
                userDto.Name,
                userDto.LastName);

            PersonalData = personalData;

            NickName = EnsuredUtils.EnsureStringLengthIsCorrect(
                userDto.NickName,
                MIN_LENGHT,
                MAX_LENGTH);

            Role = new UserRole(userDto.Role);

            Password = EnsuredUtils.EnsurePasswordIsCorrect(
                userDto.Password,
                PASSWORD_MIN_LENGHT,
                PASSWORD_MAX_LENGTH,
                _passswordPattern);
        }
    }
}
