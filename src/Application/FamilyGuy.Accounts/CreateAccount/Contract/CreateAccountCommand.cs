using FamilyGuy.Contracts.Communication.Interfaces;
using System;

namespace FamilyGuy.Accounts.CreateAccount.Contract
{
    public class CreateAccountCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { set; get; }
        public string LoginName { get; set; }
        public string BaseUrl { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string TelephoneNumber { get; set; }
        public string PasswordSalt { get; set; }
    }
}
