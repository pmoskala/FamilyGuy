using FamilyGuy.UserApi.Model;
using RestEase;
using System.Threading.Tasks;

namespace FamilyGuy.IntegrationTests.IntegrationTests.UserTests.UserApi
{
    public interface IConfirmationApi
    {
        [Put]
        Task<Response<string>> PutConfirmation([Body]ConfirmationPutModel model);
    }
}
