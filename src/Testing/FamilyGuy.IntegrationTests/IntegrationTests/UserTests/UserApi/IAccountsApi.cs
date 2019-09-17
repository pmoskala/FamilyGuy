using System;
using System.Net.Http;
using System.Threading.Tasks;
using FamilyGuy.Accounts.AccountQuery.Model;
using FamilyGuy.UserApi.Model;
using FamilyGuy.UserApi.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RestEase;

namespace FamilyGuy.IntegrationTests.IntegrationTests.UserTests.UserApi
{
    public interface IAccountsApi
    {
        [Post("api/v1.0/accounts")]
        Task<Response<string>> PostAccount([Body]PostAccountModel model);

        [Post("api/v1.0/accounts")]
        [AllowAnyStatusCode]
        Task<Response<ModelStateDictionary>> PostAccountError([Body]PostAccountModel model);

        [Post("api/v1.0/accounts/authenticate")]
        [AllowAnyStatusCode]
        Task<Response<AuthenticatedUserReadModel>> PostAuthenticate([Body]PostUserAuthenticationModel model);

        [Get("api/v1.0/accounts/{id}")]
        Task<Response<AccountReadModel>> GetAccount([Path]Guid id);
        
        [Put("api/v1.0/accounts/authenticate")]
        Task<HttpResponseMessage> PutPasswordChange([Body]PasswordChangePutModel model);
    }
}
