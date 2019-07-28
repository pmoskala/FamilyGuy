using Automatonymous;
using FamilyGuy.Contracts.Communication.Interfaces;
using FamilyGuy.Processes.UserRegistration.Contract;
using System.Threading.Tasks;

namespace FamilyGuy.Processes.UserRegistration.Handler
{
    class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
    {
        private readonly ICommandBus _commandBus;
        private readonly ISagaRepository _sagaRepository;
        private readonly IQuery _query;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(ICommandBus commandBus,
            ISagaRepository sagaRepository,
            IQuery query,
            IPasswordHasher passwordHasher)
        {
            _commandBus = commandBus;
            _sagaRepository = sagaRepository;
            _query = query;
            _passwordHasher = passwordHasher;
        }
        public async Task Handle(RegisterUserCommand command)
        {
            if (await _sagaRepository.Get<UserRegistrationSagaData>(x => x.LoginName == command.LoginName) != null)
            {
                throw new LoginNameAlreadyUsedException(command.LoginName);
            }

            UserRegistrationSaga saga = new UserRegistrationSaga(_commandBus, _passwordHasher);
            UserRegistrationSagaData data = new UserRegistrationSagaData { Id = command.Id };
            await saga.RaiseEvent(data, saga.RegisterUserCommand, command);
            await _sagaRepository.Save(data.Id, data);
        }
    }
}
