using Automatonymous;
using FamilyGuy.Contracts.Communication.Interfaces;
using FamilyGuy.Processes.UserRegistration.Contract;
using System;
using System.Threading.Tasks;

namespace FamilyGuy.Processes.UserRegistration.Handler
{
    public class ConfirmUserCommandHandler : ICommandHandler<ConfirmUserCommand>
    {
        private readonly ICommandBus _commandBus;
        private readonly ISagaRepository _sagaRepository;
        private readonly IPasswordHasher _passwordHasher;

        public ConfirmUserCommandHandler(ICommandBus commandBus, ISagaRepository sagaRepository, IPasswordHasher passwordHasher)
        {
            _commandBus = commandBus;
            _sagaRepository = sagaRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(ConfirmUserCommand command)
        {
            UserRegistrationSaga saga = new UserRegistrationSaga(_commandBus, _passwordHasher);
            UserRegistrationSagaData data = await _sagaRepository.Get<UserRegistrationSagaData>(command.ConfirmationId);
            if (data == null)
                throw new InvalidOperationException();

            await saga.RaiseEvent(data, saga.ConfirmUserCommand, command);
            await _sagaRepository.Save(command.ConfirmationId, data);
        }
    }
}