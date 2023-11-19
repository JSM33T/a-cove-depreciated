using Markdig;
using Microsoft.AspNetCore.Mvc;

namespace almondCove.Controllers
{
    public class TestRouteController : Controller
    { 
        private readonly IWebHostEnvironment _hostingEnvironment;
        public TestRouteController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
    
        [Route("/test")]
        public IActionResult Rough()
        {

            //var markdownFilePath = Path.Combine(_hostingEnvironment.WebRootPath,"something.md");

            //// Read the content of the Markdown file
            //string markdownContent = System.IO.File.ReadAllText(markdownFilePath);

            //// Convert Markdown to HTML using Markdig
            //string htmlContent = ConvertMarkdownToHtml(markdownContent);

            //// Pass the HTML content to the view
            //ViewData["HtmlContent"] = htmlContent;

            return View("/Views/Test/Index.cshtml");

        }

        private static string ConvertMarkdownToHtml(string markdownContent)
        {
            // Use Markdig to perform the conversion
            var pipeline = new MarkdownPipelineBuilder().Build();
            return Markdig.Markdown.ToHtml(markdownContent, pipeline);
        }
    }
}
