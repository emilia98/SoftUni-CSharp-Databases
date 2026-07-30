namespace Cadastre.Data
{
    using Cadastre.Data.Models;
    using Microsoft.EntityFrameworkCore;
    public class CadastreContext : DbContext
    {
        public CadastreContext()
        {
            
        }

        public CadastreContext(DbContextOptions options)
            :base(options)
        {
            
        }

        public DbSet<District> Districts { get; set; } = null!;

        public DbSet<Property> Properties { get; set; } = null!;

        public DbSet<Citizen> Citizens { get; set; } = null!;

        public DbSet<PropertyCitizen> PropertiesCitizens { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /* Defining the composite key */
            modelBuilder.Entity<PropertyCitizen>(entity =>
            {
                entity.HasKey(pc => new { pc.PropertyId, pc.CitizenId });
            });
        }
    }
}
