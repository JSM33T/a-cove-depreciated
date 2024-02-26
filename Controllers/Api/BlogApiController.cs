using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using almondcove.Models.DTO.BlogDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using System.Web;

namespace almondcove.Controllers.Api
{
    [ApiController]
    public class BlogApiController : ControllerBase
    {
        readonly string connectionString = "";
        private readonly IConfigManager _configManager;
        private readonly ILogger<BlogApiController> _logger;
        private readonly IBlogRepository _blogRepo;
        public BlogApiController(IConfigManager configManager, ILogger<BlogApiController> logger, IBlogRepository blogRepository)
        {
            _configManager = configManager;
            _logger = logger;
            _blogRepo = blogRepository;
            connectionString = _configManager.GetConnString();
        }


        [EnableRateLimiting(policyName: "fixed")]
        [HttpGet("/api/topblogs/get")]
        public async Task<IActionResult> GetTopBlogs()
        {
            try {
                return Ok(await _blogRepo.GetTopBlogsAsync()); 
            }
            catch (Exception ex) {
                _logger.LogError("error fetchingblogs message: {message}", ex.Message); return BadRequest("error fetching blogs"); 
            }
        }

        [HttpGet]
        [Route("/api/blogs/{mode}/{classify}/{key}")]
        public async Task<IActionResult> GetBlogs(string mode, string classify, string key)
        {
            mode = "0";
            List<BlogThumbsDTO> thumbs = [];
            _ = new List<BlogThumbsDTO>();
            if (mode != "n")
            {
                using SqlConnection connection = new(connectionString);
                await connection.OpenAsync();
                string sql = "";
                if (classify != "na" && key != "na")
                {
                    if (classify == "category")
                    {
                        sql = "SELECT m.Id, m.Title,m.Description,m.UrlHandle,m.DatePosted,m.Tags,YEAR(m.DatePosted) AS Year,c.Title AS Category,c.Locator,COUNT(bc.Id) AS Comments FROM TblBlogMaster m " +
                              "LEFT JOIN TblBlogComment bc ON m.Id = bc.PostId " +
                              "JOIN TblBlogCategory c ON m.CategoryId = c.Id " +
                              "WHERE c.Locator = '" + key + "' and m.IsActive = 1" +
                              "GROUP BY m.Id, m.Title,m.Description,m.UrlHandle, m.DatePosted,m.Tags,c.Title,c.Locator " +
                              "ORDER BY m.DatePosted Desc OFFSET " + mode + " " +
                              "ROWS FETCH NEXT 5 ROWS ONLY";
                    }
                    else if (classify == "year")
                    {
                        sql = "SELECT m.Id, m.Title,m.Description,m.UrlHandle,m.DatePosted,m.Tags,YEAR(m.DatePosted) AS Year,c.Title AS Category,c.Locator,COUNT(bc.Id) AS Comments FROM TblBlogMaster m " +
                              "LEFT JOIN TblBlogComment bc ON m.Id = bc.PostId " +
                              "JOIN TblBlogCategory c ON m.CategoryId = c.Id " +
                              "WHERE m.CategoryId = c.Id and YEAR(m.DatePosted) = @key AND m.IsActive = 1" +
                              "GROUP BY m.Id, m.Title,m.Description,m.UrlHandle, m.DatePosted,m.Tags,c.Title,c.Locator " +
                              "ORDER BY  m.DatePosted Desc OFFSET @mode " +
                              "ROWS FETCH NEXT 5 ROWS ONLY";

                    }
                    else if (classify == "tag")
                    {
                        sql = "SELECT m.Id, m.Title,m.Description,m.UrlHandle,m.DatePosted,m.Tags,YEAR(m.DatePosted) AS Year,c.Title AS Category,c.Locator,COUNT(bc.Id) AS Comments FROM TblBlogMaster m " +
                             "LEFT JOIN TblBlogComment bc ON m.Id = bc.PostId " +
                             "JOIN TblBlogCategory c ON m.CategoryId = c.Id " +
                             "WHERE m.CategoryId = c.Id and Tags like '%" + key + "%' AND m.IsActive = 1" +
                             "GROUP BY m.Id, m.Title,m.Description,m.UrlHandle, m.DatePosted,m.Tags,c.Title,c.Locator " +
                             "ORDER BY  m.DatePosted Desc OFFSET " + mode + " " +
                             "ROWS FETCH NEXT 5 ROWS ONLY";
                    }
                    else if (classify == "search")
                    {
                        sql = "SELECT m.Id, m.Title,m.Description,m.UrlHandle,m.DatePosted,m.Tags,YEAR(m.DatePosted) AS Year,c.Title AS Category,c.Locator,COUNT(bc.Id) AS Comments FROM TblBlogMaster m " +
                             "LEFT JOIN TblBlogComment bc ON m.Id = bc.PostId " +
                             "JOIN TblBlogCategory c ON m.CategoryId = c.Id " +
                             "WHERE m.CategoryId = c.Id and (m.Title like '%" + key + "%' OR m.Description like '%" + key + "%'  OR m.Tags like '%" + key + "%' ) AND m.IsActive = 1" +
                             "GROUP BY m.Id, m.Title,m.Description,m.UrlHandle, m.DatePosted,m.Tags,c.Title,c.Locator " +
                             "ORDER BY  m.DatePosted Desc OFFSET " + mode + " " +
                             "ROWS FETCH NEXT 5 ROWS ONLY";
                    }
                }
                else
                {
                    sql = "SELECT m.Id, m.Title,m.Description,m.UrlHandle,m.DatePosted,m.Tags,YEAR(m.DatePosted) AS Year,c.Title AS Category,c.Locator,COUNT(bc.Id) AS Comments FROM TblBlogMaster m " +
                            "LEFT JOIN TblBlogComment bc ON m.Id = bc.PostId " +
                            "JOIN TblBlogCategory c ON m.CategoryId = c.Id " +
                            "WHERE m.CategoryId = c.Id  AND m.IsActive = 1" +
                            "GROUP BY m.Id, m.Title,m.Description,m.UrlHandle, m.DatePosted,m.Tags,c.Title,c.Locator " +
                            "ORDER BY  m.DatePosted Desc OFFSET " + mode + " " +
                            "ROWS FETCH NEXT 5 ROWS ONLY";
                }
                using SqlCommand command = new(sql, connection);
                command.Parameters.AddWithValue("@key", key);
                command.Parameters.AddWithValue("@mode", mode);
                using SqlDataReader dataReader = await command.ExecuteReaderAsync();
                if (dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        thumbs.Add(new BlogThumbsDTO
                        {
                            Title = dataReader.GetString(1),
                            Description = dataReader.GetString(2),
                            UrlHandle = dataReader.GetString(3),
                            DatePosted = dataReader.GetDateTime(4),
                            Category = dataReader.GetString(7),
                            Locator = dataReader.GetString(8),
                            Comments = dataReader.GetInt32(9)
                        });
                    }
                }
                await connection.CloseAsync();
            }
            return new JsonResult(thumbs);
        }

