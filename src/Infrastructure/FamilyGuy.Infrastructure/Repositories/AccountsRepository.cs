using FamilyGuy.Accounts;
using FamilyGuy.Accounts.AccountQuery;
using FamilyGuy.Accounts.AccountQuery.Model;
using FamilyGuy.Accounts.Domain;
using FamilyGuy.Contracts;
using FamilyGuy.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

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
