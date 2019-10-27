using Autofac;
using FamilyGuy.Infrastructure;
using FamilyGuy.Persistence.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading;

namespace FamilyGuy
{
    public class Bootstrap
    {
        public static void Run(string[] args, Action<ContainerBuilder> overrideDependencies = null, string environmentName = null)
        {
            if (overrideDependencies != null)
            {
                Startup.RegisterExternalTypes = overrideDependencies;
            }

            BaseUrl.Current = "http://localhost:5000"; //todo take from settings
            IWebHostBuilder whb = WebHost.CreateDefaultBuilder(args)
                .UseKestrel();

            if (environmentName != null)
                whb.UseEnvironment(environmentName);

            whb.UseSerilog(ConfigureLogging);

            whb.UseStartup<Startup>();


            if (environmentName == null)
            {
                IWebHost webHost = whb.Build();
                CreateDbIfNotExists(webHost);
                webHost.Run();
            }
            else
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    whb.Build().Run();
                });
            }
        }

        private static void CreateDbIfNotExists(IWebHost host)
        {
            using IServiceScope scope = host.Services.CreateScope();
            IServiceProvider services = scope.ServiceProvider;
            ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                FamilyGuyDbContext context = services.GetRequiredService<FamilyGuyDbContext>();
                context.Database.Migrate();
                logger.LogDebug("Database migrations applied successful.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }

        private static void ConfigureLogging(WebHostBuilderContext webHostingContext, LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration.ReadFrom.Configuration(webHostingContext.Configuration);
        }
    }
}
