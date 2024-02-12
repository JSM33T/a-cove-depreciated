using almondcove.Interefaces.Repositories;
using almondcove.Models;
using almondcove.Models.DTO.BlogDTOs;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace almondcove.Controllers.Routes
{

    public class BlogsRouteController(IWebHostEnvironment hostingEnvironment, IBlogRepository blogRepository) : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;
        private readonly IBlogRepository _blogRepo = blogRepository;

        [Route("/blogs/{subcat?}/{subcatb?}")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "ASP0018:Unused route parameter", Justification = "<Pending>")]
        public IActionResult Browse() => View("Views/Blogs/Index.cshtml");



        [Route("/blog/{Year}/{Slug}")]
        public async Task<IActionResult> RenderBlog(string Year, string Slug)
        {

            string mdContent = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.WebRootPath, $"content/blogs/{Year}/{Slug}/content.md"));
            string htmlContent = Markdown.ToHtml(mdContent, new MarkdownPipelineBuilder().Build());

            //string mdContent = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.WebRootPath, $"content/blogs/{Year}/{Slug}/content.html"));
            //string htmlContent = mdContent;

            ViewData["HtmlContent"] = htmlContent;
            ViewData["blogdeet"] = await _blogRepo.GetBlogBySlug(Slug) ?? new BlogLoadDTO();

            return View("Views/Blogs/Viewer.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        
    }
}
