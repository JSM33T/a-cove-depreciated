using almondCove.Api;
using almondCove.Interefaces.Services;
using almondCove.Models;
using almondCove.Modules;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace almondCove.Controllers
{
    public class BlogLoad
    {
        public int Id { get; set; }
        public string Tags { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Year { get; set; }
        public string Likes { get; set; }
        public string Content { get; set; }
    }
    public class BlogsRouteController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfigManager _configManager;
        public BlogsRouteController(IWebHostEnvironment hostingEnvironment,IConfigManager configManager)
        {
            _hostingEnvironment = hostingEnvironment;
            _configManager = configManager;
        }

        [Route("/blogs")]
        public IActionResult Browse()
        {
            return View("Views/Blogs/Index.cshtml");
        }

        [HttpGet]
        [Route("/blog/{Year}/{Slug}")]
        public IActionResult Blogs(string Year, string Slug)
        {
            string connectionString = _configManager.GetConnString();
            BlogLoad blogLoad = null;
            var markdownFilePath = "";
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            var command = new SqlCommand("SELECT a.Id, a.Tags, a.Title,a.PostContent, a.UrlHandle, COUNT(b.blogid) AS LikeCount FROM TblBlogMaster a LEFT JOIN TblBlogLike b ON a.Id = b.blogid WHERE a.UrlHandle = '" + Slug + "' GROUP BY a.Id, a.Tags, a.Title, a.UrlHandle,a.PostContent; ", connection);
            var reader = command.ExecuteReader();
            string tags = string.Empty;

            if (reader.Read())
            {
                blogLoad = new()
                {
                    Id = (int)reader["Id"],
                    Tags = reader["Tags"].ToString(),
                    Title = reader["Title"].ToString(),
                    Slug = reader["UrlHandle"].ToString(),
                    Year = Year,
                    Likes = reader["LikeCount"].ToString()
                };
                 markdownFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "content/blogs/" + blogLoad.Year + "/" + blogLoad.Slug + "/content.md");
            }
            reader.Close();
            

            string markdownContent = System.IO.File.ReadAllText(markdownFilePath);
            string htmlContent = ConvertMarkdownToHtml(markdownContent);

            ViewData["HtmlContent"] = htmlContent;
            ViewData["blogdeet"] = blogLoad;
           
            return View("Views/Blogs/Viewer.cshtml");
        }

        //dedicated methods
        private static string ConvertMarkdownToHtml(string markdownContent)
        {
            // Use Markdig to perform the conversion
            var pipeline = new MarkdownPipelineBuilder().Build();
            return Markdig.Markdown.ToHtml(markdownContent, pipeline);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
