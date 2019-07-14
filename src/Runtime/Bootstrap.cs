using Autofac;
using FamilyGuy.Infrastructure;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
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

            BaseUrl.Current = "http://localhost:5000";
            IWebHostBuilder whb = WebHost.CreateDefaultBuilder(args)
                .UseKestrel();

            if (environmentName != null)
                whb.UseEnvironment(environmentName);

            whb.UseStartup<Startup>();

            ThreadPool.QueueUserWorkItem(state => { whb.Build().Run(); });
        }
    }
}
