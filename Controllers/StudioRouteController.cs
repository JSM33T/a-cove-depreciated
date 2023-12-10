using laymaann.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace laymaann.Controllers
{
    public class StudioRouteController : Controller
    {
        [Route("/studio")]
        public IActionResult Index()
        {
            return View("Views/Studio/Index.cshtml");
        }

        [Route("/music")]
        public IActionResult Music()
        {
            return View("Views/Music/Index.cshtml");
        }

        [Route("/music/album/{Slug}")]
        public IActionResult Album(string Slug)
        {
            return View("Views/Music/Albums/"+Slug.Trim()+".cshtml");
        }

        [Route("/music/single/{Slug}")]
        public IActionResult Single(string Slug)
        {
            return View("Views/Music/Singles/" + Slug.Trim() + ".cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
