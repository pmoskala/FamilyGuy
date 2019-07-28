using System.Threading.Tasks;

namespace FamilyGuy.Contracts.Communication.Interfaces
{
    public interface IEventHandler<in T>
    {
        Task Handle(T @event);
    }
}