namespace MusicHub.Data
{
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.EntityConfiguration;
    using MusicHub.Data.Models;
    using System.Reflection;

    public class MusicHubDbContext : DbContext
    {
        public MusicHubDbContext()
        {
        }

        public MusicHubDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Album> Albums { get; set; } = null!;

        public virtual DbSet<Performer> Performers { get; set; } = null!;

        public virtual DbSet<Producer> Producers { get; set; } = null!;

        public virtual DbSet<Song> Songs { get; set; } = null!;

        public virtual DbSet<SongPerformer> SongsPerformers { get; set; } = null!;

        public virtual DbSet<Writer> Writers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            /*
            builder.ApplyConfiguration(new AlbumConfiguration());
            builder.ApplyConfiguration(new SongConfiguration());
            builder.ApplyConfiguration(new PerformerConfiguration());
            builder.ApplyConfiguration(new SongPerformerConfiguration());
            builder.ApplyConfiguration(new ProducerConfiguration());
            builder.ApplyConfiguration(new WriterConfiguration());
            */
            // Convention: All EntityConfigurations are located in the SAME assembly!
            // Pass Assembly of a random EntityConfiguration
            builder.ApplyConfigurationsFromAssembly(typeof(AlbumConfiguration).Assembly);
        }
    }
}
