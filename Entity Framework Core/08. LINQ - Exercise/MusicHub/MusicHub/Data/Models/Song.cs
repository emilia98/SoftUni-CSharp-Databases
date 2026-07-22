using MusicHub.Data.Models.Enums;

namespace MusicHub.Data.Models
{
    public class Song
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        /* TimeSpan can be converted to long (Ticks) or string (Invariant culture) Db Type */
        /* Timespan is required type by default */
        public TimeSpan Duration { get; set; }

        /* DateOnly is converted to DATE SQL Type */
        /* DateOnly is required type by default */
        public DateTime CreatedOn { get; set; }

        /* Enumeration is converted to INT SQL Type (SQLServer) */
        /* PostgreSQL offer enum DB Type with custom ValueConverter in EF Core */
        /* Since Enumerations are numeric values, they are required by default */
        public Genre Genre { get; set; }

        public int? AlbumId { get; set; }

        public virtual Album? Album { get; set; }

        public int WriterId { get; set; }

        public virtual Writer Writer { get; set; } = null!;

        public decimal Price { get; set; }

        public virtual ICollection<SongPerformer> SongPerformers { get; set; }
            = new HashSet<SongPerformer>();
    }
}
