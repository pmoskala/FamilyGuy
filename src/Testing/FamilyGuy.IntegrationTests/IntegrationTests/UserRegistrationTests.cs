﻿using FamilyGuy.Accounts.AccountQuery.Model;
using FamilyGuy.Infrastructure.InMemoryRepositories;
using FamilyGuy.IntegrationTests.IntegrationTests.UserApi;
using FamilyGuy.UserApi.Controllers;
using FamilyGuy.UserApi.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RestEase;
using System;
using System.Net.Http.Headers;
using Xunit;

namespace FamilyGuy.IntegrationTests.IntegrationTests
{
    public class UserRegistrationTests : TestBase, IDisposable
    {
        [Fact]
        public async void UserCreationProcessTests()
        {
            // Setup
            Bootstrap.Run(new string[0], builder =>
            {

                //builder.RegisterType<TestSmtpClient>().AsImplementedInterfaces();
            }, "IntegrationTesting");

            Guid userId = Guid.NewGuid();
            PostAccountModel postAccountModel = new PostAccountModel()
            {
                Id = userId,
                LoginName = Guid.NewGuid().ToString(),
                Name = "Ala",
                Surname = "Kociak",
                Password = Guid.NewGuid().ToString(),
                TelephoneNumber = "555-123-321",
                Email = "ala.ma.kotowska@gmail.com"
            };

            // Act
            HttpResponseHeaders responseMessageHeaders;
            using (Response<string> response = await RestClient.For<IAccountsApi>(Url).PostAccount(postAccountModel))
            {
                Assert.True(response.ResponseMessage.IsSuccessStatusCode);
                responseMessageHeaders = response.ResponseMessage.Headers;
                Assert.True(responseMessageHeaders.Location.IsAbsoluteUri);
            }

            using (Response<string> response = await RestClient.For<IConfirmationApi>(responseMessageHeaders.Location)
                .PutConfirmation(new ConfirmationModel { Confirmed = true }))
            {
                Assert.True(response.ResponseMessage.IsSuccessStatusCode);
            }

            using (Response<AccountReadModel> response = await RestClient.For<IAccountsApi>(Url).GetAccount(userId))
            {
                Assert.True(response.ResponseMessage.IsSuccessStatusCode);
                AccountReadModel accountReadModel = response.GetContent();
                Assert.Equal(postAccountModel.LoginName, accountReadModel.LoginName);
                Assert.Equal(postAccountModel.Name, accountReadModel.Name);
                Assert.Equal(postAccountModel.Surname, accountReadModel.Surname);
            }
        }

        [Fact]
        public async void UserCreationProcessTests_ShouldFailWhenUserNameIsNotUnique()
        {
            // Setup
            Bootstrap.Run(new string[0], builder =>
            {
                //builder.RegisterType<TestSmtpClient>().AsImplementedInterfaces();
            }, "IntegrationTesting");

            Guid userId = Guid.NewGuid();
            PostAccountModel postAccountModel = new PostAccountModel()
            {
                Id = userId,
                LoginName = Guid.NewGuid().ToString(),
                Name = "Ala",
                Surname = "Kociak",
                Password = Guid.NewGuid().ToString(),
                TelephoneNumber = "555-123-321",
                Email = "ala.ma.kotowska@gmail.com"
            };

            // Act
            using (Response<string> response = await RestClient.For<IAccountsApi>(Url).PostAccount(postAccountModel))
            {
                Assert.True(response.ResponseMessage.IsSuccessStatusCode);
                HttpResponseHeaders responseMessageHeaders = response.ResponseMessage.Headers;
                Assert.True(responseMessageHeaders.Location.IsAbsoluteUri);
            }

            using (Response<ModelStateDictionary> response = await RestClient.For<IAccountsApi>(Url).PostAccountError(postAccountModel))
            {
                Assert.False(response.ResponseMessage.IsSuccessStatusCode);
            }
        }

        public void Dispose()
        {
            InMemoryAccountsRepository.Users.Clear();
        }
    }
}
