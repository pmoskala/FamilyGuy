using System.Security.Cryptography;
using System.Text;

namespace FamilyGuy.Processes.UserRegistration
{
    internal class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            return Encoding.ASCII.GetString(md5.ComputeHash(Encoding.ASCII.GetBytes(password)));
        }
    }
}
