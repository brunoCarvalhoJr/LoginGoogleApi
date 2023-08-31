using System.Security.Cryptography;
using System.Text;

namespace LoginGoogleApi.Services
{
    public class HasService
    {
        public static string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            string hashedPassorwd = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hashedPassorwd;
        }
    }
}
