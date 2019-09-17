using System.Threading.Tasks;
using FamilyGuy.UserApi.Controllers;
using RestEase;

namespace FamilyGuy.IntegrationTests.IntegrationTests.UserTests.UserApi
{
    public interface IConfirmationApi
    {
        [Put]
        Task<Response<string>> PutConfirmation([Body]ConfirmationModel model);
    }
}
