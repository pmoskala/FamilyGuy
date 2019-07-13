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
        public static void Run(string[] args, Action<ContainerBuilder> overrideDependencies = null)
        {
            if (overrideDependencies != null)
            {
                Startup.RegisterExternalTypes = overrideDependencies;
            }

            BaseUrl.Current = "http://localhost:5000";
            IWebHostBuilder whb = WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseStartup<Startup>();

            ThreadPool.QueueUserWorkItem(state => { whb.Build().Run(); });
        }
    }
}
