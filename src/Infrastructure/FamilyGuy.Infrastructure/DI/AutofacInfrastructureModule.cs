using Autofac;
using FamilyGuy.Contracts.Communication.Interfaces;
using FamilyGuy.Processes.UserRegistration;

namespace FamilyGuy.Infrastructure.DI
{
    public class AutofacInfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Bus>().AsImplementedInterfaces();
            builder.RegisterType<PasswordHasher>().AsImplementedInterfaces();

        }
    }
}
