using System.Text.RegularExpressions;

namespace almondcove.Modules
{
    public partial class StringProcessors
    {
        public static string GetFirstWord(string ipname)
        {
            return "something";
        }

        public static string RemoveSpecialChars(string ipname)
        {
            return "something";
        }

        public static string GetLastWord(string ipname)
        {
            return "something";
        }

        public static string Sanitize(string input)
        {
            string sanitizedText = input.Trim();

            // Remove quotes (single and double)
            sanitizedText = MyRegex().Replace(sanitizedText, "");

            // Remove HTML tags
            sanitizedText = MyRegex1().Replace(sanitizedText, "");

            return sanitizedText;
        }
        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }
            return new string(result);

        }
        public static string GenerateRandomStringtemp(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }
            return new string(result);
        }

        [GeneratedRegex("[\"']")]
        private static partial Regex MyRegex();
        [GeneratedRegex("<[^>]*>")]
        private static partial Regex MyRegex1();
    }
}
