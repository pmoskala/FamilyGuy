using Autofac;
using Autofac.Extensions.DependencyInjection;
using FamilyGuy.UserApi.DI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace FamilyGuy
{
    public class Startup
    {
        public static Action<ContainerBuilder> RegisterExternalTypes { get; set; } = builder => { };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddApplicationPart(typeof(AutofacUserApiModule).Assembly)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddControllersAsServices();

            AddSwaggerApiDocumentationFramework(services);
            ConfigureApiVersioningFramework(services);

            IContainer container = ConfigureAutofacDiContainer(services);
            return new AutofacServiceProvider(container);
        }

        private static void AddSwaggerApiDocumentationFramework(IServiceCollection services)
        {
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "FamilyGuy API", Version = "v1" }); });
        }

        private static IContainer ConfigureAutofacDiContainer(IServiceCollection services)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new MainModule());
            RegisterExternalTypes(builder);
            builder.Populate(services);
            IContainer container = builder.Build();
            return container;
        }

        private static void ConfigureApiVersioningFramework(IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NicePress Api v1");
            });

            app.UseMvc();
        }
    }
}
