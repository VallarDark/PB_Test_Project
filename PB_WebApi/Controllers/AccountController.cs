using Domain.Agregates.UserAgregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PB_WebApi.Models;

namespace PB_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AccountController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("registration")]
        [AllowAnonymous]
        public async Task<IResult> Registration(UserRegistrationModel registrationModel)
        {
            var userDto = new UserDto()
            {
                Email = registrationModel.Email,
                LastName = registrationModel.LastName,
                Name = registrationModel.Name,
                NickName = registrationModel.NickName,
                Password = registrationModel.Password
            };

            var result = await _userService.RegisterCasualUser(userDto);

            return Results.Ok(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IResult> Login(UserLoginModel loginModel)
        {
            //if (!ModelState.IsValide(out var errors))
            //{
            //    return Results.BadRequest(errors);
            //}

            var userDto = new UserDto()
            {
                Email = loginModel.Email,
                Password = loginModel.Password
            };

            var result = await _userService.LoginUser(userDto);

            return Results.Ok(result);
        }
    }
}
