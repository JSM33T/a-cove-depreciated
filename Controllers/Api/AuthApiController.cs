using almondcove.Filters;
using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using almondcove.Models.Domain;
using almondcove.Models.DTO.Account;
using almondcove.Modules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace almondcove.Controllers.Api
{
    [ApiController]
    public class AuthApiController(IConfigManager configManager, ILogger<AuthApiController> logger, IMailer mailer, IAuthRepository authRepository) : ControllerBase
    {
        private readonly IAuthRepository _authRepo = authRepository;
        private readonly IConfigManager _configManager = configManager;
        private readonly ILogger<AuthApiController> _logger = logger;
        private readonly IMailer _mailer = mailer;

        [Perm("guest")]
        [HttpPost("/api/account/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserLogin(LoginCreds loginCreds)
        {
            if (!ModelState.IsValid) return BadRequest("validation error");

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
                checkcommand.Parameters.AddWithValue("@password", EnDcryptor.Encrypt(loginCreds.Password, _configManager.GetCryptKey()));
                // TEST
                //_logger.LogCritical( "{cryptkey}" ,EnDcryptor.Encrypt(loginCreds.Password, _configManager.GetCryptKey()));
                using var reader = await checkcommand.ExecuteReaderAsync();
                if (reader.Read())
                {
                    var username = reader.GetString(reader.GetOrdinal("UserName"));
                    var user_id = reader.GetInt32(reader.GetOrdinal("Id"));
                    var firstname = reader.GetString(reader.GetOrdinal("FirstName"));
                    var fullname = reader.GetString(reader.GetOrdinal("FirstName")) + " " + reader.GetString(reader.GetOrdinal("LastName"));
                    var role = reader.GetString(reader.GetOrdinal("Role"));
                    var avatar = reader.GetString(reader.GetOrdinal("Image"));
                    //NULL ISSUE - BUG IN V1
                    // var sessionKeyOld = reader.GetString(reader.GetOrdinal("SessionKey"));
                    var sessionKeyOrdinal = reader.GetOrdinal("SessionKey");
                    // Check if the value is DBNull before trying to retrieve it
                    var sessionKeyOld = reader.IsDBNull(sessionKeyOrdinal) ? null : reader.GetString(reader.GetOrdinal("SessionKey"));
                    // Now you can use sessionKeyOld without the risk of a NullReferenceException

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

                    _logger.LogInformation("{user} logged in on {time}", loginCreds.UserName, DateTime.Now);
                    return Ok("logging in...");

                }
                else
                {
                    _logger.LogError("invalid creds by username {username}", loginCreds.UserName);
                    return BadRequest("Invalid Credentials");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in login form, message : {exmessage}, user : {user} ", ex.Message.ToString(), loginCreds.UserName);

                return StatusCode(500, "something went wrong");
            }
        }

        [Perm("guest")]
        [HttpPost("/api/account/signup")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserSignUp(UserProfile userProfile)
        {
            if (!ModelState.IsValid) { return BadRequest("invalid state"); }
            string body, subject;

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

                        // body = "<!DOCTYPE html><html><head><meta name=\"viewport\" content=\"width=device-width,initial-scale=1\"><title>Verification</title></head><body style=\"margin:0;padding:0;font-family:Arial,sans-serif;line-height:1.4;color:#111;background-color:#fff\"><div style=\"max-width:600px;margin:0 auto;background-color:#fff;padding:20px;border-radius:5px\"><h1 style=\"color:#111;margin-bottom:20px;font-size:24px\">Complete Signup</h1><p>Hey there,</p><div style=\"text-align:center;margin-bottom:20px\"><img src=\"https://almondcove.in/assets/favicon/apple-touch-icon.png\" width=\"100\" alt=\"Image\" style=\"max-width:100%;height:auto;border-radius:5px\"></div><p>Welcome to the AlmondCove.Your OTP is <h2><b>" + otp + " </b></h2> .You can verify your account from the following button too.</p><p>" +
                        //         "<a href=\"https://almondcove.in/account/verification/" + FilteredUsername + "/" + otp + "\"" +
                        //         " style=\"display:inline-block;padding:10px 20px;background-color:#111;color:#fff;text-decoration:none;border-radius:4px\">Verify Email</a></p><p>If you did not sign up for this account, please ignore this email.</p><div style=\"margin-top:20px;text-align:center;font-size:12px;color:#999\"><p>This is an automated email, please do not reply.</p></div></div></body></html>";
                        body = Modules.EmailBodies.SignUpEmail.SignUpEmailBody(FilteredUsername, otp);
                        bool stat = _mailer.SendEmailAsync(userProfile.EMail.ToString(), subject, body);
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
                                cmd.Parameters.AddWithValue("@cryptedpassword", EnDcryptor.Encrypt(userProfile.Password.Trim(), _configManager.GetCryptKey()));
                                cmd.Parameters.AddWithValue("@otp", otp.Trim());
                                cmd.Parameters.Add("@otptime", SqlDbType.DateTime).Value = DateTime.Now;
                                cmd.Parameters.Add("@datejoined", SqlDbType.DateTime).Value = DateTime.Now;

                                await cmd.ExecuteNonQueryAsync();
                                _logger.LogInformation("{user} registered,with Email:{email} ", userProfile.FirstName, userProfile.EMail);
                                return Ok("verification email send please verify your account");


                            }
                            catch (Exception exm)
                            {
                                _logger.LogError("Exception while user {username} 's registration on {datetime},ex message: {exmessage}", userProfile.UserName, DateTime.Now, exm.Message.ToString());
                                return BadRequest("something went wrong");

                            }

                        }
                        else
                        {
                            _logger.LogError("unable to send mail to {user} on datetime: {datetime}", userProfile.UserName, DateTime.Now);
                            return BadRequest("unable to send the mail");
                        }
                    }
                    catch (Exception ex2)
                    {
                        _logger.LogError(ex2.Message.ToString());
                        return BadRequest("something went wrong");

                    }
                }
                else if (counter == null)
                {
                    return BadRequest("something went wrong");
                }
                else
                {
                    return Conflict("username/email taken!!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("error while signup exception: {exmessage}", ex.Message.ToString());
                return BadRequest("something went wrong");
            }
        }

        [HttpPost]
        [Route("/api/user/verification")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UserVerification(Verify verify)
        {
            try
            {
                using SqlConnection connection = new(_configManager.GetConnString());
                await connection.OpenAsync();

                SqlCommand checkCmd = new("SELECT IsVerified FROM TblUserProfile WHERE Username = @Username AND OTP = @OTP", connection);
                checkCmd.Parameters.AddWithValue("@Username", verify.UserName);
                checkCmd.Parameters.AddWithValue("@OTP", verify.OTP);

                var isVerified = await checkCmd.ExecuteScalarAsync() as bool?;

                if (isVerified.HasValue)
                {
                    if (!isVerified.Value)
                    {
                        SqlCommand activateCmd = new("UPDATE TblUserProfile SET IsVerified = 1 WHERE UserName = @Username", connection);
                        activateCmd.Parameters.AddWithValue("@Username", verify.UserName);
                        await activateCmd.ExecuteNonQueryAsync();
                        return Ok("User Verified!!");
                    }
                    else
                    {
                        return BadRequest("User already verified");
                    }
                }
                else
                {
                    return BadRequest("Invalid Username or OTP");
                }
            }
            catch (Exception ex)
            {

                _logger.LogError("Error verifying a user err message:{message}", ex.Message);
                return BadRequest("Error verifying user");
            }
        }

        [HttpPost("/api/account/recover")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecoverAccount(RecoveryDTO recovery)
        {

            if (!ModelState.IsValid) return BadRequest("Invalid Username/Email!!");

            try
            {
                using SqlConnection connection = new(_configManager.GetConnString());
                await connection.OpenAsync();

                var user = await _authRepo.GetUserByUsernameOrEmail(connection, recovery.UserName);

                if (user != null)
                {
                    var userId = user.Id;
                    var username = user.UserName;
                    var userEmail = user.EMail;
                    var secret = StringProcessors.GenerateRandomString(10);
                    var otp = OTPGenerator.GenerateOTP(secret);
                    var subject = "Recover Your Account | AlmondCove";

                    string body = Modules.EmailBodies.RecoveryEmail.GenerateRecoveryEmailBody(userEmail, otp);

                    bool otpSent = _mailer.SendEmailAsync(userEmail, subject, body);

                    if (otpSent == true)
                    {
                        if (_authRepo.SaveOTPInDatabaseAsync(connection, userId, otp))
                        {
                            return Ok("OTP sent to your email. Please enter the OTP to login.");
                        }
                        else
                        {
                            _logger.LogError("error generating otp");
                            return BadRequest("Something went wrong.");
                        }
                    }
                    else
                    {
                        _logger.LogError("error generating otp msg");
                        return BadRequest("Unable to send mail.");
                    }
                }
                else
                {
                    await connection.CloseAsync();
                    return BadRequest("No record found with the given username/email.");
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError("Error recovering account for  {user} {message} ", recovery.UserName, ex.Message);
                return BadRequest("Something went wrong.");
            }
        }

        [Perm("user", "admin", "editor")]
        [HttpPost("/api/account/clearallsessions")]
        public async Task<IActionResult> DisposeSessionKey()
        {
            if (await _authRepo.DisposeSessionKey(HttpContext.Session.GetString("username"))) return Ok();
            else return BadRequest("unable to dispose session key");
        }

        [HttpPost("/api/account/loginviaotp")]
        [IgnoreAntiforgeryToken]
        public IActionResult LogInViaOtp([FromBody] Verify verify)
        {
            try
            {
                using SqlConnection connection = new(_configManager.GetConnString());
                connection.Open();

                SqlCommand checkOTP = new("SELECT * FROM TblPasswordReset WHERE Token = @otp", connection);
                checkOTP.Parameters.AddWithValue("@otp", verify.OTP);

                using SqlDataReader readera = checkOTP.ExecuteReader();
                string currUserId = "";

                if (readera.Read()) currUserId = readera.GetInt32(readera.GetOrdinal("UserId")).ToString();
                else currUserId = "";

                readera.Close();
                SqlCommand checkcommand = new("select p.*,a.Image " +
                   "from TblUserProfile p,TblAvatarMaster a " +
                   "where p.Id = @id " +
                   "and p.IsActive= 1 " +
                   "and p.IsVerified = 1 " +
                   "and p.AvatarId = a.Id", connection);
                checkcommand.Parameters.AddWithValue("@id", currUserId);
                using var reader = checkcommand.ExecuteReader();
                if (reader.Read())
                {
                    var username = reader.GetString(reader.GetOrdinal("UserName"));
                    var user_id = reader.GetInt32(reader.GetOrdinal("Id"));
                    var firstname = reader.GetString(reader.GetOrdinal("FirstName"));
                    var fullname = reader.GetString(reader.GetOrdinal("FirstName")) + " " + reader.GetString(reader.GetOrdinal("LastName"));
                    var role = reader.GetString(reader.GetOrdinal("Role"));
                    var avatar = reader.GetString(reader.GetOrdinal("Image"));
                    //set session
                    HttpContext.Session.SetString("user_id", user_id.ToString());
                    HttpContext.Session.SetString("username", username);
                    HttpContext.Session.SetString("first_name", firstname);
                    HttpContext.Session.SetString("role", role);
                    HttpContext.Session.SetString("fullname", fullname);
                    HttpContext.Session.SetString("avatar", avatar.ToString());
                    connection.Close();
                    return Ok("logging in...");

                }
                else
                {
                    connection.Close();
                    return BadRequest("Invalid/Expired Otp");
                }
            }
            catch (Exception ex2)
            {
                _logger.LogError("login via OTP failed msg: {errmsg}", ex2.Message.ToString());
                return BadRequest("Something went wrong");
            }
        }
    }
}
