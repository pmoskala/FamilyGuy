using System.Threading.Tasks;

namespace FamilyGuy.UserApi.Services
{
    public interface IAuthService
    {
        Task<AuthenticatedUserReadModel> Authenticate(string userName, string password);
    }
}
