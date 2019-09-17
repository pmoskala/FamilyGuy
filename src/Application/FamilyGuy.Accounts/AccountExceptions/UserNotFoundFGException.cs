using System;
using FamilyGuy.Contracts.Exceptions;

namespace FamilyGuy.Accounts.AccountExceptions
{
    public class UserNotFoundFgException : FgBaseNotFoundException
    {
        public string UserName { get; }

        public UserNotFoundFgException(string userName)
        {
            UserName = userName;
        }
    }
}