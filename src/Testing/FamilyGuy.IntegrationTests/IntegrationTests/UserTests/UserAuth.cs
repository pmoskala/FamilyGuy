using System;
using System.Collections.Generic;
using FamilyGuy.Infrastructure.InMemoryRepositories;
using FamilyGuy.IntegrationTests.IntegrationTests.UserTests.UserApi;
using FamilyGuy.UserApi.Controllers;
using FamilyGuy.UserApi.Model;
using FamilyGuy.UserApi.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using RestEase;
using Xunit;
using HttpResponseHeaders = System.Net.Http.Headers.HttpResponseHeaders;

namespace FamilyGuy.IntegrationTests.IntegrationTests.UserTests
{
    public class UserAuth : UserTestBase, IDisposable
    {
        [Fact]
        public async void UserAuthenticationProcessTest()
        {
            // Setup
            Bootstrap.Run(new string[0], builder =>
            {
                //builder.RegisterType<TestSmtpClient>().AsImplementedInterfaces();
            }, "IntegrationTesting");

            Guid userId = Guid.NewGuid();
            PostAccountModel postAccountModel = CreatePostAccountModel(userId);

            // Act
            HttpResponseHeaders responseMessageHeaders;
            using (Response<string> response = await RestClient.For<IAccountsApi>(Url).PostAccount(postAccountModel))
            {
                Assert.True(response.ResponseMessage.IsSuccessStatusCode);
                responseMessageHeaders = response.ResponseMessage.Headers;
                Assert.True(responseMessageHeaders.Location.IsAbsoluteUri);
            }

            using (Response<string> response = await RestClient.For<IConfirmationApi>(responseMessageHeaders.Location)
                .PutConfirmation(new ConfirmationModel {Confirmed = true}))
            {
                Assert.True(response.ResponseMessage.IsSuccessStatusCode);
            }

            PostUserAuthenticationModel postUserAuthenticationModel = new PostUserAuthenticationModel
            {
                UserName = postAccountModel.LoginName,
                Password = postAccountModel.Password
            };

            using (Response<AuthenticatedUserReadModel> response =
                await RestClient.For<IAccountsApi>(Url).PostAuthenticate(postUserAuthenticationModel))
            {
                Assert.True(response.ResponseMessage.IsSuccessStatusCode);
                AuthenticatedUserReadModel authUser = response.GetContent();
                Assert.Equal(postAccountModel.Name, authUser.Name);
                Assert.Equal(postAccountModel.Surname, authUser.Surname);
                string[] tokenParts = authUser.Token.Split('.');
                Assert.Equal(3, tokenParts.Length);
            }
        }

        public void Dispose()
        {
            InMemoryAccountsRepository.Users.Clear();
        }
    }
}