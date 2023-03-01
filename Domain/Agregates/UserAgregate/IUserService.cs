﻿using Contracts;
using Microsoft.FSharp.Core;
using System.Threading.Tasks;

namespace Domain.Agregates.UserAgregate
{
    public interface IUserService
    {
        User? CurrentUser { get; }

        bool DoesUserHavePermission(UserRoleType permission);

        RepositoryType RepositoryType { get; set; }

        Task<string> RegisterCasualUser(UserRegistrationDto userData);

        Task<string> RegisterAdminUser(UserRegistrationDto userData);

        Task<string> LoginUser(UserLoginDto userData);

        Task<User?> VerifyUser(UserValidationDto userValidationData);

        Task<PaginatedCollectionBase<User>> GetAllUsers(int pageNumber);

        Task<Unit> UpdateUserRole(string userId, UserRoleType roleType);
    }
}
