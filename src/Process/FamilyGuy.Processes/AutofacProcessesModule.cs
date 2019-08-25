using Autofac;
using FamilyGuy.Contracts.Communication.Interfaces;
using FamilyGuy.Processes.UserRegistration;

namespace FamilyGuy.Processes
{
    public class AutofacProcessesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<SagaRepository>().AsImplementedInterfaces();

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
