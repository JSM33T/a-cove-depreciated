using Microsoft.AspNetCore.Mvc;

namespace almondCove.Controllers
{
    public class TestRouteController : Controller
    {
        [Route("/test")]
        public IActionResult Rough()
        {
            return View("Views/Test/Index.cshtml");
        }
    }
}
