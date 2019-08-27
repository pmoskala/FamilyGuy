using FamilyGuy.Accounts;
using FamilyGuy.Accounts.AccountQuery;
using FamilyGuy.Accounts.AccountQuery.Model;
using FamilyGuy.Contracts;
using FamilyGuy.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using User = FamilyGuy.Accounts.Domain.User;

namespace FamilyGuy.Infrastructure.Repositories
{
    public class AccountsRepository : IAccountsPerspective, IAccountsRepository, ISqlRepository
    {
        private readonly FamilyGuyDbContext _dbContext;
        private readonly DbSet<User> _users;

        public AccountsRepository(FamilyGuyDbContext dbContext)
        {
            _dbContext = dbContext;
            _users = dbContext.Users;
        }

        public async Task Add(User user)
        {
            _users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<AccountReadModel> Get(Guid userId)
        {
            User user = await _users.FindAsync(userId);
            return AccountReadModel(user);
        }

        public async Task<AccountReadModel> Get(string userName)
        {
            User user = await _users.FirstOrDefaultAsync(x => x.UserName == userName);
            return user == null ? null : AccountReadModel(user);
        }

        public async Task<AccountWithCredentialsModel> GetUserWithCredentials(string userName)
        {
            User user = await _users.FirstOrDefaultAsync(x => x.UserName == userName);
            return AccountWithCredentialsModel(user);
        }

        private static AccountWithCredentialsModel AccountWithCredentialsModel(User user)
        {
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
    }
}
