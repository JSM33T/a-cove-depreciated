using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using almondcove.Models.Domain;
using almondcove.Models.DTO.Profile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace almondcove.Api
{
    [ApiController]
    public class ProfileApiController(IConfigManager _configuration, ILogger<ProfileApiController> _logger,IProfileRepository _profileRepo) : ControllerBase
    {

        [HttpGet]
        [Route("/api/profile/getdetails")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index()
        {
            var sessionStat = HttpContext.Session.GetString("role");

            if (sessionStat != null && (sessionStat == "user" || sessionStat == "admin"))
            {
                try 
                { 
                    return Ok( await _profileRepo.GetProfileByUsername(HttpContext.Session.GetString("username")) ); 
                }
                catch(Exception ex)
                {
                    _logger.LogError("exception in fetching profile details for user {msg}",ex.Message);
                    return BadRequest();
                }
                
            }
            else
            {
                _logger.LogError("unauth attempt to fetch profile details");
                return BadRequest("Access denied");
            }
        }

        [HttpGet("/api/getavatars")]
        public async Task<IActionResult> GetAvatars()
        {
            try
            {
                List<Avatar> entries = await _profileRepo.GetAvatarsAsync();

                return Ok(entries);
            }
            catch (Exception ex)
            {
                _logger.LogError("error fetching avatars with msg: {msg}", ex.Message);
                return BadRequest("Unable to fetch avatars");
            }
        }

        [HttpPost]
        [Route("/api/profile/password/update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(PasswordUpdateDTO passwordUpdateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Password Format");
            }

            string username = HttpContext.Session.GetString("username");

            if (passwordUpdateDTO.Password == username)
            {
                return BadRequest("Password can't be similar to your username");
            }

            try
            {
                bool updateResult = await _profileRepo.UpdatePassword(username, passwordUpdateDTO.Password);

                if (updateResult)
                {
                    return Ok("Changes Saved");
                }

                _logger.LogError("Failed to update password");
                return StatusCode(500, "Something went wrong");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating password. Message: {msg}", ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpPost]
        [Route("/api/profile/update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveProfile(ProfileUpdateDTO userProfileDTO)
        {
            UserProfile userProfile = new()
            {
                UserName = userProfileDTO.UserName,
                FirstName = userProfileDTO.FirstName,
                LastName = userProfileDTO.LastName,
                Gender = userProfileDTO.Gender,
                Bio = userProfileDTO.Bio,
                EMail = userProfileDTO.EMail,
                AvatarId = userProfileDTO.AvatarId
                
            };

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            try
            {
                using var connection = new SqlConnection(_configuration.GetConnString());
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
                 _logger.LogError("Error updating profile exception {exc}", ex.Message);
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
            var sql = "UPDATE TblUserProfile SET UserName = @UserName, FirstName = @FirstName, LastName = @LastName, AvatarId = @AvatarId, Gender = @Gender, Bio = @Bio,EMail = @EmailId, dateupdated = @dateupdated WHERE Id = @userid";
            using var command = new SqlCommand(sql, connection, transaction);
            command.Parameters.AddWithValue("@FirstName", userProfile.FirstName.Trim());
            command.Parameters.AddWithValue("@LastName", userProfile.LastName.Trim());
            command.Parameters.AddWithValue("@Gender", userProfile.Gender?.Trim() ?? "");
            command.Parameters.AddWithValue("@EmailId", userProfile.EMail.Trim());
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
            using var connection = new SqlConnection(_configuration.GetConnString());
            connection.Open();

            var sql = "SELECT Image FROM TblAvatarMaster WHERE Id = @avtrid";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@avtrid", avatarId);

            var avatar = (string)command.ExecuteScalar();

            return avatar;
        }

    }
}
