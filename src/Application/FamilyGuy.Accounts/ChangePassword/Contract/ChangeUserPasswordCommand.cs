using System;
using FamilyGuy.Contracts.Communication.Interfaces;

namespace FamilyGuy.Accounts.ChangePassword.Contract
{
    public class ChangeUserPasswordCommand : ICommand
    {
        public Guid UserId { get; set; }
        public string Password { get; set; }
    }
}