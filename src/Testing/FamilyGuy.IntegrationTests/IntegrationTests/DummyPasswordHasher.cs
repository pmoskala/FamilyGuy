using FamilyGuy.Processes.UserRegistration;
using System.Threading.Tasks;

namespace FamilyGuy.IntegrationTests.IntegrationTests
{
    public class DummyPasswordHasher : IPasswordHasher
    {
        public async Task<string> HashString(string password, string salt)
        {
            return await Task.Run(() => password);
        }

        public async Task<bool> CheckHash(string password, string passwordHash, string salt)
        {
            return await HashString(password, salt) == passwordHash;
        }

        public async Task<string> GetSalt()
        {
            return await Task.Run(() => string.Empty);
        }
    }
}