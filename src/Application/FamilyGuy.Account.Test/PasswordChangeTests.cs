using Autofac;
using FamilyGuy.Accounts;
using FamilyGuy.Accounts.AccountQuery;
using FamilyGuy.Accounts.AccountQuery.Model;
using FamilyGuy.Accounts.ChangePassword.Contract;
using FamilyGuy.Accounts.DI;
using FamilyGuy.Accounts.Domain;
using FamilyGuy.Contracts.Communication.Interfaces;
using FamilyGuy.Infrastructure.DI;
using FamilyGuy.Infrastructure.InMemoryRepositories;
using FamilyGuy.Infrastructure.PasswordHasher;
using FamilyGuy.Processes.UserRegistration;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FamilyGuy.Account.Test
{
    [Collection("PasswordChangeTests")]
    public class PasswordChangeTests
    {
        private readonly IContainer _container;

        public PasswordChangeTests()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterModule(new AutofacInfrastructureModule());
            builder.RegisterModule(new AutofacAccountsModule());
            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .InstancePerLifetimeScope();
            builder.RegisterInstance(new NullLoggerFactory())
                .As<ILoggerFactory>()
                .SingleInstance();

            builder.RegisterType<InMemoryAccountsRepository>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<PasswordHasher>()
                .As<IPasswordHasher>()
                .InstancePerDependency();

            _container = builder.Build();
        }

        [Fact]
        public async Task ChangeUserPasswordCommand_UserPasswordAndSaltChanged()
        {
            // Arrange
            const string userName = "UserName";
            Guid userId = Guid.NewGuid();
            const string userPassword = "WeakPassword1";
            const string passwordSalt = "SaltySalt";
            User user = new User(userId, userName, "Dirk", "Gently", "gently@detectives.org", userPassword, passwordSalt, "555-1234");
            IAccountsRepository accountsRepository = _container.Resolve<IAccountsRepository>();
            await accountsRepository.Add(user);

            // Act
            ICommandBus commandBus = _container.Resolve<ICommandBus>();
            string newPassword = "KubusPuchatek654";
            await commandBus.Send(new ChangeUserPasswordCommand
            {
                UserId = userId,
                Password = newPassword
            });

            IAccountsPerspective accountsPerspective = _container.Resolve<IAccountsPerspective>();
            AccountWithCredentialsModel userWithCredentials = await accountsPerspective.GetUserWithCredentials(userName);

            // Assert
            userWithCredentials.PasswordHash
                .Should().NotBeNullOrWhiteSpace("Password should be hashed and contained")
                .And.NotBe(userPassword)
                .And.NotBe(newPassword);

            userWithCredentials.PasswordSalt
                .Should().NotBeEmpty()
                .And.NotBe(passwordSalt);
        }
    }
}