using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace almondcove.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestApiController : ControllerBase
    {
        public IActionResult Something()
        {
            return Ok();
        }
    }
}
