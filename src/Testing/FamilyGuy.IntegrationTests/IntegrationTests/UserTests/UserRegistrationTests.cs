using Autofac;
using FamilyGuy.Accounts.AccountQuery.Model;
using FamilyGuy.Infrastructure.InMemoryRepositories;
using FamilyGuy.IntegrationTests.IntegrationTests.UserTests.UserApi;
using FamilyGuy.UserApi.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using RestEase;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Xunit;

namespace FamilyGuy.IntegrationTests.IntegrationTests.UserTests
{
    [Collection("IntegrationTests")]
    public class UserRegistrationTests : TestBase
    {
        [Fact]
        public async void UserCreationProcessTests()
        {
            // Setup
            InMemoryAccountsRepository inMemoryAccountsRepository = new InMemoryAccountsRepository();

            Bootstrap.Run(new string[0], builder =>
            {
                builder.RegisterInstance(inMemoryAccountsRepository).AsImplementedInterfaces().SingleInstance();
            }, "IntegrationTesting");

            Guid userId = Guid.NewGuid();
            AccountPostModel accountPostModel = CreatePostAccountModel(userId);

            // Act
            HttpResponseHeaders responseMessageHeaders;
            using (Response<string> response = await RestClient.For<IAccountsApi>(Url).PostAccount(accountPostModel))
            {
                Assert.True(response.ResponseMessage.IsSuccessStatusCode);
                responseMessageHeaders = response.ResponseMessage.Headers;
                Assert.True(responseMessageHeaders.Location.IsAbsoluteUri);
            }

            using (Response<string> response = await RestClient.For<IConfirmationApi>(responseMessageHeaders.Location)
                .PutConfirmation(new ConfirmationPutModel { Confirmed = true }))
            {
                Assert.True(response.ResponseMessage.IsSuccessStatusCode);
            }

            using (Response<AccountReadModel> response = await RestClient.For<IAccountsApi>(Url).GetAccount(userId))
            {
                Assert.True(response.ResponseMessage.IsSuccessStatusCode);
                AccountReadModel accountReadModel = response.GetContent();
                Assert.Equal(accountPostModel.LoginName, accountReadModel.LoginName);
                Assert.Equal(accountPostModel.Name, accountReadModel.Name);
                Assert.Equal(accountPostModel.Surname, accountReadModel.Surname);
            }
        }

        [Fact]
        public async void UserCreationProcessTests_ShouldFailWhenUserNameIsNotUniqueAndRegistrationProcessIsNotComplete()
        {
            // Setup
            InMemoryAccountsRepository inMemoryAccountsRepository = new InMemoryAccountsRepository();

            Bootstrap.Run(new string[0], builder =>
            {
                builder.RegisterInstance(inMemoryAccountsRepository).AsImplementedInterfaces().SingleInstance();
            }, "IntegrationTesting");

            Guid userId = Guid.NewGuid();
            AccountPostModel accountPostModel = CreatePostAccountModel(userId);

            // Act
            using (Response<string> response = await RestClient.For<IAccountsApi>(Url).PostAccount(accountPostModel))
            {
                Assert.True(response.ResponseMessage.IsSuccessStatusCode);
                HttpResponseHeaders responseMessageHeaders = response.ResponseMessage.Headers;
                Assert.True(responseMessageHeaders.Location.IsAbsoluteUri);
            }

            using (Response<ModelStateDictionary> response = await RestClient.For<IAccountsApi>(Url).PostAccountError(accountPostModel))
            {
                Assert.False(response.ResponseMessage.IsSuccessStatusCode);
                Dictionary<string, string[]> deserializeObject = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(response.StringContent);
                Assert.Equal($"The provided username {accountPostModel.LoginName} already exists", deserializeObject["LoginName"][0]);
            }
        }

        [Fact]
        public async void UserCreationProcessTests_ShouldFailWhenUserNameIsNotUnique()
        {
            // Setup
            InMemoryAccountsRepository inMemoryAccountsRepository = new InMemoryAccountsRepository();

            Bootstrap.Run(new string[0], builder =>
            {
                builder.RegisterInstance(inMemoryAccountsRepository).AsImplementedInterfaces().SingleInstance();
            }, "IntegrationTesting");

            Guid userId = Guid.NewGuid();
            AccountPostModel accountPostModel = CreatePostAccountModel(userId);

            // Act
            HttpResponseHeaders responseMessageHeaders;
            using (Response<string> response = await RestClient.For<IAccountsApi>(Url).PostAccount(accountPostModel))
            {
                Assert.True(response.ResponseMessage.IsSuccessStatusCode);
                responseMessageHeaders = response.ResponseMessage.Headers;
                Assert.True(responseMessageHeaders.Location.IsAbsoluteUri);
            }

            using (Response<string> response = await RestClient.For<IConfirmationApi>(responseMessageHeaders.Location)
                .PutConfirmation(new ConfirmationPutModel { Confirmed = true }))
            {
                Assert.True(response.ResponseMessage.IsSuccessStatusCode);
            }

            using (Response<ModelStateDictionary> response = await RestClient.For<IAccountsApi>(Url).PostAccountError(accountPostModel))
            {
                Assert.False(response.ResponseMessage.IsSuccessStatusCode);
                Dictionary<string, string[]> deserializeObject = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(response.StringContent);
                Assert.Equal($"The provided username {accountPostModel.LoginName} already exists", deserializeObject["LoginName"][0]);
            }
        }

        private static AccountPostModel CreatePostAccountModel(Guid userId)
        {
            return new AccountPostModel
            {
                Id = userId,
                LoginName = Guid.NewGuid().ToString(),
                Name = "Ala",
                Surname = "Kociak",
                Password = Guid.NewGuid().ToString(),
                TelephoneNumber = "555-123-321",
                Email = "ala.ma.kotowska@gmail.com"
            };
        }
    }
}
