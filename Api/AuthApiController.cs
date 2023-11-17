using almondCove.Modules;
using almondCove.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace almondCove.Api
{

    public class LoginCreds
    {
        [Required]
        [MaxLength(50)]
        [MinLength(4)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(6)]
        public string Password { get; set; }

        public string Otp { get; set; }
    }

    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {

        private readonly IConfigManager _configManager;
        private readonly ILogger<AuthApiController> _logger;
        public AuthApiController(IConfigManager configManager, ILogger<AuthApiController> logger)
        {
            _configManager = configManager;
            _logger = logger;

        }

        [HttpPost("/api/account/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserLogin([FromBody] LoginCreds loginCreds)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Credentials");
            }
            try
            {
                using SqlConnection connection = new(_configManager.GetConnString());
                await connection.OpenAsync();
                SqlCommand checkcommand = new("select p.*,a.Image " +
                    "from TblUserProfile p,TblAvatarMaster a " +
                    "where (p.UserName = @username OR p.email = @username)" +
                    " and CryptedPassword =@password" +
                    " and p.IsActive= 1 " +
                    " and p.IsVerified = 1 " +
                    " and p.AvatarId = a.Id", connection);
                checkcommand.Parameters.AddWithValue("@username", loginCreds.UserName.ToLower());
                checkcommand.Parameters.AddWithValue("@password", EnDcryptor.Encrypt(loginCreds.Password,_configManager.GetCryptKey()));
                using var reader = await checkcommand.ExecuteReaderAsync();
                if (reader.Read())
                {
                    var username = reader.GetString(reader.GetOrdinal("UserName"));
                    var user_id = reader.GetInt32(reader.GetOrdinal("Id"));
                    var firstname = reader.GetString(reader.GetOrdinal("FirstName"));
                    var fullname = reader.GetString(reader.GetOrdinal("FirstName")) + " " + reader.GetString(reader.GetOrdinal("LastName"));
                    var role = reader.GetString(reader.GetOrdinal("Role"));
                    var avatar = reader.GetString(reader.GetOrdinal("Image"));
                    var sessionKeyOld = reader.GetString(reader.GetOrdinal("SessionKey"));
                    //set session
                    HttpContext.Session.SetString("user_id", user_id.ToString());
                    HttpContext.Session.SetString("username", username);
                    HttpContext.Session.SetString("first_name", firstname);
                    HttpContext.Session.SetString("role", role);
                    HttpContext.Session.SetString("fullname", fullname);
                    HttpContext.Session.SetString("avatar", avatar.ToString());
                    var sessionKeyNew = "";

                    if (sessionKeyOld == null)
                    {
                        string SessionKey = StringProcessors.GenerateRandomString(20);

                        await reader.CloseAsync();
                        SqlCommand setKey = new()
                        {
                            CommandText = "UPDATE TblUserProfile SET SessionKey = @sessionkey WHERE Id = @userid",
                            Connection = connection
                        };
                        setKey.Parameters.AddWithValue("@sessionkey", SessionKey);
                        setKey.Parameters.AddWithValue("@userid", user_id);
                        await setKey.ExecuteNonQueryAsync();
                        sessionKeyNew = SessionKey;


                    }
                    else
                    {
                        sessionKeyNew = sessionKeyOld;
                    }


                    Response.Cookies.Append("SessionKey", sessionKeyNew, new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(150)
                    });


                    _logger.LogInformation(loginCreds.UserName + " logged in");
                    // await TeleLog.Logstuff("*" + loginCreds.UserName + "* logged in") ;
                    return Ok("logging in...");

                }
                else
                {
                    _logger.LogInformation("invalid creds by username:" + loginCreds.UserName);
                    return BadRequest("Invalid Credentials");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString() + " : in login form,user : " + loginCreds.UserName);
                return StatusCode(500, "soomething went wrong");

            }
        }

        [Route("/account/logout")]
        public void LogOut()
        {
            Response.Cookies.Delete("SessionKey");
            HttpContext.Session.Clear();
            Response.Redirect("/");
        }
    }
}
