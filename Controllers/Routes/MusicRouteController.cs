using almondcove.Models;
using almondcove.Models.Props;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace almondcove.Controllers.Routes
{
    public class MusicRouteController(IWebHostEnvironment hostingEnvironment,ILogger<MusicRouteController> logger) : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;
        private readonly ILogger<MusicRouteController> _logger = logger;

        [Route("/music")]
        public IActionResult Music()
        {
            return View("Views/Music/Index.cshtml");
        }

        [Route("/music/{Type}/{Slug}")]
        public IActionResult Single(string Type,string Slug)
        {
            
            string markdownFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "content/music/" + Type+ "/" + Slug );
            //fetch html content
            string htmlContent = System.IO.File.ReadAllText(markdownFilePath + "/content.html");
            //fetch seo/meta props
            string metaPropsFetch = System.IO.File.ReadAllText(markdownFilePath + "/props.json");
            JsonSerializerOptions jsonSerializerOptions = new()
            {
                // Enable case-insensitive deserialization
                PropertyNameCaseInsensitive = true,
                // Ignore null values
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, 
            };
            MetaProps propModel = JsonSerializer.Deserialize<MetaProps>(metaPropsFetch, jsonSerializerOptions);
            MetaProps meta = new()
            {
                Title = propModel?.Title ?? "Almondcove Music",
                Description = propModel?.Description ?? "Almondcove Music:  compilatio of bootlegs, originals and uch more form jsm33t & various artists",
                Tags = propModel?.Tags ?? "Music,almondcove,remixes,bootlegs,bollywood remixes,studio",
                Author = propModel?.Author ?? "Various artists",
            };

            ViewData["MetaProps"] = meta;
            ViewData["HtmlContent"] = new HtmlString(htmlContent);

            return View("Views/Music/Viewer.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
