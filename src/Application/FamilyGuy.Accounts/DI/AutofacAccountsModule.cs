using Autofac;
using FamilyGuy.Contracts.Communication.Interfaces;

namespace FamilyGuy.Accounts.DI
{
    public class AutofacAccountsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(GetType().Assembly)
                .AsClosedTypesOf(typeof(ICommandHandler<>)).AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(GetType().Assembly)
                .AsClosedTypesOf(typeof(IEventHandler<>)).AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(GetType().Assembly)
                .AsClosedTypesOf(typeof(IQueryHandler<,>)).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}