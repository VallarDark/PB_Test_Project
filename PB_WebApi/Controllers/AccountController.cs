using Domain.Agregates.UserAgregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PB_WebApi.Models;

namespace PB_WebApi.Controllers
{
    [EnableCors("CrudPolicy")]
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
    }
}
