namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {

            using MusicHubDbContext context =
                new MusicHubDbContext();

            // DbInitializer.ResetDatabase(context);

            //Test your solutions here
            // string result = ExportAlbumsInfo(context, 9);
            // Console.WriteLine(result);

            string result = ExportSongsAboveDuration(context, 4);
            Console.WriteLine(result);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder sb = new StringBuilder();

            /* AsNoTracking() -> tells to EF Cor that we do NOT need to
               track changes and to perform identity resolution 
               (caching some part of the data in the Change Tracker) */
            /* Use .AsNoTracking() for read-only queries to increase performance,
               because ChangeTracker is not involved. */
            /* EF Core can decide to perform some operations at client-side,
               despite working with IQueryable<T> and query is still not materialized! */
            /* Formatting operations are responsibility of the front-end 
               and should NOT be delegated to the Db Server! */
            /* Anonymous types are NOT tracked Entitities by ChangeTracker */
            /* Property of an anonymous types that are of Entity Types are TRACKED by ChangeTracker */
            /* Some complex queries can't be translated by EF Core Db Provider! */
            /* Can't translate exception -> move materialization of the query (.ToArray()) before failing LINQ */
            /* .AsEnumerable() can be used to delegate the query execution at client-side earlier */
            /* It is recommended to use .Skip()/.Take() to limit the total amount of the result data.  */
            /* .Skip()/.Take() are slow LINQ operations, because .Skip() passes through all skipped rows. */
            /* Basic Pagination with Number Navigation -> .Skip()/.Take() */
            /* Advanced Pagination with sequence loading -> Unique .OrderBy() + .Where(paginationFilter) */
            /* Use .AsSplitQuery() when your query is performing many LEFT JOINs with other of the same query level (CROSS-PRODUCT) */
            /* .AsSplitQuery() -> Split "fat" query into several small queries and results of these queries will be joined client-side */
            var albumsInfo = context.Albums
                .AsNoTracking()
                .AsSplitQuery()
                .Include(a => a.Songs)
                .Where(a => a.ProducerId == producerId)
                .Select(a => new
                {
                    a.Name,
                    // ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy") -> In newer versions of EF Core in could happen before materialization of the query
                    // ReleaseDate = ReleaseDateFormatting(a.ReleaseDate),
                    a.ReleaseDate,
                    ProducerName = a.Producer != null ?
                                a.Producer.Name : null,
                    Songs = a.Songs
                        .Select(s => new
                        {
                            SongName = s.Name,
                            SongPrice = s.Price,
                            WriterName = s.Writer.Name
                        })
                        .OrderByDescending(s => s.SongName)
                        .ThenBy(s => s.WriterName)
                        .ToArray(), // It's recommended to materialize the "subquery", so everything after that to be moved to the client-side
                    TotalPrice = a.Price
                })
                .AsEnumerable()
                .OrderByDescending(a => a.TotalPrice)
                .ToArray(); // Materialize the query to trigger the execution and retrieve the data from the database

            /* After query is materialized, every LINQ operation is client-side in-memory! */
            /* Change Tracking is disabled! */
            foreach (var album in albumsInfo)
            {
                sb
                    .AppendLine($"-AlbumName: {album.Name}")
                    .AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}")
                    .AppendLine($"-ProducerName: {album.ProducerName}")
                    .AppendLine("-Songs:");

                int songIdx = 1;
                foreach (var s in album.Songs)
                {
                    sb
                        .AppendLine($"---#{songIdx++}")
                        .AppendLine($"---SongName: {s.SongName}")
                        .AppendLine($"---Price: {s.SongPrice:F2}")
                        .AppendLine($"---Writer: {s.WriterName}");
                }

                sb.AppendLine($"-AlbumPrice: {album.TotalPrice:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ReleaseDateFormatting(DateTime releaseDate)
        {
            /* EF Core can't execute the formatting in the database, 
               since it doesn't know the implementation of this method! */
            return releaseDate.ToString("MM/dd/yyyy");
        }

        /* Duration.TotalSeconds can't be translated in SQL */
        /* I. Move filtering (selection) to client-side */
        /* II. Use .FromSql() to start with raw SQL query containing the .Where() clause */
        /* Use .Join() to remove redundant cross join data from the query */
        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            StringBuilder sb = new StringBuilder();

            var songs = context.Songs
                // .FromSqlInterpolated($"SELECT * FROM [Songs] WHERE DATEPART(SECOND, [Duration]) + 60 * DATEPART(MINUTE, [Duration]) + 3600 * DATEPART(HOUR, [Duration]) > {duration}")
                .AsNoTracking()
                // .AsSplitQuery()
                .Select(s => new
                {
                    s.Name,
                    Performers = s.SongPerformers
                        .Select(sp => sp.Performer) // Skip collection in EF Core
                        .Select(p => p.FirstName + " " + p.LastName)
                        .OrderBy(p => p)
                        .ToArray(),
                    WriterName = s.Writer.Name,
                    AlbumProducer = (s.Album != null && s.Album.Producer != null)
                        ? s.Album.Producer.Name : null,
                    s.Duration
                })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.WriterName)
                .AsEnumerable()
                .Where(s => s.Duration.TotalSeconds > duration);
                // .ToArray();

            int songIdx = 1;
            foreach (var song in songs)
            {
                sb
                    .AppendLine($"-Song #{songIdx++}")
                    .AppendLine($"---SongName: {song.Name}")
                    .AppendLine($"---Writer: {song.WriterName}");

                foreach (var performerFullName in song.Performers)
                {
                    sb.AppendLine($"---Performer: {performerFullName}");
                }

                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.Duration.ToString("c")}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
