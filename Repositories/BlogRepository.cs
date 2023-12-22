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
