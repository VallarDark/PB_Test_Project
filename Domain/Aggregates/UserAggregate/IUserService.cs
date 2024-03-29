﻿using Contracts;
using Microsoft.FSharp.Core;
using System.Threading.Tasks;

namespace Domain.Aggregates.UserAggregate
{
    public interface IUserService
    {
        User? CurrentUser { get; }

        bool DoesUserHavePermission(UserRoleType permission);

        RepositoryType RepositoryType { get; }

        Task<TokenDto?> RegisterCasualUser(UserRegistrationDto userData);

        Task<TokenDto?> RegisterAdminUser(UserRegistrationDto userData);

        Task<TokenDto?> LoginUser(UserLoginDto userData);

        Task<Unit> LogOut();

        Task<Unit> ChangeRepositoryType(RepositoryType repositoryType);

        Task<User?> VerifyUser(UserValidationDto userValidationData);

        Task<PaginatedCollectionBase<User>> GetAllUsers(int pageNumber);

        Task<Unit> UpdateUserRole(string userId, UserRoleType roleType);
    }
}
