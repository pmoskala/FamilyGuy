namespace FamilyGuy.Processes.UserRegistration
{
    public interface IPasswordHasher
    {
        string Hash(string password);
    }
}