namespace TravelAgency.DataProcessor.ExportDtos
{
    public class ExportCustomerDto
    {
        public string FullName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public ICollection<ExportBookingDto> Bookings { get; set; } = Array.Empty<ExportBookingDto>();
    }
}
