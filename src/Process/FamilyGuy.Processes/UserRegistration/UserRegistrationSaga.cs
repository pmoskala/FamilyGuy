using Automatonymous;
using FamilyGuy.Accounts.CreateAccount.Contract;
using FamilyGuy.Contracts.Communication.Interfaces;
using FamilyGuy.Processes.UserRegistration.Contract;
using System.Linq;


namespace FamilyGuy.Processes.UserRegistration
{
    internal class UserRegistrationSaga : AutomatonymousStateMachine<UserRegistrationSagaData>
    {
        private readonly ICommandBus _commandBus;
        private readonly IPasswordHasher _passwordHasher;

        public Event<RegisterUserCommand> RegisterUserCommand { get; set; }
        public Event<ConfirmUserCommand> ConfirmUserCommand { get; set; }

        public State WaitingForConfirmation { get; set; }

        public UserRegistrationSaga(ICommandBus commandBus, IPasswordHasher passwordHasher)
        {
            _commandBus = commandBus;
            _passwordHasher = passwordHasher;
            UserRegistrationSagaData.States = States.ToDictionary(f => f.Name, f => f);

            Initially(
                When(RegisterUserCommand)
                    .Then(CopyDataToSaga)
                    .Then(SendRegistrationNotification)
                    .TransitionTo(WaitingForConfirmation));

            During(WaitingForConfirmation,
                When(ConfirmUserCommand)
                    .Then(CreateUserAccount)
                    .TransitionTo(Final));
        }

        private void CopyDataToSaga(BehaviorContext<UserRegistrationSagaData, RegisterUserCommand> context)
        {
            context.Instance.Id = context.Data.Id;
            context.Instance.BaseUrl = context.Data.BaseUrl;
            context.Instance.LoginName = context.Data.LoginName;
            context.Instance.Name = context.Data.Name;
            context.Instance.Surname = context.Data.Surname;
            context.Instance.PasswordHash = _passwordHasher.Hash(context.Data.Password);
            context.Instance.Email = context.Data.Email;
            context.Instance.TelephoneNumber = context.Data.TelephoneNumber;
        }

        private void SendRegistrationNotification(BehaviorContext<UserRegistrationSagaData> context)
        {
            // todo send notification
            //_commandBus.Send(new SendNotificationCommand()
            //{
            //    LoginName = context.Instance.LoginName,
            //    Body = $"<a href=\"{context.Instance.BaseUrl}/Confirmation/{context.Instance.Id}\">Click her to confirm</a>",
            //    Title = "Registration confirmation",
            //    NotificationType = "RegistrationConfirmation"
            //});
        }

        private void CreateUserAccount(BehaviorContext<UserRegistrationSagaData> context)
        {
            _commandBus.Send(new CreateAccountCommand()
            {
                Id = context.Instance.Id,
                LoginName = context.Instance.LoginName,
                Name = context.Instance.Name,
                Surname = context.Instance.Surname,
                BaseUrl = context.Instance.BaseUrl,
                Email = context.Instance.Email,
                PasswordHash = context.Instance.PasswordHash,
                TelephoneNumber = context.Instance.TelephoneNumber
            });
        }
    }
}