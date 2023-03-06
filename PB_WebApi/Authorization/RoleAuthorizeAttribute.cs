using Domain.Aggregates.UserAggregate;
using Microsoft.AspNetCore.Authorization;

namespace PB_WebApi.Authorization
{
    internal class RequiredRoleAuthorizeAttribute : AuthorizeAttribute
    {
        const string POLICY_POSTFIX = "Role";
        private UserRoleType role;

        public UserRoleType Role
        {
            get => role;
            set
            {
                role = value;

                Policy = $"{Enum.GetName(role)}{POLICY_POSTFIX}";

                return;
            }
        }

        public RequiredRoleAuthorizeAttribute()
        {
            Role = UserRoleType.User;
        }

        public RequiredRoleAuthorizeAttribute(UserRoleType role)
        {
            Role = role;
        }
    }
}
