using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SampleProject.Models.Utils
{
    public class PasswordUtils
    {

        public static string HashPassword(string password, out string salt)
        {
            salt = CreateSalt(16);  
            var saltedPassword = salt + password;

            using (var sha256 = SHA256.Create())
            {
                var hashPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(hashPassword);
            }


        }
        private static string CreateSalt(int size)
        {
            byte[] saltBytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }

        public static bool VerifyPassword(string salt, string hashPassword, string password)
        {
            var saltedPassword = salt + password;

            using (var sha256 = SHA256.Create())
            {

                var hashResult = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                var inputHash = Convert.ToBase64String(hashResult);

                if (inputHash == hashPassword)
                    return true;

            }
            return false; 


        }

    }
}