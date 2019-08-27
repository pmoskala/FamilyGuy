using FamilyGuy.Accounts.AccountQuery.Model;
using System;
using System.Threading.Tasks;

namespace FamilyGuy.Accounts.AccountQuery
{
    public interface IAccountsPerspective
    {
        Task<AccountReadModel> Get(Guid userId);
        Task<AccountReadModel> Get(string userName);
        Task<AccountWithCredentialsModel> GetUserWithCredentials(string userName);
    }
}