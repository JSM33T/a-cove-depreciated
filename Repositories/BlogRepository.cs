using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using almondcove.Models.DTO.BlogDTOs;
using almondcove.Modules;
using Microsoft.Data.SqlClient;

namespace almondcove.Repositories
{
    public class BlogRepository(IConfigManager configManager, ILogger<BlogRepository> logger) : IBlogRepository
    {
        private readonly IConfigManager _configManager = configManager;
        private readonly ILogger<BlogRepository> _logger = logger;

        public async Task<BlogLoadDTO> GetBlogBySlug(string slug)
        {
            using var connection = new SqlConnection(_configManager.GetConnString());
            await connection.OpenAsync();

            string extractQuery = @"
                SELECT a.Id, a.Tags, a.Title, a.PostContent, a.UrlHandle,YEAR(CONVERT(DATE, a.DatePosted)) AS Year, COUNT(b.blogid) AS LikeCount
                FROM TblBlogMaster a
                LEFT JOIN TblBlogLike b ON a.Id = b.blogid
                WHERE a.UrlHandle = @Slug
                GROUP BY a.Id, a.Tags, a.Title, a.UrlHandle, a.PostContent,a.DatePosted
            ";

            using var command = new SqlCommand(extractQuery, connection);
            command.Parameters.AddWithValue("@Slug", slug);

            using var reader = await command.ExecuteReaderAsync();
            if (reader.Read())
            {
                return new BlogLoadDTO
                {
                    Id = (int)reader["Id"],
                    Tags = reader["Tags"].ToString(),
                    Title = reader["Title"].ToString(),
                    Slug = reader["UrlHandle"].ToString(),
                    Year = reader["Year"].ToString(),
                    Likes = reader["LikeCount"].ToString()
                };
            }

            return null;
        }

        public async Task<List<BlogThumbsDTO>> GetTopBlogsAsync()
        {
            List<BlogThumbsDTO> entries = [];

            try
            {
                string sql;
                string connectionString = _configManager.GetConnString();

                using SqlConnection connection = new(connectionString);
                await connection.OpenAsync();

                sql = $@"
                           SELECT 
                            bm.ID, 
                            bm.Title, 
                            bm.Description, 
                            cat.Title as Category, 
                            cat.Locator,
                            bm.UrlHandle, 
                            bm.DatePosted, 
                            COALESCE(comment_counts.CommentCount, 0) AS CommentCount,
                            COALESCE(like_counts.LikeCount, 0) AS LikeCount
                        FROM TblBlogMaster bm
                        LEFT JOIN (
                            SELECT 
                                PostId, 
                                COUNT(Id) AS CommentCount
                            FROM TblBlogComment
                            GROUP BY PostId
                        ) AS comment_counts ON bm.ID = comment_counts.PostId
                        LEFT JOIN (
                            SELECT 
                                BlogId, 
                                COUNT(Id) AS LikeCount
                            FROM TblBlogLike
                            GROUP BY BlogId
                        ) AS like_counts ON bm.ID = like_counts.BlogId
                        LEFT JOIN TblBlogCategory cat ON bm.CategoryId = cat.Id
                        WHERE bm.IsActive = 1
                        ORDER BY bm.DatePosted DESC
                            ";

                using SqlCommand command = new(sql, connection);
                using SqlDataReader dataReader = await command.ExecuteReaderAsync();

                while (await dataReader.ReadAsync())
                {
                    BlogThumbsDTO entry = new()
                    {
                        Id = (int)dataReader["Id"],
                        Title = (string)dataReader["Title"],
                        Description = (string)dataReader["Description"],
                        DatePosted = (DateTime)dataReader["DatePosted"],
                        DateFormatted = DateTimeFormats.FormatDateOrRelative((DateTime)dataReader["DatePosted"]),
                        UrlHandle = (string)dataReader["UrlHandle"],
                        Comments = (int)dataReader["CommentCount"],
                        Likes = (int)dataReader["LikeCount"],
                        Category = (string)dataReader["Category"],
                        Locator = (string)dataReader["Locator"],
                    };
                    entries.Add(entry);
                }

                await connection.CloseAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("error in get top blogs:{msg}", ex.Message.ToString());
            }

            return entries;
        }
    }
}
