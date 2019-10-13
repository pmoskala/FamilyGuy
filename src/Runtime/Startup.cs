using Autofac;
using Autofac.Extensions.DependencyInjection;
using FamilyGuy.ActionFilters;
using FamilyGuy.Infrastructure.DI;
using FamilyGuy.Infrastructure.Extensions;
using FamilyGuy.Middleware;
using FamilyGuy.Persistence.Configuration;
using FamilyGuy.TestingAuth;
using FamilyGuy.UserApi.DI;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace FamilyGuy
{
    public class Startup
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public IConfiguration Configuration { get; }
        public static Action<ContainerBuilder> RegisterExternalTypes { get; set; } = builder => { };
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            _hostingEnvironment = hostingEnvironment;
            Configuration = configuration;
            _logger = loggerFactory.CreateLogger("Startup logger");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            LogSettings();
            services.AddControllers(opt => opt.Filters.Add(typeof(FluentValidationActionFilter)))
                .AddApplicationPart(typeof(AutofacUserApiModule).Assembly)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddControllersAsServices()
                .AddFluentValidation(fvc =>
                {
                    fvc.RegisterValidatorsFromAssemblyContaining<AutofacUserApiModule>();
                });

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

        private void LogSettings()
        {
            if (!_hostingEnvironment.IsDevelopment())
                return;

            JwtSettings jwtSettings = Configuration.GetSettings<JwtSettings>();
            _logger.LogWarning($"{nameof(jwtSettings.Issuer)}: {jwtSettings.Issuer}");
            _logger.LogWarning($"{nameof(jwtSettings.ExpiryMinutes)}: {jwtSettings.ExpiryMinutes}");
            _logger.LogWarning($"{nameof(jwtSettings.Key)}: {jwtSettings.Key}");

            SqlSettings sqlSettings = Configuration.GetSettings<SqlSettings>();
            _logger.LogWarning($"{nameof(sqlSettings.ConnectionString)}: {sqlSettings.ConnectionString}");
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FamilyGuy API", Version = "v1" });
            });
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
        public void Configure(IApplicationBuilder app)
        {
            app.UseResponseTimeMeasurementMiddleware();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "FamilyGuy Api v1"); });

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseRouting();

            // todo limit this later, it's ok for the time being
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}