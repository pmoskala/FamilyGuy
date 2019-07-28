using Autofac;
using FamilyGuy.Contracts.Communication.Interfaces;

namespace FamilyGuy.Infrastructure.DI
{
    public class AutofacInfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Bus>().AsImplementedInterfaces();
        }
    }
}
