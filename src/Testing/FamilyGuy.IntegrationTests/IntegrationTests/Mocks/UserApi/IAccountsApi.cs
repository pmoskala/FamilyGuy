using RestEase;
using System.Threading.Tasks;

namespace FamilyGuy.IntegrationTests.IntegrationTests.Mocks.UserApi
{
    public interface IAccountsApi
    {
        [Get("api/v1.0/accounts")]
        Task<Response<string>> GetAccounts();
    }
}
