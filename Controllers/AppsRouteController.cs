using Microsoft.AspNetCore.Mvc;

namespace almondcove.Controllers
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
