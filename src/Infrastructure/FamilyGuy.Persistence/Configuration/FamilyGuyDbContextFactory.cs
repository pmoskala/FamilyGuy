using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace FamilyGuy.Persistence.Configuration
{
    public class FamilyGuyDbContextFactory : IDesignTimeDbContextFactory<FamilyGuyDbContext>
    {
        public FamilyGuyDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<FamilyGuyDbContext> optionsBuilder = new DbContextOptionsBuilder<FamilyGuyDbContext>();

            SqlSettings sqlSettings = new SqlSettings();
            new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(GetAppSettingsFileName())
                .AddEnvironmentVariables()
                .Build()
                .GetSection("Sql")
                .Bind(sqlSettings);

            optionsBuilder.UseSqlServer(sqlSettings.ConnectionString);
            return new FamilyGuyDbContext(optionsBuilder.Options);
        }

        private string GetAppSettingsFileName()
        {
            string environmentName = Environment.GetEnvironmentVariable("EnvironmentName") ?? "Development";
            return environmentName == "Development" ? "appsettings.Development.json" : "appsettings.json";
        }
    }
}
