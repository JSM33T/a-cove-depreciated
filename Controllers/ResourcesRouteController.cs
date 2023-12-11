using almondcove.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace almondcove.Controllers
{
    public class ResourcesRouteController : Controller
    {
        [Route("/resources")]
        public IActionResult Index()
        {
            return View("Views/Gallery/Index.cshtml");
        }

        [Route("/resources/browse")]
        public IActionResult Browse()
        {
            return View("Views/Gallery/Index.cshtml");
        }

        [Route("/resources/view/{slug}")]
        public IActionResult Viewer()
        {
            return View("Views/Gallery/Index.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
