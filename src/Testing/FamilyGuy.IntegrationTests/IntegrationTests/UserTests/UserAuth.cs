using Autofac;
using FamilyGuy.Accounts;
using FamilyGuy.Accounts.Domain;
using FamilyGuy.Infrastructure.InMemoryRepositories;
using FamilyGuy.IntegrationTests.IntegrationTests.UserTests.UserApi;
using FamilyGuy.UserApi.Model;
using FamilyGuy.UserApi.Services;
using RestEase;
using System;
using Xunit;

namespace FamilyGuy.IntegrationTests.IntegrationTests.UserTests
{
    public class UserAuth : TestBase, IDisposable
    {

        [Fact]
        public async void UserAuthenticationProcessTest()
        {
            InMemoryAccountsRepository inMemoryAccountsRepository = new InMemoryAccountsRepository();
            // Setup
            Bootstrap.Run(new string[0], builder =>
            {
                builder.RegisterType<DummyPasswordHasher>().AsImplementedInterfaces();
                builder.RegisterInstance(inMemoryAccountsRepository).As<IAccountsRepository>();
            }, "IntegrationTesting");

            User user = new User(Guid.NewGuid(),
                    "userName", "Ala",
                    "Kotowska", "ala@ktowoscy.pl",
                    "2384u982374982374", "872472364",
                    "555-5555");

            await inMemoryAccountsRepository.Add(user);

            UserAuthenticationPostModel userAuthenticationPostModel = new UserAuthenticationPostModel
            {
                UserName = user.UserName,
                Password = user.PasswordHash
            };

            using Response<AuthenticatedUserReadModel> response = await RestClient.For<IAccountsApi>(Url).PostAuthenticate(userAuthenticationPostModel);

            Assert.True(response.ResponseMessage.IsSuccessStatusCode);
            AuthenticatedUserReadModel authUser = response.GetContent();
            Assert.Equal(user.Name, authUser.Name);
            Assert.Equal(user.Surname, authUser.Surname);
            string[] tokenParts = authUser.Token.Split('.');
            Assert.Equal(3, tokenParts.Length);
        }

        public void Dispose()
        {
            InMemoryAccountsRepository.Users.Clear();
        }
    }
}