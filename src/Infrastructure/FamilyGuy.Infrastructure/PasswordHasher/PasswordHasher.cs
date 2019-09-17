using FamilyGuy.Processes.UserRegistration;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FamilyGuy.Infrastructure.PasswordHasher
{
    public class PasswordHasher : IPasswordHasher
    {
        public async Task<string> HashString(string password, string salt)
        {
            return await Task.Run(() =>
            {
                string saltedPassword = $"{password}{salt}";
                using SHA256 sha256 = SHA256.Create();
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            });
        }

        public async Task<bool> CheckHash(string password, string passwordHash, string salt)
        {
            return await HashString(password, salt) == passwordHash;
        }

        public async Task<string> GetSalt()
        {
            return await Task.Run(() =>
            {
                byte[] bytes = new byte[128 / 8];
                using RandomNumberGenerator keyGenerator = RandomNumberGenerator.Create();
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            });
        }
    }
}