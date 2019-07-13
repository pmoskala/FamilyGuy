namespace FamilyGuy.Infrastructure.Communication.Interfaces
{
    public interface IEventHandler<T>
    {
        void Handle(T @event);
    }
}