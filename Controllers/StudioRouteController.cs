using almondCove.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace almondCove.Controllers
{
    public class StudioModel
    {
        public List<AlbumInfo> Studio { get; set; }
    }
    public class StudioInfo
    {
        public int Album_id { get; set; }
        public string Album_name { get; set; }
        public string Album_desc { get; set; }
        public string Category { get; set; }
        public string Year { get; set; }

        public string Artists { get; set; }
        public bool IsActive { get; set; }
    }
    public class StudioRouteController : Controller
    {
        [Route("/studio")]
        public IActionResult Index()
        {
            return View("Views/Gallery/Index.cshtml");
        }

        [Route("/studio/browse")]
        public IActionResult Browse()
        {
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "content", "studio", "studio.json");

            if (System.IO.File.Exists(jsonFilePath))
            {
                string json = System.IO.File.ReadAllText(jsonFilePath);

                try
                {
                    GalleryModel galleryModel = JsonSerializer.Deserialize<GalleryModel>(json);
                    ViewData["StudioData"] = galleryModel.Gallery;
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

            return View("Views/Studio/Browse.cshtml");
        }

        [Route("/studio/view/{slug}")]
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
