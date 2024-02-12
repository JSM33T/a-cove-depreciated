using almondcove.Models;
using almondcove.Models.Props;
using almondcove.Modules;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace almondcove.Controllers.Routes
{
    public class MusicRouteController(IWebHostEnvironment hostingEnvironment, ILogger<MusicRouteController> logger) : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;
        private readonly ILogger<MusicRouteController> _logger = logger;

        [Route("/music")]
        public IActionResult Music() => View("Views/Music/Index.cshtml");

        //[Route("/music/{Type}/{Slug}")]
        //public IActionResult Single(string Type, string Slug)
        //{

        //    string markdownFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "content/music/" + Type + "/" + Slug);

        //    string htmlContent = System.IO.File.ReadAllText(markdownFilePath + "/content.html");
        //    string metaPropsFetch = System.IO.File.ReadAllText(markdownFilePath + "/props.json");

        //    MetaProps propModel = JsonSerializationHelper.Deserialize<MetaProps>(metaPropsFetch);
        //    MetaProps meta = new()
        //    {
        //        Title = propModel?.Title ?? "Almondcove Music",
        //        Description = propModel?.Description ?? "Almondcove Music:  compilatio of bootlegs, originals and much more form jsm33t & various artists",
        //        Tags = propModel?.Tags ?? "Music,almondcove,remixes,bootlegs,bollywood remixes,studio",
        //        Author = propModel?.Author ?? "Various artists",
        //    };

        //    ViewData["MetaProps"] = meta;
        //    ViewData["HtmlContent"] = new HtmlString(htmlContent);

        //    return View("Views/Music/Viewer.cshtml");
        //}


        [Route("/music/album/{Slug}")]
        public IActionResult Albums(string Slug) => View($"Views/Music/Albums/{Slug}.cshtml");

        [Route("/music/single/{Slug}")]
        public IActionResult Singles(string Slug) => View($"Views/Music/Singles/{Slug}.cshtml");


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
