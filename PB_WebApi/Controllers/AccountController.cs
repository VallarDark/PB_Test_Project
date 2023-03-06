using Contracts;
using Domain.Aggregates.UserAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PresentationModels.Models;

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
        private readonly IUserTokenProvider _tokenProvider;

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="userService"> User service</param>
        /// <param name="tokenProvider"> User token provider</param>

        public AccountController(
            IUserService userService,
            IUserTokenProvider tokenProvider)
        {
            _userService = userService;
            _tokenProvider = tokenProvider;
        }

        /// <summary>
        /// Registration
        /// </summary>
        /// <param name="registrationModel"> Registration model</param>
        /// <returns>HTTP success status | HTTP error</returns>

        [HttpPost("registration")]
        [AllowAnonymous]
        public async Task<IActionResult> Registration(UserRegistrationModel registrationModel)
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

            var userInfo = new UserInfoDto
            {
                TokenDto = result,
                UserName = _userService.CurrentUser?.NickName
            };

            return Ok(userInfo);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginModel"> Login model</param>
        /// <returns>HTTP success status | HTTP error</returns>

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginModel loginModel)
        {
            var userLoginDto = new UserLoginDto()
            {
                Email = loginModel.Email,
                Password = loginModel.Password
            };

            var result = await _userService.LoginUser(userLoginDto);

            var userInfo = new UserInfoDto
            {
                TokenDto = result,
                UserName = _userService.CurrentUser?.NickName
            };

            return Ok(userInfo);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="tokenData"> Refresh token + JWT</param>
        /// <returns>HTTP success status | HTTP error</returns>

        [HttpPost("refreshToken")]
        [AllowAnonymous]
        public IActionResult RefreshToken([FromBody] RefreshTokenModel tokenData)
        {
            var tokenDto = new TokenDto
            {
                Token = tokenData.Token,
                RefreshToken = tokenData.RefreshToken
            };

            return Ok(_tokenProvider.RefreshToken(tokenDto));
        }

        /// <summary>
        /// LogOut
        /// </summary>
        /// <returns>HTTP success status</returns>

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _userService.LogOut();

            return Ok("Logged out");
        }

        /// <summary>
        /// Change repository type
        /// </summary>
        /// <returns>HTTP success status</returns>

        [HttpPost("changeRepository")]
        [Authorize]
        public async Task<IActionResult> ChangeRepository([FromBody] RepositoryType repositoryType)
        {

            await _userService.ChangeRepositoryType(repositoryType);

            return Ok("Repository changed");
        }
    }
}
