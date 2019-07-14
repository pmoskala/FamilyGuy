using FamilyGuy.IntegrationTests.IntegrationTests.Mocks.UserApi;
using RestEase;
using System.Net;
using Xunit;

namespace FamilyGuy.IntegrationTests.IntegrationTests
{
    public class UserRegistrationTests : TestBase
    {
        [Fact]
        public async void ExampleTest()
        {
            Bootstrap.Run(new string[0], builder => { });

            using (Response<string> accounts = await RestClient.For<IAccountsApi>(Url).GetAccounts())
            {
                Assert.Equal(HttpStatusCode.OK, accounts.ResponseMessage.StatusCode);
            }
        }
    }
}
