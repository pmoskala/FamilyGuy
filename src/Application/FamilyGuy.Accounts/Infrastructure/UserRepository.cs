using FamilyGuy.Accounts.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FamilyGuy.Accounts.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        public static List<User> Users { get; } = new List<User>();

        public async Task Add(User user)
        {
            Users.Add(user);
            await Task.Delay(100);
        }
    }
}
