using System.Security.Cryptography;
using System.Text;

namespace SecurityWebSite
{
    public class SecurityUtils
    {

        public static string GenerateSalt(int length = 10)
        {

            // Creating byte array for salt and filling it with random bytes
            byte[] saltBytes = new byte[length * sizeof(char)];
            RandomNumberGenerator.Fill(saltBytes);

            // Converting the salt bytes to a hex string
            string saltString = Convert.ToHexString(saltBytes);

            return saltString;

        }

        public static string HashPassword(string password, string salt)
        {

            string saltedPassword = password + salt;

            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(saltedPassword));
            string hashHexString = Convert.ToHexString(hashBytes);

            return hashHexString;

        }

    }
}
