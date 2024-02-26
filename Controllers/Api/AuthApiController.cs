using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using almondcove.Models.Domain;
using almondcove.Models.DTO.Account;
using almondcove.Modules;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;

namespace almondcove.Controllers.Apix
{
    [ApiController]
    public class AuthApiController(IConfigManager configManager, ILogger<AuthApiController> logger, IMailer mailer, IAuthRepository authRepository) : ControllerBase
    {
        private readonly IAuthRepository _authRepo = authRepository;
        private readonly IConfigManager _configManager = configManager;
        private readonly ILogger<AuthApiController> _logger = logger;
        private readonly IMailer _mailer = mailer;

        [HttpPost("/api/account/login")]
        public async Task<IActionResult> UserLogin(LoginCreds loginCreds)
        {
            if (!ModelState.IsValid) return BadRequest("validation error");
            if (await LoginUser(loginCreds)) return Ok();
            else return BadRequest("Invalid credentials");
        }

        private async Task<bool> LoginUser(LoginCreds loginCreds)
        {
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

                using var reader = await checkcommand.ExecuteReaderAsync();
                if (reader.Read())
                {
                    var username = reader.GetString(reader.GetOrdinal("UserName"));
                    var user_id = reader.GetInt32(reader.GetOrdinal("Id"));
                    var firstname = reader.GetString(reader.GetOrdinal("FirstName"));
                    var fullname = reader.GetString(reader.GetOrdinal("FirstName")) + " " + reader.GetString(reader.GetOrdinal("LastName"));
                    var role = reader.GetString(reader.GetOrdinal("Role"));
                    var avatar = reader.GetString(reader.GetOrdinal("Image"));
                    var email = reader.GetString(reader.GetOrdinal("EMail"));

                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.NameIdentifier, user_id.ToString()),
                        new(ClaimTypes.Name, username),
                        new(ClaimTypes.GivenName, firstname),
                        new(ClaimTypes.Role, role),
                        new(ClaimTypes.Email,email),
                        new("fullname", fullname),
                        new("avatar", avatar.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "Cookie");

                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(
                           CookieAuthenticationDefaults.AuthenticationScheme,
                           new ClaimsPrincipal(claimsIdentity),
                           new AuthenticationProperties
                           {
                               IsPersistent = true,
                               ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(300))
                           }
                    );
                    _logger.LogError("invalid creds by username {username}", User.FindFirst(ClaimTypes.Name)?.Value);
                    return true;

                }
                else
                {
                    _logger.LogError("invalid creds by username {username}", loginCreds.UserName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in login form, message : {exmessage}, user : {user} ", ex.Message.ToString(), loginCreds.UserName);

                return false;
            }
        }

        [HttpPost("/api/account/signup")]
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
                        body = Modules.EmailBodies.SignUpEmail.SignUpEmailBody(FilteredUsername, otp);
                        bool stat = _mailer.SendEmailAsync(userProfile.EMail.ToString(), subject, body);

                        if (!stat) return BadRequest("unable to send the mail");
                        
                        SqlCommand maxIdCommand = new("SELECT ISNULL(MAX(Id), 0) + 1 FROM TblUserProfile", connection);
                        int newId = Convert.ToInt32(maxIdCommand.ExecuteScalar());
                        cmd = new(@"
                                INSERT INTO TblUserProfile 
                                (Id,FirstName,LastName,EMail,UserName,IsActive,IsVerified,OTP,OTPTime,Role,Bio,Gender,Phone,AvatarId,DateJoined,CryptedPassword) 
                                VALUES
                                (@Id,@firstname,@lastname,@email,@username,1,0,@otp,@otptime,'user','','','',1,@datejoined,@cryptedpassword)
                            ", connection);
                        cmd.Parameters.AddWithValue("@Id", newId);
                        cmd.Parameters.AddWithValue("@firstname", userProfile.FirstName.Trim());
                        cmd.Parameters.AddWithValue("@lastname", userProfile.LastName);
                        cmd.Parameters.AddWithValue("@email", userProfile.EMail);
                        cmd.Parameters.AddWithValue("@username", FilteredUsername.Trim());
                        cmd.Parameters.AddWithValue("" +
                            "@cryptedpassword",
                            EnDcryptor.Encrypt(userProfile.Password.Trim(), _configManager.GetCryptKey())
                            );
                        cmd.Parameters.AddWithValue("@otp", otp.Trim());
                        cmd.Parameters.Add("@otptime", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@datejoined", SqlDbType.DateTime).Value = DateTime.Now;

                        await cmd.ExecuteNonQueryAsync();
                        _logger.LogInformation("{user} registered,with Email:{email} ", userProfile.FirstName, userProfile.EMail);
                        return Ok("verification email send please verify your account");
                    }
                    catch (Exception ex2)
                    {
                        _logger.LogError("error creating user msg: {message}",ex2.Message.ToString());
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
                return StatusCode(500,"Something went wrong");
            }
        }

        [HttpPost("/api/user/verification")]
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
        public async Task<IActionResult> RecoverAccount(RecoveryDTO recovery)
        {

            if (!ModelState.IsValid) return BadRequest("Invalid Username/Email!!");

            try
            {
                using SqlConnection connection = new(_configManager.GetConnString());
                await connection.OpenAsync();

                var user = await _authRepo.GetUserByUsernameOrEmailAsync(connection, recovery.UserName);


                if (user == null)
                {
                    await connection.CloseAsync();
                    return BadRequest("No record found with the given username/email.");
                }
               
                var userId = user.Id;
                var username = user.UserName;
                var userEmail = user.EMail;
                var secret = StringProcessors.GenerateRandomString(10);
                var otp = OTPGenerator.GenerateOTP(secret);
                var subject = "Recover Your Account | AlmondCove";

                string body = Modules.EmailBodies.RecoveryEmail.GenerateRecoveryEmailBody(userEmail, otp);

                bool otpSent = _mailer.SendEmailAsync(userEmail, subject, body);

                if (otpSent && await _authRepo.SaveOTPInDatabaseAsync(connection, userId, otp))
                {
                    return Ok("OTP sent to your email. Please enter the OTP to login.");
                }
                _logger.LogError(otpSent ? "Error saving OTP in database." : "Error sending OTP email.");
                return BadRequest(otpSent ? "Something went wrong." : "Unable to send mail.");

            }
            catch (Exception ex)
            {
                _logger.LogError("Error recovering account for  {user} {message} ", recovery.UserName, ex.Message);
                return BadRequest("Something went wrong.");
            }
        }

        [HttpPost("/api/account/loginviaotp")]
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
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.NameIdentifier, user_id.ToString()),
                        new(ClaimTypes.Name, username),
                        new(ClaimTypes.GivenName, firstname),
                        new(ClaimTypes.Role, role),
                        new("fullname", fullname),
                        new("avatar", avatar.ToString())
                    };

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
