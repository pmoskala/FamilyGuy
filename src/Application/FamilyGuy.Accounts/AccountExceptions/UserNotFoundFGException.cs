using FamilyGuy.Contracts.Exceptions;

namespace FamilyGuy.Accounts.AccountExceptions
{
    public class UserNotFoundFgException : FgBaseNotFoundException
    {

        public UserNotFoundFgException(string userName) : base($"User with username {userName} not found.")
        {
        }
    }
}