using System.ComponentModel.DataAnnotations;
using static TravelAgency.Common.ValidationConstants;

namespace TravelAgency.Data.Models
{
    public class TourPackage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(TourPackagePackageNameMaxLength)]
        public string PackageName { get; set; } = null!;

        [MaxLength(TourPackageDescriptionMaxLength)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } 
            = new List<Booking>();

        public virtual ICollection<TourPackageGuide> TourPackagesGuides { get; set; } 
            = new HashSet<TourPackageGuide>();
    }
}