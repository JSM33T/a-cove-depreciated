using almondCove.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace almondCove.Controllers
{
    public class GalleryModel
    {
        public List<AlbumInfo> Gallery { get; set; }
    }
    public class AlbumInfo {
        public int Album_id { get; set; }
        public string Album_name { get; set; }
        public string Album_desc { get; set; }
        public string Category { get; set; }
        public string Year{ get; set; }
        public bool IsActive{ get; set; }
    }
    public class GalleryRouteController : Controller
    {
        private readonly ILogger<GalleryRouteController> _logger;
        public GalleryRouteController(ILogger<GalleryRouteController> logger)
        {
            _logger = logger;
        }

        [Route("/gallery/")]
        public IActionResult Browse()
        {
            return View("Views/Gallery/Index.cshtml");
        }

        [Route("/gallery/browse")]
        public IActionResult Index()
        {
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "content","gallery", "gallery.json");

            if (System.IO.File.Exists(jsonFilePath))
            {
                string json = System.IO.File.ReadAllText(jsonFilePath);

                try
                {
                    GalleryModel galleryModel = JsonSerializer.Deserialize<GalleryModel>(json);
                    ViewData["GalleryData"] = galleryModel.Gallery;
                }
                catch (JsonException ex)
                {
                    ViewData["ErrorMessage"] = $"Error parsing JSON: {ex.Message}";
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Gallery data not found.";
            }

            return View("Views/Gallery/Browse.cshtml");
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
