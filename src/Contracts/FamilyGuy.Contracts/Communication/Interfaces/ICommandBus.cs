using System.Threading.Tasks;

namespace FamilyGuy.Contracts.Communication.Interfaces
{
    public interface ICommandBus
    {
        Task Send<T>(T command) where T : ICommand;
    }
}