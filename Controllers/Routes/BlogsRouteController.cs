using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using almondcove.Models;
using almondcove.Models.DTO.BlogDTOs;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace almondcove.Controllers.Routes
{

    public class BlogsRouteController(IWebHostEnvironment hostingEnvironment,IBlogRepository blogRepository) : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;
        //private readonly IConfigManager _configManager = configManager;
        private readonly IBlogRepository _blogRepo = blogRepository;

        [Route("/blogs/{subcat?}/{subcatb?}")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "ASP0018:Unused route parameter", Justification = "<Pending>")]
        public IActionResult Browse()
        {
            return View("Views/Blogs/Index.cshtml");
        }

        [Route("/blog/{Year}/{Slug}")]
        public async Task<IActionResult> Blogs(string Year, string Slug)
        {
            BlogLoadDTO blogLoad  =await _blogRepo.GetBlogBySlug(Slug);
            string markdownFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "content/blogs/" + blogLoad.Year + "/" + blogLoad.Slug + "/content.html");
            string markdownContent = System.IO.File.ReadAllText(markdownFilePath);
            string htmlContent = ConvertMarkdownToHtml(markdownContent);

            ViewData["HtmlContent"] = htmlContent;
            ViewData["blogdeet"] = blogLoad;

            return View("Views/Blogs/Viewer.cshtml");
        }

        private static string ConvertMarkdownToHtml(string markdownContent)
        {
            var pipeline = new MarkdownPipelineBuilder().Build();
            return Markdown.ToHtml(markdownContent, pipeline);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
