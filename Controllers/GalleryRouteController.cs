using almondCove.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace almondCove.Controllers
{
    public class GalleryRouteController : Controller
    {
        [Route("/gallery")]
        public IActionResult Index()
        {
            return View("Views/Gallery/Index.cshtml");
        }

        [Route("/gallery/browse")]
        public IActionResult Browse()
        {
            return View("Views/Gallery/Index.cshtml");
        }

        [Route("/gallery/view/{slug}")]
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
