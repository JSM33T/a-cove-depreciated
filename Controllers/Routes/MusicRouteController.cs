using almondcove.Interefaces.Repositories;
using almondcove.Models;
using almondcove.Models.DTO.BlogDTOs;
using almondcove.Models.Props;
using almondcove.Repositories;
using Markdig;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Diagnostics;
using System.Reflection;
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
            _logger.LogInformation(propModel.Author);
            MetaProps meta = new()
            {
                Title = propModel.Title,
                Description = propModel.Description,
                Tags = propModel.Tags,
                Author = propModel.Author,

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
