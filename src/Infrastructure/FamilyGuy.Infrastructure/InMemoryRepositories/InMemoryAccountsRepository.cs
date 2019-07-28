using FamilyGuy.Accounts;
using FamilyGuy.Accounts.AccountQuery;
using FamilyGuy.Accounts.AccountQuery.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User = FamilyGuy.Accounts.Domain.User;

namespace FamilyGuy.Infrastructure.InMemoryRepositories
{

    public class InMemoryAccountsRepository : IAccountsPerspective, IAccountsRepository, IInMemoryRepository
    {
        public static List<User> Users = new List<User>();
        public async Task Add(User user)
        {
            await Task.Run(() => Users.Add(user));
        }

        public async Task<AccountReadModel> Get(Guid userId)
        {
            User user = await Task.FromResult(Users.FirstOrDefault(x => x.Id == userId));
            return new AccountReadModel
            {
                Id = user.Id,
                LoginName = user.UserName,
                Name = user.Name,
                Surname = user.Surname
            };
        }
    }
}
