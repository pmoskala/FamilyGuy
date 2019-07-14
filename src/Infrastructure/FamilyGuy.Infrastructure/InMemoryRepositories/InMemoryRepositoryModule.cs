using Autofac;
using System.Reflection;

namespace FamilyGuy.Infrastructure.InMemoryRepositories
{
    public class InMemoryRepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Assembly assembly = typeof(InMemoryRepositoryModule)
                .GetTypeInfo()
                .Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .Where(x => x.IsAssignableTo<IInMemoryRepository>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
