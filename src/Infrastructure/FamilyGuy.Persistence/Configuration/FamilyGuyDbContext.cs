using FamilyGuy.Accounts.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyGuy.Persistence.Configuration
{
    public class FamilyGuyDbContext : DbContext
    {
        private readonly SqlSettings _settings;
        public DbSet<User> Users { get; set; }

        public FamilyGuyDbContext(DbContextOptions<FamilyGuyDbContext> options) : base(options)
        {

        }

        public FamilyGuyDbContext(DbContextOptions<FamilyGuyDbContext> options, SqlSettings settings) : base(options)
        {
            _settings = settings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;
            if (_settings.InMemory)
            {
                optionsBuilder.UseInMemoryDatabase("FamilyGuyInMemoryDatabase");
                return;
            }
            optionsBuilder.UseSqlServer(_settings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<User> itemBuilder = modelBuilder.Entity<User>();
            itemBuilder.HasKey(x => x.Id);
        }
    }
}
