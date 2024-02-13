using almondcove.Filters;
using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using almondcove.Models.Domain;
using almondcove.Models.DTO.Account;
using almondcove.Models.DTO.Profile;
using almondcove.Modules;
using almondcove.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;

namespace almondcove.Controllers.Api
{
    [ApiController]
    [ValidateAntiForgeryToken]
    public class ProfileApiController(IConfigManager _configuration, ILogger<ProfileApiController> _logger, IProfileRepository _profileRepo) : ControllerBase
    {
        [HttpGet("/api/profile/getdetails")]
        public async Task<IActionResult> Index() => Ok(await _profileRepo.GetProfileByUsername( User.FindFirst(ClaimTypes.Name)?.Value));


        [HttpGet("/api/getavatars")]        
        public async Task<List<Avatar>> GetAvatars() => await _profileRepo.GetAvatarsAsync();


        [HttpPost]
        [Route("/api/profile/password/update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(PasswordUpdateDTO passwordUpdateDTO)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid Password Format");
            string username =  User.FindFirst(ClaimTypes.Name)?.Value;
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
                if (userProfile.UserName !=  User.FindFirst(ClaimTypes.Name)?.Value)
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
                new SqlParameter("@userid", SqlDbType.VarChar) { Value =  User.FindFirst(ClaimTypes.NameIdentifier)?.Value }
            ]);
            await command.ExecuteNonQueryAsync();
        }

        private async Task UpdateSessionVariables(UserProfile userProfile)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            // Update the existing claims or add new ones
            claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(ClaimTypes.Name));
            claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(ClaimTypes.GivenName));
            claimsIdentity.RemoveClaim(claimsIdentity.FindFirst("fullname"));
            claimsIdentity.RemoveClaim(claimsIdentity.FindFirst("avatar"));

            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, userProfile.UserName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName, userProfile.FirstName.Trim()));
            claimsIdentity.AddClaim(new Claim("fullname", userProfile.FirstName.Trim() + " " + userProfile.LastName.Trim()));
            claimsIdentity.AddClaim(new Claim("avatar", await GetAvatar(userProfile.AvatarId)));
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   new ClaimsPrincipal(claimsIdentity),
                   new AuthenticationProperties
                   {
                       IsPersistent = true, // Make the cookie persistent
                       ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(300)) // Adjust expiration time
                   }
            );
        }

        private async Task<string> GetAvatar(int avatarId)
        {
            Avatar avtr = await _profileRepo.GetAvatarByIdAsync(avatarId);
            return avtr.Image;
        }

    }
}
