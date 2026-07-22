namespace MusicHub.Data.Models
{
    public class Album
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public DateTime ReleaseDate { get; set; }

        /* Calculated property has two variants:
         - Computed Column in DB Server - property to be mapped to computed column 
           (value is calculated duing query execution)
         - Calculated Property in Client - property NOT mapped to DB Column
           (value is calculated during query execution, but in client memory)
         */
        /*
         Drawback of this calculated property is that always 
         all Songs need to be fetched during Album fetch.
         */
        /* If the navigation collection consists of many Entities, 
         * then business logic in Client on adding can handle this use-case.*/
        public decimal Price 
            => this.Songs.Sum(s => s.Price);

        public int? ProducerId { get; set; }

        public virtual Producer? Producer { get; set; }

        public virtual ICollection<Song> Songs { get; set; } 
                = new HashSet<Song>();
    }
}
