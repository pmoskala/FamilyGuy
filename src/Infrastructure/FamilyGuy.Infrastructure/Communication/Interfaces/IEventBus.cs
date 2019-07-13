namespace FamilyGuy.Infrastructure.Communication.Interfaces
{
    public interface IEventBus
    {
        void Publish<T>(T @event);
    }
}