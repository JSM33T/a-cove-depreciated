using System.Text;

namespace almondcove.Modules.EmailBodies
{
    public static class RecoveryEmail
    {
        public static string GenerateRecoveryEmailBody(string userEmail, string otp)
        {
            StringBuilder bodyBuilder = new StringBuilder();
            bodyBuilder.Append("<h1>Hey there,</h1><br>")
                       .Append($" This is for the recovery of your account \"<b>{userEmail}</b>\" .")
                       .Append($" Your OTP is: <b>{otp}</b> which is valid for 30 minutes.")
                       .Append(" You can use this OTP to reset your password.");

            return bodyBuilder.ToString();
        }
    }
}
