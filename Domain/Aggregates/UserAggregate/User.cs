﻿using Contracts;
using Domain.Utils;
using Microsoft.FSharp.Core;
using System;
using System.Text.RegularExpressions;

namespace Domain.Aggregates.UserAggregate
{
    public class User : IEntity
    {
        private const string PASSWORD_PATTERN =
            @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{3,}$";

        public const int MIN_LENGHT = 6;
        public const int MAX_LENGTH = 20;
        public const int PASSWORD_MIN_LENGHT = 8;
        public const int PASSWORD_MAX_LENGTH = 50;

        private readonly static Regex _passwordPattern;

        private string id = Guid.NewGuid().ToString();

        public string Id => id;

        static User()
        {
            _passwordPattern = new Regex(PASSWORD_PATTERN);
        }

        public PersonalData PersonalData { get; private set; }

        public string NickName { get; private set; }

        public string Password { get; private set; }

        public string SessionToken { get; private set; }

        public UserRole Role { get; private set; }

        public RepositoryType RepositoryType { get; private set; }

        public Unit UpdateRole(UserRole role)
        {
            EnsuredUtils.EnsureNotNull(role);

            var result = EnsuredUtils.EnsureNewValueIsNotSame(Role, role);

            Role = role;

            return result;
        }

        public Unit GenerateNewSessionToken()
        {
            SessionToken = Guid.NewGuid().ToString();

            return default;
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

        public Unit ChangeRepositoryType(RepositoryType repositoryType)
        {
            RepositoryType = repositoryType;

            return default;
        }

        public Unit ResetPassword(string password)
        {
            EnsuredUtils.EnsureNewValueIsNotSame(Password, password);

            Password = EncodingUtils.EncodeData(EnsuredUtils.EnsurePasswordIsCorrect(
                password,
                PASSWORD_MIN_LENGHT,
                PASSWORD_MAX_LENGTH,
                _passwordPattern));

            return default;
        }

        public bool VerifyPassword(string password)
        {
            return Password.Equals(EncodingUtils.EncodeData(password));
        }

        public bool VerifyPasswordByHash(string passwordHash)
        {
            return EncodingUtils.GetHashCode(Password).Equals(passwordHash);
        }

        public bool DoesHavePermission(UserRoleType permission)
        {
            return Role.CompareTo(permission) >= 0;
        }

        public User(
            PersonalData personalData,
            string nickName,
            UserRole role,
            string password)
        {
            PersonalData = EnsuredUtils.EnsureNotNull(personalData);

            NickName = EnsuredUtils.EnsureStringLengthIsCorrect(
                nickName,
                MIN_LENGHT,
                MAX_LENGTH);

            Role = EnsuredUtils.EnsureNotNull(role);

            Password = EncodingUtils.EncodeData(EnsuredUtils.EnsurePasswordIsCorrect(
                password,
                PASSWORD_MIN_LENGHT,
                PASSWORD_MAX_LENGTH,
                _passwordPattern));

            SessionToken = Guid.NewGuid().ToString();
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

            Password = EncodingUtils.EncodeData(
                EnsuredUtils.EnsurePasswordIsCorrect(
                    EncodingUtils.DecodeData(userDto.Password),
                    PASSWORD_MIN_LENGHT,
                    PASSWORD_MAX_LENGTH,
                    _passwordPattern));

            SessionToken = userDto.SessionToken;
        }
    }
}
