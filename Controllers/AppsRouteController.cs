using Microsoft.AspNetCore.Mvc;

namespace laymaann.Controllers
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
