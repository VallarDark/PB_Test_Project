using Domain.Aggregates.UserAggregate;
using Microsoft.AspNetCore.Authorization;

namespace PB_WebApi.Authorization
{
    internal class UserRoleRequirement : IAuthorizationRequirement
    {
        public UserRoleType RequiredRole { get; set; }

        public UserRoleRequirement(UserRoleType requiredRole)
        {
            RequiredRole = requiredRole;
        }
    }
}
