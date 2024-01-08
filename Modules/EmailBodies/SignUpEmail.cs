using System.Text;

namespace almondcove.Modules.EmailBodies
{
    public static class SignUpEmail
    {
        public static string SignUpEmailBody(string userName, string otp)
        {
            StringBuilder bodyBuilder = new();
            bodyBuilder.Append("<html>")
                       .Append("<body style=\"font-family: 'Arial', sans-serif;\">")
                       .Append("<div style=\"background-color: #f4f4f4; padding: 20px;border-radius:10px\">")
                       .Append("<h1 style=\"color: #333;\">Welcome to AlmondCove!</h1>")
                       .Append("<p style=\"color: #555; font-size: 16px;\">Dear ")
                       .Append(userName)
                       .Append(",</p>")
                       .Append("<p style=\"color: #555; font-size: 16px;\">Thank you for joining our community. <strong>")
                       .Append(otp)
                       .Append("</strong> is your otp for confirming your email account")
                       .Append("</p><p style=\"color: #555; font-size: 16px;\">Here are a few things you can do to get started:</p>")
                       .Append("<ul style=\"color: #555; font-size: 16px;\">")
                       .Append("   <li>Explore our website and discover exciting resources.</li>")
                       .Append("   <li>Customize your profile settings to make the most of your experience.</li>")
                       .Append("   <li>Connect with other users and start engaging with our community.</li>")
                       .Append("</ul>")
                       .Append("<p style=\"color: #555; font-size: 16px;\">If you have any questions or need assistance, feel free to reach out to our support team.</p>")
                       .Append("<div style=\"margin-top: 20px; padding: 15px; background-color: #121519; border-radius:10px;color: #fff; text-align: center;\">")
                       .Append("<p style=\"margin: 0;\">Karan</p>")
                       .Append("Founder, almondcove.in")
                       .Append("</div>")
                       .Append("</div>")
                       .Append("</body>")
                       .Append("</html>");

            return bodyBuilder.ToString();
        }

    }
}