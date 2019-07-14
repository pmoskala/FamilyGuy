using FamilyGuy.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User = FamilyGuy.Accounts.Domain.User;

namespace FamilyGuy.Infrastructure.InMemoryRepositories
{

    public class InMemoryUserRepository : IUserRepository, IInMemoryRepository
    {
        public static List<User> Users = new List<User>();
        public async Task Add(User user)
        {
            await Task.Run(() => Users.Add(user));
        }

        public async Task<User> Get(Guid id)
        {
            return await Task.FromResult(Users.FirstOrDefault(x => x.Id == id));
        }
    }
}
