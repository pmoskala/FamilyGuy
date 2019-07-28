using FamilyGuy.Accounts.Domain;
using System.Threading.Tasks;

namespace FamilyGuy.Accounts
{
    public interface IAccountsRepository
    {
        Task Add(User user);
    }
}
