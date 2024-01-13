using System.Security.Cryptography;
using System.Text;

namespace almondcove.Modules
{
    public static class EnDcryptor
    {

        public static string Encrypt(string plaintext,string encryptionKey)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
            byte[] iv = new byte[16];
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

            using Aes aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] ciphertextBytes;
            using MemoryStream msEncrypt = new();
            using ICryptoTransform encryptor = aes.CreateEncryptor();
            using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
            csEncrypt.Write(plaintextBytes, 0, plaintextBytes.Length);
            csEncrypt.FlushFinalBlock();
            ciphertextBytes = msEncrypt.ToArray();

            string ciphertext = Convert.ToBase64String(ciphertextBytes);
            return ciphertext;
        }
        public static string Decrypt(string ciphertext, string encryptionKey)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
            byte[] iv = new byte[16]; // Use a random IV for increased security
            byte[] ciphertextBytes = Convert.FromBase64String(ciphertext);

            using Aes aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] plaintextBytes;
            using MemoryStream msDecrypt = new(ciphertextBytes);
            using ICryptoTransform decryptor = aes.CreateDecryptor();
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            plaintextBytes = new byte[ciphertextBytes.Length];
            int decryptedByteCount = csDecrypt.Read(plaintextBytes, 0, plaintextBytes.Length);

            string plaintext = Encoding.UTF8.GetString(plaintextBytes, 0, decryptedByteCount);
            return plaintext;
        }
        public static string ToBase64UrlString(string text)
        {
            return text.Replace('+', '-').Replace('/', '_');
        }

        public static string FromBase64UrlString(string text)
        {
            return text.Replace('-', '+').Replace('_', '/');
        }
    }
}
