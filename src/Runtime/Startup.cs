using Autofac;
using Autofac.Extensions.DependencyInjection;
using FamilyGuy.Infrastructure.Extensions;
using FamilyGuy.Persistence.Configuration;
using FamilyGuy.Settings;
using FamilyGuy.TestingAuth;
using FamilyGuy.UserApi.DI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Text;

namespace FamilyGuy
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public IConfiguration Configuration { get; }
        public static Action<ContainerBuilder> RegisterExternalTypes { get; set; } = builder => { };


        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                    .AddApplicationPart(typeof(AutofacUserApiModule).Assembly)
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .AddControllersAsServices();

            services.AddEntityFrameworkSqlServer()
                    .AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<FamilyGuyDbContext>();

            if (_hostingEnvironment.EnvironmentName != "IntegrationTesting")
                ConfigureJwtServices(services);
            else
                FakeTestingAuth(services);

            AddSwaggerApiDocumentationFramework(services);
            ConfigureApiVersioningFramework(services);

            IContainer container = ConfigureAutofacDiContainer(services);
            return new AutofacServiceProvider(container);
        }

        private void ConfigureJwtServices(IServiceCollection services)
        {
            JwtSettings jwtSettings = Configuration.GetSettings<JwtSettings>();
            byte[] key = Encoding.ASCII.GetBytes(jwtSettings.Key);

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        private static void FakeTestingAuth(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test Scheme";
                options.DefaultChallengeScheme = "Test Scheme";
            }).AddTestAuth(o => { });
        }

        private static void AddSwaggerApiDocumentationFramework(IServiceCollection services)
        {
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "FamilyGuy API", Version = "v1" }); });
        }

        private IContainer ConfigureAutofacDiContainer(IServiceCollection services)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new MainModule(Configuration, _hostingEnvironment.EnvironmentName));

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

            // todo limit this later, it's ok for the time being
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NicePress Api v1");
            });

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
