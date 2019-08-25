using FamilyGuy.Accounts.CreateAccount.Contract;
using FamilyGuy.Accounts.Domain;
using FamilyGuy.Contracts.Communication.Interfaces;
using System.Threading.Tasks;

namespace FamilyGuy.Accounts.CreateAccount
{
    class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand>
    {
        private readonly IAccountsRepository _accountRepository;

        public CreateAccountCommandHandler(IAccountsRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task Handle(CreateAccountCommand command)
        {
            User user = new User(command.Id, command.LoginName, command.Name, command.Surname, command.Email, command.PasswordHash, command.TelephoneNumber);
            await _accountRepository.Add(user);
        }
    }
}