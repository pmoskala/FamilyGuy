using Automatonymous;
using System;
using System.Collections.Generic;

namespace FamilyGuy.Processes.UserRegistration
{
    public class UserRegistrationSagaData : ISagaData
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string LoginName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PasswordHash { get; set; }
        public string BaseUrl { get; set; }
        public string TelephoneNumber { get; set; }
        public State CurrentState { get; set; }
        public static IDictionary<string, State> States { get; set; }
    }

    //marker interface
    public interface ISagaData
    {
    }


}
