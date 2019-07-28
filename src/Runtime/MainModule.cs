using Autofac;
using FamilyGuy.Accounts;
using FamilyGuy.Infrastructure.DI;
using FamilyGuy.Infrastructure.InMemoryRepositories;
using FamilyGuy.Processes;
using FamilyGuy.UserApi.DI;
using Microsoft.Extensions.Configuration;

namespace FamilyGuy
{
    public class MainModule : Module
    {
        private readonly IConfiguration _configuration;

        private readonly string _hostingEnvironmentEnvironmentName;

        public MainModule(IConfiguration configuration, string hostingEnvironmentEnvironmentName)
        {
            _configuration = configuration;
            _hostingEnvironmentEnvironmentName = hostingEnvironmentEnvironmentName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<AutofacAccountsModule>();
            builder.RegisterModule<AutofacProcessesModule>();

            builder.RegisterModule<AutofacUserApiModule>();

            if (_hostingEnvironmentEnvironmentName == "Production" || _hostingEnvironmentEnvironmentName == "Development")
                builder.RegisterModule<SqlModule>();
            if (_hostingEnvironmentEnvironmentName == "IntegrationTesting")
                builder.RegisterModule<InMemoryRepositoryModule>();

            builder.RegisterModule<AutofacInfrastructureModule>();

            builder.RegisterModule(new SettingsModule(_configuration));
        }
    }
}
