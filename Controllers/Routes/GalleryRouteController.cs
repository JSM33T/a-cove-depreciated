using almondcove.Models.DTO.Media.Gallery;
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
                //todo- viewdata over passing model as whole
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
            if (System.IO.File.Exists(jsonFilePath))
            {
                string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
                ViewData["gallery_slug"] = Slug;
                List<AlbumItemsDTO> albumModel = JsonSerializer.Deserialize<List<AlbumItemsDTO>>(jsonContent);
                //todo- viewdata over passing model as whole
                response = View("Views/Gallery/Viewer.cshtml", albumModel);
            }
            return response;
        }
    }
}
