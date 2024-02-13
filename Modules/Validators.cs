using System.Net.Mail;
using System.Text.RegularExpressions;

namespace almondcove.Modules
{
    public partial class Validators
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IsAlphaNumeric(string input)
        {
            Regex regex = IsAlphaNum();
            return regex.IsMatch(input);
        }

        [GeneratedRegex("^[a-zA-Z0-9]+$")]
        private static partial Regex IsAlphaNum();
    }
}
