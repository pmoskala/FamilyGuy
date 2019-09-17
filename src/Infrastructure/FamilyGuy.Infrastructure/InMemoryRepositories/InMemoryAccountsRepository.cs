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
    public class InMemoryAccountsRepository : IFlushableRepository, IAccountsPerspective, IAccountsRepository,
        IInMemoryRepository
    {
        public static List<User> Users = new List<User>();

        public async Task Add(User user) 
            => await Task.Run(() => Users.Add(user));

        public async Task UpdatePassword(Guid userId, string passwordHash, string salt)
        {
            User user = await Task.FromResult(Users.FirstOrDefault(x => x.Id == userId));
            user.SetPassword(passwordHash, salt);
        }

        public async Task<AccountReadModel> Get(Guid userId)
        {
            User user = await Task.FromResult(Users.FirstOrDefault(x => x.Id == userId));
            return user == null ? null : AccountReadModel(user);
        }

        public async Task<AccountReadModel> Get(string userName)
        {
            User user = await Task.FromResult(Users.FirstOrDefault(x => x.UserName == userName));
            return user == null ? null : AccountReadModel(user);
        }

        public async Task<AccountWithCredentialsModel> GetUserWithCredentials(string userName)
        {
            User user = await Task.FromResult(Users.FirstOrDefault(x => x.UserName == userName));
            return new AccountWithCredentialsModel
            {
                Id = user.Id,
                PasswordSalt = user.PasswordSalt,
                Name = user.Name,
                Surname = user.Surname,
                PasswordHash = user.PasswordHash
            };
        }

        private static AccountReadModel AccountReadModel(User user)
        {
            return new AccountReadModel
            {
                Id = user.Id,
                LoginName = user.UserName,
                Name = user.Name,
                Surname = user.Surname
            };
        }

        public void Clear() => Users.Clear();
    }

    public interface IFlushableRepository
    {
        void Clear();
    }
}