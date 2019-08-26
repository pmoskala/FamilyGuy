﻿using FamilyGuy.Accounts.AccountQuery.Model;
using FamilyGuy.UserApi.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RestEase;
using System;
using System.Threading.Tasks;

namespace FamilyGuy.IntegrationTests.IntegrationTests.UserApi
{
    public interface IAccountsApi
    {
        [Post("api/v1.0/accounts")]
        Task<Response<string>> PostAccount([Body]PostAccountModel model);

        [Post("api/v1.0/accounts")]
        [AllowAnyStatusCode]
        Task<Response<ModelStateDictionary>> PostAccountError([Body]PostAccountModel model);

        [Get("api/v1.0/accounts/{id}")]
        Task<Response<AccountReadModel>> GetAccount([Path]Guid id);
    }
}