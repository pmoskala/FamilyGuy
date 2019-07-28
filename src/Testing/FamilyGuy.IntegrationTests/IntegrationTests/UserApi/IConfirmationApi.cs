using FamilyGuy.UserApi.Controllers;
using RestEase;
using System.Threading.Tasks;

namespace FamilyGuy.IntegrationTests.IntegrationTests.UserApi
{
    public interface IConfirmationApi
    {
        [Put]
        Task<Response<string>> PutConfirmation([Body]ConfirmationModel model);
    }
}
