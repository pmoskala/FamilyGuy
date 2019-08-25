using FamilyGuy.Accounts.AccountQuery.Model;
using FamilyGuy.Contracts.Communication.Interfaces;
using System;
using System.Threading.Tasks;

namespace FamilyGuy.Accounts.AccountQuery
{
    public class GetAccountHandler : IQueryHandler<Task<AccountReadModel>, Guid>, IQueryHandler<Task<AccountReadModel>, UserAuthenticationModel>
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

        public async Task<AccountReadModel> Handle(UserAuthenticationModel request)
        {
            return await _perspective.Get(request.UserName, request.PasswordHash);
        }
    }
}