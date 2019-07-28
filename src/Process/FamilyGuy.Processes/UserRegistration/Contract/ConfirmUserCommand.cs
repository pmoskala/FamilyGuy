using FamilyGuy.Contracts.Communication.Interfaces;
using System;

namespace FamilyGuy.Processes.UserRegistration.Contract
{
    public class ConfirmUserCommand : ICommand
    {
        public Guid ConfirmationId { get; set; }
        public bool Confirmed { get; set; }
    }
}