using almondcove.Filters;
using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using almondcove.Models.Domain;
using almondcove.Models.DTO.Profile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace almondcove.Controllers.Api
{
    [ApiController]
    [ValidateAntiForgeryToken]
    public class ProfileApiController(IConfigManager _configuration, ILogger<ProfileApiController> _logger, IProfileRepository _profileRepo) : ControllerBase
    {
        [HttpGet("/api/profile/getdetails")]
        [Perm("admin", "user", "editor")]
        public async Task<IActionResult> Index() => Ok(await _profileRepo.GetProfileByUsername(HttpContext.Session.GetString("username")));


        [HttpGet("/api/getavatars")]        
        public async Task<List<Avatar>> GetAvatars() => await _profileRepo.GetAvatarsAsync();


        [HttpPost]
        [Route("/api/profile/password/update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(PasswordUpdateDTO passwordUpdateDTO)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid Password Format");
            string username = HttpContext.Session.GetString("username");
            if (passwordUpdateDTO.Password == username) return BadRequest("Password can't be similar to your username");
            bool updateResult = await _profileRepo.UpdatePassword(username, passwordUpdateDTO.Password);
            return updateResult ? Ok("Changes Saved") : StatusCode(500, "Something went wrong");
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

            if (!ModelState.IsValid) BadRequest("Invalid Data");

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

                _logger.LogInformation("id is {id} and image is{image}", userProfile.AvatarId, userProfile.AvatarImg);
                await UpdateSessionVariables(userProfile);

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
            var sql = @"
                UPDATE TblUserProfile 
                SET UserName = @UserName, 
                    FirstName = @FirstName, 
                    LastName = @LastName, 
                    AvatarId = @AvatarId, 
                    Gender = @Gender, 
                    Bio = @Bio,
                    EMail = @EmailId, 
                    dateupdated = @dateupdated 
                WHERE Id = @userid
            ";
            using var command = new SqlCommand(sql, connection, transaction);
            command.Parameters.AddRange(
            [
                new SqlParameter("@FirstName", SqlDbType.VarChar) { Value = userProfile.FirstName.Trim() },
                new SqlParameter("@LastName", SqlDbType.VarChar) { Value = userProfile.LastName.Trim() },
                new SqlParameter("@Gender", SqlDbType.VarChar) { Value = userProfile.Gender?.Trim() ?? "" },
                new SqlParameter("@EmailId", SqlDbType.VarChar) { Value = userProfile.EMail.Trim() },
                new SqlParameter("@AvatarId", SqlDbType.Int) { Value = userProfile.AvatarId },
                new SqlParameter("@dateupdated", SqlDbType.DateTime) { Value = DateTime.Now },
                new SqlParameter("@UserName", SqlDbType.VarChar) { Value = userProfile.UserName.Trim() },
                new SqlParameter("@Bio", SqlDbType.VarChar) { Value = userProfile.Bio.Trim() },
                new SqlParameter("@userid", SqlDbType.VarChar) { Value = HttpContext.Session.GetString("user_id") }
            ]);
            await command.ExecuteNonQueryAsync();
        }

        private async Task UpdateSessionVariables(UserProfile userProfile)
        {
            HttpContext.Session.SetString("username", userProfile.UserName);
            HttpContext.Session.SetString("first_name", userProfile.FirstName.Trim());
            HttpContext.Session.SetString("fullname", userProfile.FirstName.Trim() + " " + userProfile.LastName.Trim());
            HttpContext.Session.SetString("avatar",await GetAvatar(userProfile.AvatarId));
        }

        private async Task<string> GetAvatar(int avatarId)
        {
            Avatar avtr = await _profileRepo.GetAvatarByIdAsync(avatarId);
            return avtr.Image;
        }

    }
}
