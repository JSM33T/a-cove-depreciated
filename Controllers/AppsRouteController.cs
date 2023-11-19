using Microsoft.AspNetCore.Mvc;

namespace almondCove.Controllers
{
    public class AppsRouteController : Controller
    {
        [Route("/apps")]
        public IActionResult Index()
        {
            return View("Views/Apps/Index.cshtml");
        }
    }
}
