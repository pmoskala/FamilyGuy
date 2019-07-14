using FamilyGuy.Accounts.Domain;
using System.Threading.Tasks;

namespace FamilyGuy.Accounts
{
    interface IUserRepository
    {
        Task Add(User user);
    }
}
