using System;

namespace FamilyGuy.Contracts.Exceptions
{
    public class FgBaseUnauthorizedException : Exception
    {
        public FgBaseUnauthorizedException(string message) : base(message)
        {
        }
    }
}
