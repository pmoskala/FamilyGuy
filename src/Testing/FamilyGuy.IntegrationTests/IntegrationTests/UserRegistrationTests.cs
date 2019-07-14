using FamilyGuy.Infrastructure.InMemoryRepositories;
using FamilyGuy.IntegrationTests.IntegrationTests.Mocks.UserApi;
using RestEase;
using System;
using System.Net;
using Xunit;

namespace FamilyGuy.IntegrationTests.IntegrationTests
{
    public class UserRegistrationTests : TestBase, IDisposable
    {
        [Fact]
        public async void ExampleTest()
        {
            Bootstrap.Run(new string[0], builder => { }, "IntegrationTesting");

            using (Response<TempUserResult> response = await RestClient.For<IAccountsApi>(Url).GetAccounts())
            {
                Assert.Equal(HttpStatusCode.OK, response.ResponseMessage.StatusCode);
                TempUserResult user = response.GetContent();
                Assert.Equal("ala", user.UserName);
            }
        }

        public void Dispose()
        {
            InMemoryUserRepository.Users.Clear();
        }
    }

    public class TempUserResult
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
