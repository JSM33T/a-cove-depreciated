using almondcove.Models.DTO.Media.Gallery;
using Markdig;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace almondcove.Controllers.Routes
{
    public class GalleryRouteController(IWebHostEnvironment hostingEnvironment) : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;

        [Route("/gallery")]
        public IActionResult Browse()
        {
            IActionResult response = NotFound();
            string webRootPath = _hostingEnvironment.WebRootPath;
            string jsonFilePath = Path.Combine(webRootPath, "content", "gallery", "gallery.json");

            if (System.IO.File.Exists(jsonFilePath))
            {
                string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
                List<AlbumDTO> albumModel = JsonSerializer.Deserialize<List<AlbumDTO>>(jsonContent);
                response = View("Views/Gallery/Index.cshtml", albumModel);

            }
            return response;
        }


        [Route("/gallery/album/{Slug}")]
        public IActionResult Viewer(string Slug)
        {
            IActionResult response = NotFound();
            string webRootPath = _hostingEnvironment.WebRootPath;
            string jsonFilePath = Path.Combine(webRootPath, "content", "gallery", Slug, "content.json");
            string story = Path.Combine(webRootPath, "content", "gallery", Slug, "content.md");
            if (System.IO.File.Exists(jsonFilePath))
            {
                string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
                ViewData["gallery_slug"] = Slug;
                AlbumCollection albumModel = JsonSerializer.Deserialize<AlbumCollection>(jsonContent);
                string htmlContent = Markdown.ToHtml(System.IO.File.ReadAllText(story), new MarkdownPipelineBuilder().Build());
                ViewData["gallerystory"] = htmlContent;
                response = View("Views/Gallery/Viewer.cshtml", albumModel);
            }
            return response;
        }
    }
}
