using FamilyGuy.Accounts.Domain;
using System;
using System.Threading.Tasks;

namespace FamilyGuy.Accounts
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task<User> Get(Guid id);
    }
}
