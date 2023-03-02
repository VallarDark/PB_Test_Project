using Domain.Agregates.UserAgregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PB_WebApi.Models;

namespace PB_WebApi.Controllers
{
    /// <summary>
    /// SignIn/SignOut Controller
    /// </summary>

    [EnableCors("CrudPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="userService"> User service</param>

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registration
        /// </summary>
        /// <param name="registrationModel"> Registration model</param>
        /// <returns>HTTP success status | HTTP error</returns>

        [HttpPost("registration")]
        [AllowAnonymous]
        public async Task<IResult> Registration(UserRegistrationModel registrationModel)
        {
            var userRegistrationDto = new UserRegistrationDto()
            {
                Email = registrationModel.Email,
                LastName = registrationModel.LastName,
                Name = registrationModel.Name,
                NickName = registrationModel.NickName,
                Password = registrationModel.Password
            };

            var result = await _userService.RegisterCasualUser(userRegistrationDto);

            return Results.Ok(result);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginModel"> Login model</param>
        /// <returns>HTTP success status | HTTP error</returns>

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IResult> Login(UserLoginModel loginModel)
        {
            var userLoginDto = new UserLoginDto()
            {
                Email = loginModel.Email,
                Password = loginModel.Password
            };

            var result = await _userService.LoginUser(userLoginDto);

            return Results.Ok(result);
        }

        /// <summary>
        /// LogOut
        /// </summary>
        /// <returns>HTTP success status</returns>

        [HttpPost("logout")]
        [Authorize]
        public async Task<IResult> LogOut()
        {
            await _userService.LogOut();

            return Results.Ok("log out");
        }
    }
}
