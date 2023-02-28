using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PB_WebApi.Controllers
{
    [EnableCors("CrudPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [Authorize()]
        [HttpGet("Test"), Authorize]
        public async Task<IResult> Test()
        {
            return Results.Ok();
        }
    }
}