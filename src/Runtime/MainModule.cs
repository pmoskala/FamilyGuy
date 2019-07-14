using Autofac;
using FamilyGuy.UserApi.DI;

namespace FamilyGuy
{
    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<AutofacUserApiModule>();
        }
    }
}
