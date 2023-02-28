using Domain.Agregates.UserAgregate;
using Microsoft.AspNetCore.Authorization;

namespace PB_WebApi.Authorization
{
    internal class RequedRoleAuthorizeAttribute : AuthorizeAttribute
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

        public RequedRoleAuthorizeAttribute()
        {
            Role = UserRoleType.User;
        }

        public RequedRoleAuthorizeAttribute(UserRoleType role)
        {
            Role = role;
        }
    }
}
