using Domain.Agregates.UserAgregate;
using Microsoft.AspNetCore.Authorization;

namespace PB_WebApi.Authorization
{
    public class UserJwtAuthorizationHandler : IAuthorizationHandler
    {
        private readonly IUserService _userService;
        private readonly IUserTokenProvider _tokenProvider;

        public UserJwtAuthorizationHandler(IUserService userService, IUserTokenProvider tokenProvider)
        {
            _userService = userService;
            _tokenProvider = tokenProvider;
        }

        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            //if (context.HasFailed)
            //{
            //    return;
            //}

            //var result = context.User;

            //var r = context.Requirements;

            //foreach (var item in context.Requirements)
            //{
            //    context.Succeed(item);
            //}
        }
    }
}
