using System;
using FamilyGuy.Accounts.Domain;
using System.Threading.Tasks;

namespace FamilyGuy.Accounts
{
    public interface IAccountsRepository
    {
        Task Add(User user);
        Task UpdatePassword(Guid userId, string passwordHash, string salt);
    }
}
