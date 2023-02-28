using Domain.Agregates.UserAgregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PB_WebApi.Authorization;

namespace PB_WebApi.Controllers
{
    [EnableCors("CrudPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [RequedRoleAuthorize(UserRoleType.User)]
        [HttpGet("Test"), Authorize]
        public async Task<IResult> Test()
        {
            return Results.Ok();
        }

        [RequedRoleAuthorize(UserRoleType.Admin)]
        [HttpGet("Test2"), Authorize]
        public async Task<IResult> Test2()
        {
            return Results.Ok();
        }
    }
}