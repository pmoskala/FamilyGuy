using System.Threading.Tasks;

namespace FamilyGuy.Contracts.Communication.Interfaces
{
    public interface IEventBus
    {
        Task Publish<T>(T @event) where T : IEvent;
    }
}