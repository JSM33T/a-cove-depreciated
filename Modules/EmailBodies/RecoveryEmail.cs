using System.Text;

namespace almondcove.Modules.EmailBodies
{
    public static class RecoveryEmail
    {
        public static string GenerateRecoveryEmailBody(string userEmail, string otp)
        {
            StringBuilder bodyBuilder = new();
            bodyBuilder.Append("<html>")
                    .Append("<body style=\"font-family: 'Arial', sans-serif;\">")
                    .Append("<div style=\"background-color: #f4f4f4; padding: 20px;border-radius:10px\">")
                    .Append("<h1 style=\"color: #333;\">Hello there!</h1>")
                    .Append("<p style=\"color: #555; font-size: 16px;\">We noticed you're having a bit of trouble accessing your account.</p>")
                    .Append("<p style=\"color: #555; font-size: 16px;\">No worries, we've got you covered!</p>")
                    .Append("<p style=\"color: #555; font-size: 16px;\">To recover your account '<strong>")
                    .Append(userEmail)
                    .Append("</strong>', use the following One-Time Password (OTP): <strong>")
                    .Append(otp)
                    .Append("</strong>.</p>")
                    .Append("<p style=\"color: #555; font-size: 16px;\">This OTP is valid for the next 30 minutes, so don't wait too long!</p>")
                    .Append("<p style=\"color: #555; font-size: 16px;\">If you didn't request this recovery, please contact our support immediately.</p>")
                    .Append("<p style=\"color: #555; font-size: 16px;\">Stay secure!</p>")
                    .Append("<div style=\"margin-top: 20px; padding: 15px;border-radius:10px;background-color: #121519; color: #fff; text-align: center;\">")
                    .Append("<p style=\"margin: 0;\">-Jass</p>")
                    .Append("<p style=\"margin: 0;\">founder,AlmondCove</p>")
                    .Append("</div>")
                    .Append("</div>")
                    .Append("</body>")
                    .Append("</html>");

            return bodyBuilder.ToString();
        }

        
    }
}
