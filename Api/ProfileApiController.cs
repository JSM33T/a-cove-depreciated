using almondCove.Models.Domain;
using almondCove.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace almondCove.Api
{
    [ApiController]
    public class ProfileApiController : ControllerBase
    {
        private readonly IConfigManager _configManager;
        public ProfileApiController(IConfigManager configuration)
        {
            _configManager = configuration;
        }


        [HttpGet]
        [Route("/api/profile/getdetails")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index()
        {
            UserProfile userProfile = null;
            string connectionString = _configManager.GetConnString();
            var sessionStat = HttpContext.Session.GetString("role");

            if (sessionStat != null && (sessionStat == "user" || sessionStat == "admin"))
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT a.FirstName,a.LastName,a.UserName,a.Role,a.Gender,a.Bio,a.DateJoined,a.Phone,a.EMail, b.Image FROM TblUserProfile a, TblAvatarMaster b WHERE UserName = @username and a.AvatarId = b.Id", connection);
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
                        Phone = reader.GetString(7),
                        EMail = reader.GetString(8),
                        AvatarImg = reader.GetString(9),

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
    }
}
