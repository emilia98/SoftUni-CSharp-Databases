using System.ComponentModel.DataAnnotations;
using static TravelAgency.Common.ValidationConstants;

namespace TravelAgency.DataProcessor.ImportDtos
{
    public class ImportBookingDto
    {
        [Required]
        public string BookingDate { get; set; } = null!;

        [Required]
        public string CustomerName { get; set; } = null!;

        [Required]
        public string TourPackageName { get; set; } = null!;
    }
}
