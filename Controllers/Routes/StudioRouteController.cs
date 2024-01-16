using almondcove.Models.DTO.Media.Gallery;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace almondcove.Controllers.Routes
{
    public class StudioRouteController(IWebHostEnvironment hostingEnvironment) : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;

        [Route("/studio")]
        public IActionResult Browse()
        {
            string jsonFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "content", "studio", "studio.json");

            IActionResult response = System.IO.File.Exists(jsonFilePath)
                ? View("Views/Studio/Index.cshtml", DeserializeJsonFile<List<AlbumDTO>>(jsonFilePath))
                : NotFound();

            return response;
        }
        

        [Route("/studio/lr-preset/{Slug}")]
        public IActionResult Viewer(string Slug)
        {
            string jsonFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "content", "studio", "lr-presets", Slug, "content.json");

            IActionResult response = System.IO.File.Exists(jsonFilePath)
                ? View("Views/Gallery/Viewer.cshtml", DeserializeJsonFile<List<AlbumItemsDTO>>(jsonFilePath))
                : NotFound();

            ViewData["gallery_slug"] = Slug;

            return response;
        }

        private static T DeserializeJsonFile<T>(string filePath)
        {
            string jsonContent = System.IO.File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(jsonContent);
        }
    }
}
