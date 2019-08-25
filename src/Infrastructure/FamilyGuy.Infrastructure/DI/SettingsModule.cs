using Autofac;
using FamilyGuy.Infrastructure.Extensions;
using FamilyGuy.Persistence.Configuration;
using FamilyGuy.Settings;
using Microsoft.Extensions.Configuration;

namespace FamilyGuy.Infrastructure.DI
{
    public class SettingsModule : Module
    {
        private readonly IConfiguration _configuration;

        public SettingsModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_configuration.GetSettings<SqlSettings>())
                .SingleInstance();
            builder.RegisterInstance(_configuration.GetSettings<JwtSettings>())
                .SingleInstance();
        }
    }
}
