using Autofac;
using FamilyGuy.Infrastructure.DI;
using FamilyGuy.UserApi.DI;

namespace FamilyGuy
{
    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<AutofacUserApiModule>();
            builder.RegisterModule<MongoModule>();
        }
    }
}
