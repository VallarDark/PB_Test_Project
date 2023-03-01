using Domain.Agregates.UserAgregate;
using Microsoft.AspNetCore.Authorization;

namespace PB_WebApi.Authorization
{
    internal class UserAuthorizationHandler : AuthorizationHandler<UserRoleRequirement>
    {
        private readonly IUserService _userService;

        public UserAuthorizationHandler(IUserService userService)
        {
            _userService = userService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            UserRoleRequirement requirement)
        {
            if (context.HasFailed)
            {
                return;
            }

            if (_userService.CurrentUser == null
                || _userService.CurrentUser.Role.CompareTo(requirement.RequiredRole) < 0)
            {
                context.Fail();

                return;
            };

            context.Succeed(requirement);
        }
    }
}
