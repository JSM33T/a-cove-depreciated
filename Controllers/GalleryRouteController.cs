using almondcove.Models.DTO.Media.Gallery;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace almondcove.Controllers
{
    public class GalleryRouteController(ILogger<GalleryRouteController> logger, IWebHostEnvironment hostingEnvironment) : Controller
    {
        private readonly ILogger<GalleryRouteController> _logger = logger;
        private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;

        [Route("/gallery")]
        public IActionResult Browse()
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string jsonFilePath = Path.Combine(webRootPath, "content", "gallery","gallery.json");
            _logger.LogError(jsonFilePath + " and " + webRootPath);
            if (System.IO.File.Exists(jsonFilePath))
            {
                string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
                List<AlbumDTO> albumModel = JsonSerializer.Deserialize<List<AlbumDTO>>(jsonContent);
                _logger.LogError(albumModel.ToString());
                return View("Views/Gallery/Index.cshtml", albumModel);
            }
            else
            {
                return NotFound();
            }
        }


        [Route("/gallery/album/{Slug}")]
        public IActionResult Viewer(string Slug)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string jsonFilePath = Path.Combine(webRootPath, "content","gallery", Slug, "content.json");
            _logger.LogError(jsonFilePath + " and " + webRootPath);
            if (System.IO.File.Exists(jsonFilePath))
            {
                // Read the JSON file content
                string jsonContent = System.IO.File.ReadAllText(jsonFilePath);

                List<AlbumItemsDTO> albumModel = JsonSerializer.Deserialize<List<AlbumItemsDTO>>(jsonContent);
                return View("Views/Gallery/Viewer.cshtml", albumModel);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
