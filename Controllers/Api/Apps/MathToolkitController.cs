using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace almondcove.Controllers.Api.Apps
{
    [ApiController]
    public class MathToolkitController(ILogger<MathToolkitController> logger) : ControllerBase
    {
        private readonly ILogger<MathToolkitController> _logger = logger;

        //public IActionResult FibonacciSum()
        //{
        //    _logger.LogError("message here :{msg}", "something");
        //    return Ok(); 
        //}

        //public IActionResult FibonacciNthTerm()
        //{
        //    _logger.LogError("message here :{msg}", "something");
        //    return Ok();
        //}
    }
}
