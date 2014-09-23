using System;
using System.Security.Cryptography;
using System.Text;

namespace Top4ever.Utils
{
    public class PasswordCryptographer
    {
        // Fields
        private const char Delim = '*';
        private const int SaltLength = 6;

        // Methods
        public static bool AreEqual(string saltedPassword, string password)
        {
            if (string.IsNullOrEmpty(saltedPassword))
            {
                return string.IsNullOrEmpty(password);
            }
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
            int index = saltedPassword.IndexOf(Delim);
            if (index <= 0)
            {
                return saltedPassword.Equals(password);
            }
            string str = SaltPassword(password, saltedPassword.Substring(0, index));
            string str2 = saltedPassword.Substring(index + 1);
            return (str2.Equals(str) || str2.Equals(SaltPassword(password, "System.Byte[]")));
        }

        public static string GenerateSaltedPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return password;
            }
            byte[] data = new byte[SaltLength];
            new RNGCryptoServiceProvider().GetBytes(data);
            string salt = Convert.ToBase64String(data);
            return (salt + Delim + SaltPassword(password, salt));
        }

        public static string SaltPassword(string password, string salt)
        {
            return Convert.ToBase64String(SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(salt + password)));
        }
    }
}