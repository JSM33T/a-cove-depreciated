using laymaann.Api;
using laymaann.Interefaces.Services;
using laymaann.Models;
using laymaann.Models.DTO;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace laymaann.Controllers
{
    
    public class BlogsRouteController(IWebHostEnvironment hostingEnvironment, IConfigManager configManager) : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;
        private readonly IConfigManager _configManager = configManager;

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
            BlogLoadDTO blogLoad = null;
            var markdownFilePath = "";
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            string extractQuery = @"
                SELECT a.Id, a.Tags, a.Title,a.PostContent, a.UrlHandle, COUNT(b.blogid) AS LikeCount FROM TblBlogMaster a 
                LEFT JOIN TblBlogLike b ON a.Id = b.blogid 
                WHERE a.UrlHandle = @Slug 
                GROUP BY a.Id, a.Tags, a.Title, a.UrlHandle,a.PostContent
            ";
            var command = new SqlCommand(extractQuery, connection);
            command.Parameters.AddWithValue("@Slug", Slug);
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
