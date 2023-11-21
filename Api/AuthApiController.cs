using almondCove.Models.Domain;
using almondCove.Modules;
using almondCove.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

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
        private readonly IMailer _mailer;
        public AuthApiController(IConfigManager configManager, ILogger<AuthApiController> logger,IMailer mailer)
        {
            _configManager = configManager;
            _logger = logger;
            _mailer = mailer;

        }

        [HttpPost("/api/account/login")]
        [IgnoreAntiforgeryToken]
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

        [HttpPost("/api/account/signup")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UserSignUp([FromBody] UserProfile userProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid state");
            }
            else
            {
            string body, subject;
                if (userProfile.UserName != null && userProfile.Password != null)
                {
                    if (userProfile.FirstName.Trim() == "")
                    {
                        return BadRequest("first name is mandatory");
                    }
                    else if (userProfile.UserName.Trim() == "")
                    {
                        return BadRequest("username is mandatory");
                    }
                    else if (userProfile.UserName.Trim().Contains(" "))
                    {
                        return BadRequest("spaces aren't allowed in a username");
                    }
                    else if (userProfile.EMail.Trim() == "")
                    {
                        return BadRequest("email is mandatory");
                    }
                    else if (Validators.IsValidEmail(userProfile.EMail.Trim()) == false)
                    {
                        return BadRequest("invalid email format");
                    }
                    else if (userProfile.Password.Trim() == "")
                    {
                        return BadRequest("password is mandatory");
                    }
                    else if (userProfile.Password.Trim().Length <= 6)
                    {
                        return BadRequest("password should be atlease 6 chars long");
                    }
                    else
                    {
                        try
                        {
                            string FilteredUsername = userProfile.UserName.Trim().ToLower().ToString();
                            using SqlConnection connection = new(_configManager.GetConnString());
                            await connection.OpenAsync();
                            SqlCommand cmd = new("select count(*) from TblUserProfile where UserName = @inputusername or EMail = @inputemail", connection);
                            cmd.Parameters.AddWithValue("@inputusername", FilteredUsername);
                            cmd.Parameters.AddWithValue("@inputemail", userProfile.EMail);
                            var counter = cmd.ExecuteScalar().ToString();
                            if (counter == "0")
                            {
                                string secret = StringProcessors.GenerateRandomString(10);
                                var otp = OTPGenerator.GenerateOTP(secret);
                                subject = "Verify Your Account | AlmondCove";
                                try
                                {

                                    body = "<!DOCTYPE html><html><head><meta name=\"viewport\" content=\"width=device-width,initial-scale=1\"><title>Verification</title></head><body style=\"margin:0;padding:0;font-family:Arial,sans-serif;line-height:1.4;color:#111;background-color:#fff\"><div style=\"max-width:600px;margin:0 auto;background-color:#fff;padding:20px;border-radius:5px\"><h1 style=\"color:#111;margin-bottom:20px;font-size:24px\">Complete Signup</h1><p>Hey there,</p><div style=\"text-align:center;margin-bottom:20px\"><img src=\"https://almondCove.in/assets/favicon/apple-touch-icon.png\" width=\"100\" alt=\"Image\" style=\"max-width:100%;height:auto;border-radius:5px\"></div><p>Welcome to the AlmondCove.Your OTP is <h2><b>" + otp + " </b></h2> .You can verify your account from the following button too.</p><p>" +
                                        "<a href=\"https://almondCove.in/account/verification/" + FilteredUsername + "/" + otp + "\"" +
                                        " style=\"display:inline-block;padding:10px 20px;background-color:#111;color:#fff;text-decoration:none;border-radius:4px\">Verify Email</a></p><p>If you did not sign up for this account, please ignore this email.</p><div style=\"margin-top:20px;text-align:center;font-size:12px;color:#999\"><p>This is an automated email, please do not reply.</p></div></div></body></html>";

                                    //body = "<h1>Hey there,</h1>" +
                                    //        "<p> This is for the verification of your account @almondCove." +
                                    //        "" + otp + " is your OTP which is valid for 30 minutes </p>." +
                                    //        "Or alternatively you can click here to verify directly:" +
                                    //        "<button type=\"button\" href=\"https://almondCove.in/account/verification/" + FilteredUsername + "/" + otp + "\"><b> VERIFY </b></button>";

                                    //int stat = Mailer.MailSignup(subject, body, userProfile.EMail.ToString());
                                  bool stat =  _mailer.SendEmailAsync(userProfile.EMail.ToString(), subject, body);


                                    if (stat)
                                    {
                                        try
                                        {
                                            SqlCommand maxIdCommand = new("SELECT ISNULL(MAX(Id), 0) + 1 FROM TblUserProfile", connection);
                                            int newId = Convert.ToInt32(maxIdCommand.ExecuteScalar());
                                            cmd = new("insert into TblUserProfile (Id,FirstName,LastName,EMail,UserName,IsActive,IsVerified,OTP,OTPTime,Role,Bio,Gender,Phone,AvatarId,DateJoined,CryptedPassword) VALUES(@Id,@firstname,@lastname,@email,@username,1,0,@otp,@otptime,'user','','','',1,@datejoined,@cryptedpassword)", connection);
                                            cmd.Parameters.AddWithValue("@Id", newId);
                                            cmd.Parameters.AddWithValue("@firstname", userProfile.FirstName.Trim());
                                            cmd.Parameters.AddWithValue("@lastname", userProfile.LastName);
                                            cmd.Parameters.AddWithValue("@email", userProfile.EMail);
                                            cmd.Parameters.AddWithValue("@username", FilteredUsername.Trim());
                                            cmd.Parameters.AddWithValue("@cryptedpassword", EnDcryptor.Encrypt(userProfile.Password.Trim(),_configManager.GetCryptKey()));
                                            cmd.Parameters.AddWithValue("@otp", otp.Trim());
                                            cmd.Parameters.Add("@otptime", SqlDbType.DateTime).Value = DateTime.Now;
                                            cmd.Parameters.Add("@datejoined", SqlDbType.DateTime).Value = DateTime.Now;

                                            await cmd.ExecuteNonQueryAsync();
                                            _logger.LogInformation(userProfile.FirstName + " registered, Email: " + userProfile.EMail);
                                            return Ok("verification email send please verify your account");
                    
                                           
                                        }
                                        catch (Exception exm)
                                        {
                                            _logger.LogError("Error while user registration: " + exm.Message.ToString());
                                            return BadRequest("something went wrong");
                                           
                                        }

                                    }
                                    else
                                    {
                                            return BadRequest("unable to send the mail");
                                    }
                                }
                                catch (Exception ex2)
                                {
                                        _logger.LogError(ex2.Message.ToString());
                                        return BadRequest("something went wrong");
                                
                                }
                            }
                            else
                            {
                                    return BadRequest("username/email taken!!");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("error while signup" + ex.Message.ToString());
                            return BadRequest("something went wrong");
                         
                        }
                    }
                }
                else
                {
                    return BadRequest("invalid data");
                }
            
            }
        }
    }
}