        [HttpGet]
        [Route("/api/blog/{Slug}/authors")]

        public async Task<IActionResult> LoadAuthors(string Slug)
        {
            List<object> data = [];
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(@"
                SELECT ba.AuthorId,u.UserName,u.Bio,u.FirstName,u.LastName,avt.Image FROM TblBlogMaster AS b 
                JOIN TblBlogAuthor AS ba ON ba.BlogId = b.Id 
                JOIN TblUserProfile AS u ON ba.AuthorId = u.Id 
                JOIN TblAvatarMaster AS avt ON u.AvatarId = avt.Id where b.UrlHandle = @UrlHandle 
            ", connection);
            command.Parameters.AddWithValue("@UrlHandle", Slug);
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var row = new
                {
                    userId = reader.GetInt32(0),
                    userName = reader.GetString(1),
                    userBio = reader.GetString(2),
                    firstName = reader.GetString(3),
                    lastName = reader.GetString(4),
                    avatarImage = reader.GetString(5),
                };

                data.Add(row);
            }
            await reader.CloseAsync();
            await connection.CloseAsync();
            return new JsonResult(data);
        }

        [HttpGet]
        [Route("/api/blog/{Slug}/likes")]

        public async Task<int> LoadLikes(string Slug)
        {
            List<object> data = [];
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT COUNT(*) FROM TblBlogLike a,TblBlogMaster b where b.UrlHandle = @UrlHandle  and b.Id = a.BlogId", connection);
                command.Parameters.AddWithValue("@UrlHandle", Slug);
                int reader = (int)await command.ExecuteScalarAsync();
                await connection.CloseAsync();
                return reader;
            }
            catch
            {
                return 0;
            }

        }


        [HttpPost("/api/blog/addlike")]
        public async Task<IActionResult> AddLike(BlogLikeDTO blogLike)
        {

            List<object> data = [];
            var SessionUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // var SessionUserId = 1;
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT COUNT(*) FROM TblBlogLike a,TblBlogMaster b where a.UserId = @userid and b.UrlHandle = @slug and b.Id = a.BlogId", connection);
                command.Parameters.AddWithValue("@userid", SessionUserId);
                command.Parameters.AddWithValue("@slug", blogLike.Slug);
                int likecounter = (int)await command.ExecuteScalarAsync();

                SqlCommand blogIdFind = new("SELECT Id from TblBlogMaster where UrlHandle = @blogslug", connection);
                blogIdFind.Parameters.AddWithValue("@blogslug", blogLike.Slug);
                int blogId = Convert.ToInt32(blogIdFind.ExecuteScalar());

                if (likecounter == 1)
                {
                    command = new SqlCommand("DELETE FROM TblBlogLike where UserId = @userid and BlogId = @blogid", connection);
                    command.Parameters.AddWithValue("@blogid", blogId);
                    command.Parameters.AddWithValue("@userid", SessionUserId);
                    command.ExecuteNonQuery();
                    await connection.CloseAsync();
                    return Ok("Like deleted");
                }
                else
                {
                    SqlCommand maxIdCommand = new("SELECT ISNULL(MAX(Id), 0) + 1 FROM TblBlogLike", connection);
                    int newLikeId = Convert.ToInt32(maxIdCommand.ExecuteScalar());

                    SqlCommand commandIns = new("INSERT INTO TblBlogLike(Id,BlogId,UserId,DateAdded) values(@id,@blogid,@userid,@dateadded)", connection);
                    commandIns.Parameters.AddWithValue("@id", newLikeId);
                    commandIns.Parameters.AddWithValue("@blogid", blogId);
                    commandIns.Parameters.AddWithValue("@userid", SessionUserId);
                    commandIns.Parameters.AddWithValue("@slug", blogLike.Slug);
                    commandIns.Parameters.AddWithValue("@dateadded", DateTime.Now);
                    commandIns.ExecuteNonQuery();
                    //   await connection.CloseAsync();
                    //   await TeleLog.Logstuff("*" + HttpContext.Session.GetString("username") + "* liked a blog:\n\"" + blogLike.Slug + "\"");
                    return Ok("Like added");


                }

            }
            catch (Exception ex)
            {
                _logger.LogError("load user's isLiked message {message}", ex.Message.ToString());
                return BadRequest("Something went wrong");
            }

        }

        [HttpPost]
        [Route("/api/blog/likestat")]
        public async Task<IActionResult> IsLiked(BlogLikeDTO blogLike)
        {

            if (!User.Identity.IsAuthenticated) return Ok(false);
            string LoggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            List<object> data = [];
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT COUNT(*) FROM TblBlogLike a,TblBlogMaster b where a.UserId = @userid and b.UrlHandle = @slug and b.Id = a.BlogId", connection);
                command.Parameters.AddWithValue("@userid", LoggedInUserId);
                command.Parameters.AddWithValue("@slug", blogLike.Slug);
                int likecounter = (int)await command.ExecuteScalarAsync();
                await connection.CloseAsync();
                return likecounter == 1 ? Ok(true) : Ok(false);

            }
            catch (Exception ex)
            {
                _logger.LogError("load user's isLiked message:" + ex.Message.ToString() + " user logged in:" + HttpContext.Session.GetString("username").ToString());
                return BadRequest();
            }

        }

        [HttpGet]
        [Route("/api/blogs/categories/load")]
        public async Task<IActionResult> LoadCategories()
        {
            List<object> data = [];
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                                        @"SELECT
                                            BC.Id AS CategoryId,
                                            BC.Title AS CategoryTitle,
                                            BC.Locator AS CategoryLocator,
                                            COALESCE(COUNT(BM.Id), 0) AS NumberOfActiveItems
                                        FROM 
                                            TblBlogCategory BC

                                        LEFT JOIN (
                                            SELECT CategoryId, Id
                                            FROM TblBlogMaster
                                            WHERE IsActive = 1 -- Add this condition to filter active blogs
                                        ) BM ON BC.Id = BM.CategoryId

                                        GROUP BY 
                                            BC.Id, BC.Title, BC.Locator

                                        ",
                                         connection);

            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var row = new
                {
                    id = reader.GetInt32(0),
                    title = reader.GetString(1),
                    locator = reader.GetString(2),
                    qty = reader.GetInt32(3)
                };

                data.Add(row);
            }
            await reader.CloseAsync();
            await connection.CloseAsync();
            return new JsonResult(data);
        }


        [Authorize]
        [HttpPost("/api/blog/comment/add")]
        public async Task<IActionResult> AddComment(BlogCommentDTO blogComment)
        {

           
            if (blogComment.Comment == null || blogComment.Slug == null) return BadRequest("invalid data");
            try
            {
                string blogid = "";
                string encodedcomment = HttpUtility.HtmlEncode(blogComment.Comment.ToString().Trim());
                if (encodedcomment.Length <= 3) return BadRequest("comment too short");
                    
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT Id FROM TblBlogMaster WHERE UrlHandle = @urlhandle", connection);
                command.Parameters.AddWithValue("@urlhandle", blogComment.Slug);
                var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync()) { blogid = reader.GetInt32(0).ToString(); }
                await reader.CloseAsync();

                SqlCommand maxIdCommand = new("SELECT ISNULL(MAX(Id), 0) + 1 FROM TblBlogComment", connection);
                int newId = Convert.ToInt32(maxIdCommand.ExecuteScalar());

                command = new SqlCommand("insert into TblBlogComment(Id,Comment,UserId,PostId,IsActive,DatePosted) values(" + newId + ",'" + encodedcomment + "'," + User.FindFirst(ClaimTypes.NameIdentifier).Value + "," + blogid + ",1,@dateposted)", connection);
                command.Parameters.Add("@dateposted", SqlDbType.DateTime).Value = DateTime.Now;
                await command.ExecuteNonQueryAsync();
                return Ok("Comment added");
                 

            }
            catch (Exception ex)
            {
                _logger.LogError("error in adding comment on a blog msg {msg} ", ex.Message.ToString());
                return BadRequest("Something went wrong");
            }
        }

        [HttpPost("/api/blog/comments/load")]
        
        public async Task<IActionResult> LoadComments(BlogCommentDTO blogComment)
        {

            Dictionary<int, dynamic> comments = [];
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(@"SELECT
						  c.Id,
						  c.Comment,
						  c.UserId,
						  u.FirstName,
						  u.LastName,
						  u.UserName as CommentUserName,
						  u.Id,
						  LEFT(c.DatePosted, 12),
						  a.Image,
						  r.Id AS ReplyId,
						  r.Reply AS ReplyComment,						  
						LEFT(r.DatePosted, 12),
						  u2.Id AS ReplyUserId,
						  u2.FirstName AS ReplyFirstName,
						  u2.LastName AS ReplyLastName,
						  a2.Image AS ReplyImage,
						u2.UserName AS ReplyUserName
						FROM
						  TblBlogComment c
						  JOIN TblUserProfile u ON c.UserId = u.Id
						  JOIN TblAvatarMaster a ON u.AvatarId = a.Id
						  LEFT JOIN TblBlogReply r ON c.Id = r.CommentId
                          LEFT JOIN TblBlogMaster bm ON bm.Id = c.PostId
						  LEFT JOIN TblUserProfile u2 ON r.UserId = u2.Id
						  LEFT JOIN TblAvatarMaster a2 ON u2.AvatarId = a2.Id
                        WHERE bm.UrlHandle = @posturl
						ORDER BY
						  c.DatePosted;
						", connection);
            command.Parameters.AddWithValue("@posturl", blogComment.Slug);
            var reader = await command.ExecuteReaderAsync();
            string user = "";
            string SessionUser = User.FindFirst(ClaimTypes.Name)?.Value;
            bool editable = false, replyeditable = false;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (SessionUser != null)
                    {
                        user = "yes";
                        try
                        {
                            editable = !string.IsNullOrEmpty(reader.GetString(5)) && reader.GetString(5) == SessionUser;
                        }
                        catch
                        {
                            editable = false;
                        }
                        try
                        {
                            replyeditable = !string.IsNullOrEmpty(reader.GetString(16)) && reader.GetString(16) == SessionUser;
                        }
                        catch
                        {
                            replyeditable = false;
                        }
                    }
                    else
                    {
                        user = "no";
                    }
                    var commentId = reader.GetInt32(0);
                    if (!comments.TryGetValue(commentId, out dynamic value))
                    {
                        var comment = new
                        {
                            id = commentId,
                            edit = editable,
                            user,
                            fullname = reader.GetString(3) + " " + reader.GetString(4),
                            userid = reader.GetInt32(2),
                            username = reader.GetString(5),
                            comment = HttpUtility.HtmlDecode(reader.GetString(1)),
                            date = reader.GetString(7),
                            avatar = reader.GetString(8),
                            replies = new List<object>()
                        };
                        value = comment;
                        comments.Add(commentId, value);
                    }
                    if (!reader.IsDBNull(9))
                    {
                        var reply = new
                        {
                            replyEdit = replyeditable,
                            user,
                            replyId = reader.GetInt32(9),
                            replyComment = HttpUtility.HtmlDecode(reader.GetString(10)),
                            replyUserId = reader.GetInt32(12),
                            replyDate = reader.GetString(11),
                            //replyFirstName = reader.GetString(13),
                            //replyLastName = reader.GetString(14),
                            replyFullName = reader.GetString(13) + " " + reader.GetString(14),
                            replyAvatar = reader.GetString(15)
                        };
                        value.replies.Add(reply);
                    }
                }
            }
            else
            {
                //return StatusCode(404);
                string errorMessage = "error";
                return BadRequest(new { error = errorMessage }); // HTTP 400 Bad Request
            }

            await reader.CloseAsync();
            await connection.CloseAsync();
            return new JsonResult(comments.Values);

        }

        [Authorize]
        [HttpPost]
        [Route("/api/blog/comment/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(BlogCommentDTO blogComment)
        {
                var Userdet = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                try
                {
                    using var connection = new SqlConnection(connectionString);
                    await connection.OpenAsync();
                    var sql = "UPDATE TblBlogComment SET Comment = @Commentval WHERE Id = @Idval AND UserId = @UserId";
                    var command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@Commentval", HttpUtility.HtmlEncode(blogComment.Comment));
                    command.Parameters.AddWithValue("@Idval", blogComment.Id);
                    command.Parameters.AddWithValue("@UserId", Userdet);
                    await command.ExecuteNonQueryAsync();
                    return Ok("changes saved");
                }
                catch (Exception ex)
                {
                    _logger.LogError("error while editing comment exception:{msg}", ex.Message);
                    return BadRequest("Something went wrong");
                }
        }

        [Authorize]
        [HttpPost("/api/blog/reply/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditrReply(BlogCommentDTO blogComment)
        {
           
                string Userdet = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                try
                {
                    using var connection = new SqlConnection(connectionString);
                    await connection.OpenAsync();
                    var sql = "UPDATE TblBlogReply SET Reply = @Replyval WHERE Id = @Idval AND UserId = @UserId ";
                    var command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@Replyval", HttpUtility.HtmlEncode(blogComment.Comment));
                    command.Parameters.AddWithValue("@Idval", blogComment.Id);
                    command.Parameters.AddWithValue("@UserId", Userdet);
                    await command.ExecuteNonQueryAsync();
                    await connection.CloseAsync();
                    return Ok("changes saved");
                }
                catch (Exception ex)
                {
                    _logger.LogError("error editing reply by user " + Userdet + "message:" + ex.Message.ToString());
                    return BadRequest("Something went wrong");
                }
        }


        [Authorize]
        [HttpPost("/api/blog/comment/delete")]
        public async Task<IActionResult> DeleteComment(BlogCommentDTO blogComment)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            try
            {
                using var command1 = connection.CreateCommand();
                command1.Transaction = transaction;
                command1.CommandText = "DELETE FROM TblBlogComment WHERE Id = @id and UserId = @user_id";
                command1.Parameters.AddWithValue("@id", blogComment.Id);
                command1.Parameters.AddWithValue("@user_id", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                await command1.ExecuteNonQueryAsync();

                using var command2 = connection.CreateCommand();
                command2.Transaction = transaction;
                command2.CommandText = "DELETE FROM TblBlogReply WHERE CommentId = @id";
                command2.Parameters.AddWithValue("@id", blogComment.Id);
                command2.Parameters.AddWithValue("@user_id", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await command2.ExecuteNonQueryAsync();
                transaction.Commit();
                return Ok("Commend deleted");

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError("error deleting comment: {msg}", ex.Message.ToString());
                return BadRequest("Something went wrong");
            }
        }

        [Authorize]
        [HttpPost("/api/blog/reply/add")]
        public async Task<IActionResult> AddReply(BlogReplyDTO blogReply)
        {
            string encodedreply = HttpUtility.HtmlEncode(blogReply.ReplyText.ToString().Trim());

            try
            {
                if (encodedreply.Trim() == "") return BadRequest("Reply too short");
                
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                SqlCommand maxIdCommand = new("SELECT ISNULL(MAX(Id), 0) + 1 FROM TblBlogReply", connection);
                int newId = Convert.ToInt32(maxIdCommand.ExecuteScalar());
                var command = new SqlCommand("insert into TblBlogReply(Id,CommentId,UserId,Reply,IsActive,DatePosted) values(" + newId + ",'" + blogReply.CommentId + "','" + User.FindFirst(ClaimTypes.NameIdentifier).Value + "','" + encodedreply + "',1,@dateposted)", connection);
                command.Parameters.Add("@dateposted", SqlDbType.DateTime).Value = DateTime.Now;
                await command.ExecuteNonQueryAsync();
                return Ok("reply added");

            }
            catch (Exception ex)
            {
                _logger.LogInformation("error in adding reply exc message: {eexmessage}", ex.Message);
                return BadRequest("Something went wrong");
            }
        }



        [Authorize]
        [HttpPost("/api/blog/reply/delete")]
        
        public async Task<IActionResult> DeleteReply(BlogReplyDTO blogReply)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                using var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM TblBlogReply WHERE Id = @id and UserId = @user_id";
                command.Parameters.AddWithValue("@id", blogReply.ReplyId);
                command.Parameters.AddWithValue("@user_id", User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await command.ExecuteNonQueryAsync();
                return Ok("Reply deleted");

            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(500,"Error deletng reply");
            }
        }


    }
}
