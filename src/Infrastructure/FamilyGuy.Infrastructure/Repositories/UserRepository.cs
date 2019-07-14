using FamilyGuy.Accounts;
using FamilyGuy.Accounts.Domain;
using FamilyGuy.Contracts;
using FamilyGuy.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FamilyGuy.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository, ISqlRepository
    {
        private readonly FamilyGuyDbContext _dbContext;
        private readonly DbSet<User> _users;

        public UserRepository(FamilyGuyDbContext dbContext)
        {
            _dbContext = dbContext;
            _users = dbContext.Users;
        }

        public async Task Add(User user)
        {
            _users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> Get(Guid id)
        {
            return await _users.FindAsync(id);
        }
    }
}
