using almondcove.Interefaces.Services;
using almondcove.Models.Domain;
using almondcove.Modules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace almondcove.Api
{
    [ApiController]
    public class ProfileApiController(IConfigManager configuration, ILogger<ProfileApiController> logger) : ControllerBase
    {
                [HttpGet]
        [Route("/api/profile/getdetails")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index()
        {
            UserProfile userProfile = null;
            string connectionString = configuration.GetConnString();
            var sessionStat = HttpContext.Session.GetString("role");

            if (sessionStat != null && (sessionStat == "user" || sessionStat == "admin"))
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand(@"
                                SELECT a.FirstName,a.LastName,a.UserName,a.Role,a.Gender,a.Bio,a.DateJoined,a.EMail, b.Image ,b.Id
                                FROM TblUserProfile a, TblAvatarMaster b 
                                WHERE UserName = @username and a.AvatarId = b.Id
                ", connection);
                command.Parameters.AddWithValue("@username", HttpContext.Session.GetString("username"));
                var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    userProfile = new UserProfile()
                    {
                        FirstName = reader.GetString(0),
                        LastName = reader.GetString(1),
                        UserName = reader.GetString(2),
                        Role = reader.GetString(3),
                        Gender = reader.GetString(4),
                        Bio = reader.GetString(5),
                        DateElement = reader.GetDateTime(6).ToString("yyyy-MM-dd"),
                        EMail = reader.GetString(7),
                        AvatarImg = reader.GetString(8),
                        AvatarId = reader.GetInt32(9)

                    };
                }

                await reader.CloseAsync();
                await connection.CloseAsync();

                return Ok(userProfile);
            }
            else
            {
                return BadRequest("Access denied" );
            }
        }

        [HttpGet]
        [Route("/api/getavatars")]
        public async Task<IActionResult> GetAvatars()
        {
            try
            {
                using SqlConnection connection = new(configuration.GetConnString());
                await connection.OpenAsync();

                string sql = "SELECT * FROM TblAvatarMaster";

                using SqlCommand command = new(sql, connection);
                using SqlDataReader dataReader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                List<Avatar> entries = new();

                while (await dataReader.ReadAsync())
                {
                    Avatar entry = new()
                    {
                        Id = dataReader["Id"] as int? ?? 0,
                        Title = dataReader["Title"] as string ?? "",
                        Image = dataReader["Image"] as string ?? ""
                    };
                    entries.Add(entry);
                }

                return Ok(entries);
            }
            catch (SqlException ex)
            {
                logger.LogError("SQL error in GetAvatars exception: {message}", ex.Message);
                return BadRequest("Unable to fetch avatars");
            }
        }

        [HttpPost]
        [Route("api/profile/password/update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword([FromBody] UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                if (userProfile.Password == HttpContext.Session.GetString("username").ToString())
                {
                    return BadRequest("Password can't be similar to your username");
                }
                else
                {
                    try
                    {
                        string LoggedUser = HttpContext.Session.GetString("username").ToString();
                        using SqlConnection connection = new(configuration.GetConnString());

                        await connection.OpenAsync();
                        SqlCommand insertCommand = new(@"
                                    UPDATE TblUserProfile SET CryptedPassword = @cryptedpassword,DateUpdated = @dateupdated 
                                    where UserName = @username"
                        , connection);
                        insertCommand.Parameters.AddWithValue("@username", LoggedUser);
                        insertCommand.Parameters.AddWithValue("@cryptedpassword", EnDcryptor.Encrypt(userProfile.Password,configuration.GetCryptKey()));
                        insertCommand.Parameters.Add("@dateupdated", SqlDbType.DateTime).Value = DateTime.Now;

                        await insertCommand.ExecuteNonQueryAsync();
                        await connection.CloseAsync();
                        // // Log.Information(LoggedUser + " changed their password");
                        return Ok("Changes Saved");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("error updating profile:" + ex.Message.ToString());
                        return BadRequest("Something went wrong");
                    }
                }

            }
            else
            {
                return BadRequest("Invalid Password Format");
            }
        }

        [HttpPost]
        [Route("/api/profile/update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveProfile([FromBody] UserProfile userProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            try
            {
                using var connection = new SqlConnection(configuration.GetConnString());
                await connection.OpenAsync();

                using var transaction = connection.BeginTransaction();
                if (userProfile.UserName != HttpContext.Session.GetString("username"))
                {
                    if (await IsUsernameTakenAsync(connection, userProfile.UserName, transaction))
                    {
                        transaction.Rollback();
                        return BadRequest("Username taken");
                    }
                }

                await UpdateUserProfileAsync(connection, userProfile, transaction);

                transaction.Commit();

                UpdateSessionVariables(userProfile);

                return Ok("Changes Saved");
            }
            catch (Exception ex)
            {
                 logger.LogError("Error updating profile exception {exc}", ex.Message);
                return BadRequest("Something went wrong");
            }
        }

        private static async Task<bool> IsUsernameTakenAsync(SqlConnection connection, string username, SqlTransaction transaction)
        {
            var sql = "SELECT COUNT(*) FROM TblUserProfile WHERE UserName = @username";
            using var command = new SqlCommand(sql, connection, transaction);
            command.Parameters.AddWithValue("@username", username.Trim());
            var count = (int)await command.ExecuteScalarAsync();
            return count != 0;
        }

        private async Task UpdateUserProfileAsync(SqlConnection connection, UserProfile userProfile, SqlTransaction transaction)
        {
            var sql = "UPDATE TblUserProfile SET UserName = @UserName, FirstName = @FirstName, LastName = @LastName, AvatarId = @AvatarId, Gender = @Gender, Bio = @Bio, dateupdated = @dateupdated WHERE Id = @userid";
            using var command = new SqlCommand(sql, connection, transaction);
            command.Parameters.AddWithValue("@FirstName", userProfile.FirstName.Trim());
            command.Parameters.AddWithValue("@LastName", userProfile.LastName.Trim());
            command.Parameters.AddWithValue("@Gender", userProfile.Gender.Trim());
            command.Parameters.AddWithValue("@AvatarId", userProfile.AvatarId);
            command.Parameters.AddWithValue("@dateupdated", DateTime.Now);
            command.Parameters.AddWithValue("@UserName", userProfile.UserName.Trim());
            command.Parameters.AddWithValue("@Bio", userProfile.Bio.Trim());
            command.Parameters.AddWithValue("@userid", HttpContext.Session.GetString("user_id"));
            await command.ExecuteNonQueryAsync();
        }

        private void UpdateSessionVariables(UserProfile userProfile)
        {
            HttpContext.Session.SetString("username", userProfile.UserName);
            HttpContext.Session.SetString("first_name", userProfile.FirstName.Trim());
            HttpContext.Session.SetString("fullname", userProfile.FirstName.Trim() + " " + userProfile.LastName.Trim());
            HttpContext.Session.SetString("avatar", GetAvatar(userProfile.AvatarId));
        }

        private string GetAvatar(int avatarId)
        {
            using var connection = new SqlConnection(configuration.GetConnString());
            connection.Open();

            var sql = "SELECT Image FROM TblAvatarMaster WHERE Id = @avtrid";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@avtrid", avatarId);

            var avatar = (string)command.ExecuteScalar();

            return avatar;
        }

    }
}
