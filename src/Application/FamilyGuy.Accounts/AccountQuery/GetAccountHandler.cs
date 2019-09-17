using FamilyGuy.Accounts.AccountQuery.Model;
using FamilyGuy.Contracts.Communication.Interfaces;
using System;
using System.Threading.Tasks;
using FamilyGuy.Accounts.AccountExceptions;

namespace FamilyGuy.Accounts.AccountQuery
{
    public class GetAccountHandler :
        IQueryHandler<Task<AccountReadModel>, Guid>,
        IQueryHandler<Task<AccountReadModel>, string>, // todo this is bad, use custom class not string
        IQueryHandler<Task<AccountWithCredentialsModel>, AccountByUserNameQuery>
    {
        private readonly IAccountsPerspective _perspective;

        public GetAccountHandler(IAccountsPerspective perspective)
        {
            _perspective = perspective;
        }

        public async Task<AccountReadModel> Handle(Guid userId)
        {
            return await _perspective.Get(userId);
        }

        public async Task<AccountReadModel> Handle(string request)
        {
            return await _perspective.Get(request);
        }

        public async Task<AccountWithCredentialsModel> Handle(AccountByUserNameQuery query)
        {
            AccountWithCredentialsModel accountWithCredentialsModel = await _perspective.GetUserWithCredentials(query.UserName);
            if (accountWithCredentialsModel != null)
                return accountWithCredentialsModel;
            throw new UserNotFoundFgException(query.UserName);
        }
    }
}