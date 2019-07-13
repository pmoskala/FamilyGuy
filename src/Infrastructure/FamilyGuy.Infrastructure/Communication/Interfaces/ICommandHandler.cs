namespace FamilyGuy.Infrastructure.Communication.Interfaces
{
    public interface ICommandHandler<T>
    {
        void Handle(T command);
    }
}