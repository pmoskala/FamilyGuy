namespace FamilyGuy.Infrastructure.Communication.Interfaces
{
    public interface ICommandBus
    {
        void Send<T>(T command);
    }
}