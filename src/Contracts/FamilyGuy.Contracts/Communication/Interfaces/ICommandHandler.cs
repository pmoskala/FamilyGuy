using System.Threading.Tasks;

namespace FamilyGuy.Contracts.Communication.Interfaces
{
    public interface ICommandHandler<in T>
    {
        Task Handle(T command);
    }
}