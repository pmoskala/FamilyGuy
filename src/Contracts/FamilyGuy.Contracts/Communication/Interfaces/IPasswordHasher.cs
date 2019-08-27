using System.Threading.Tasks;

namespace FamilyGuy.Processes.UserRegistration
{
    public interface IPasswordHasher
    {
        Task<string> HashString(string password, string salt);
        Task<bool> CheckHash(string password, string passwordHash, string salt);
        Task<string> GetSalt();
    }
}