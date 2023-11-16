using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace almondCove.Api
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login()
        {
            return Ok();
        }

        [HttpPost("singup")]
        public IActionResult SignUp()
        {
            return Ok();
        }
    }
}
