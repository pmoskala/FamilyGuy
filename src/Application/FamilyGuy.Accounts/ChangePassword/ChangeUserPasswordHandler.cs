using System.Threading.Tasks;
using FamilyGuy.Accounts.AccountQuery.Model;
using FamilyGuy.Accounts.ChangePassword.Contract;
using FamilyGuy.Contracts.Communication.Interfaces;
using FamilyGuy.Processes.UserRegistration;

namespace FamilyGuy.Accounts.ChangePassword
{
    public class ChangeUserPasswordHandler : ICommandHandler<ChangeUserPasswordCommand>
    {
        private readonly IAccountsRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        public ChangeUserPasswordHandler(IAccountsRepository repository, IPasswordHasher passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(ChangeUserPasswordCommand command)
        {
            string salt = await _passwordHasher.GetSalt();
            string passwordHash = await _passwordHasher.HashString(command.Password, salt);
            await _repository.UpdatePassword(command.UserId, passwordHash, salt);
        }
    }
}