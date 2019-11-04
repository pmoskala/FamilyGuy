using Automatonymous;
using FamilyGuy.Accounts.CreateAccount.Contract;
using FamilyGuy.Contracts.Communication.Interfaces;
using FamilyGuy.Processes.UserRegistration.Contract;
using System.Linq;
using System.Threading.Tasks;


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
                    .ThenAsync(CopyDataToSaga)
                    .Then(SendRegistrationNotification)
                    .TransitionTo(WaitingForConfirmation));

            During(WaitingForConfirmation,
                When(ConfirmUserCommand)
                    .ThenAsync(CreateUserAccount)
                    .TransitionTo(Final));
        }

        private async Task CopyDataToSaga(BehaviorContext<UserRegistrationSagaData, RegisterUserCommand> context)
        {
            string salt = await _passwordHasher.GetSalt();
            context.Instance.Id = context.Data.Id;
            context.Instance.BaseUrl = context.Data.BaseUrl;
            context.Instance.LoginName = context.Data.UserName;
            context.Instance.Name = context.Data.FirstName;
            context.Instance.Surname = context.Data.LastName;
            context.Instance.PasswordHash = await _passwordHasher.HashString(context.Data.Password, salt);
            context.Instance.PasswordSalt = salt;
            context.Instance.Email = context.Data.Email;
            context.Instance.TelephoneNumber = context.Data.TelephoneNumber;
        }

        private void SendRegistrationNotification(BehaviorContext<UserRegistrationSagaData> context)
        {
            // todo send notification
            //_commandBus.Send(new SendNotificationCommand()
            //{
            //    UserName = context.Instance.UserName,
            //    Body = $"<a href=\"{context.Instance.BaseUrl}/Confirmation/{context.Instance.Id}\">Click her to confirm</a>",
            //    Title = "Registration confirmation",
            //    NotificationType = "RegistrationConfirmation"
            //});
        }

        private async Task CreateUserAccount(BehaviorContext<UserRegistrationSagaData> context)
        {
            await _commandBus.Send(new CreateAccountCommand()
            {
                Id = context.Instance.Id,
                LoginName = context.Instance.LoginName,
                Name = context.Instance.Name,
                Surname = context.Instance.Surname,
                BaseUrl = context.Instance.BaseUrl,
                Email = context.Instance.Email,
                PasswordHash = context.Instance.PasswordHash,
                PasswordSalt = context.Instance.PasswordSalt,
                TelephoneNumber = context.Instance.TelephoneNumber
            });
        }
    }
}