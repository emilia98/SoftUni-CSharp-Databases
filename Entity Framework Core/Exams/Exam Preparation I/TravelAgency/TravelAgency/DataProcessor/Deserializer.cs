using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Text.Json;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ImportDtos;
using TravelAgency.Utilities;

namespace TravelAgency.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedCustomer = "Successfully imported customer - {0}";
        private const string SuccessfullyImportedBooking = "Successfully imported booking. TourPackage: {0}, Date: {1}";

        public static string ImportCustomers(TravelAgencyContext dbContext, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<ImportCustomerDto>? customerDtos = XmlSerializerWrapper
                .Deserialize<ImportCustomerDto[]>(xmlString, "Customers");

            ICollection<Customer> existingCustomers = dbContext
                .Customers
                .AsNoTracking()
                .ToArray();
            ICollection<Customer> customersToPersist = new List<Customer>();

            if (customerDtos == null)
            {
                return sb.ToString();
            }

            foreach (ImportCustomerDto customerDto in customerDtos)
            {
                if (!IsValid(customerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isDuplicated = existingCustomers
                    .Any(c => c.FullName == customerDto.FullName
                           || c.Email == customerDto.Email
                           || c.PhoneNumber == customerDto.PhoneNumber);
                isDuplicated |= customersToPersist
                    .Any(c => c.FullName == customerDto.FullName
                           || c.Email == customerDto.Email
                           || c.PhoneNumber == customerDto.PhoneNumber);

                if (isDuplicated)
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                Customer newCustomer = new Customer()
                {
                    FullName = customerDto.FullName,
                    Email = customerDto.Email,
                    PhoneNumber = customerDto.PhoneNumber
                };

                customersToPersist.Add(newCustomer);

                sb.AppendLine(string.Format(SuccessfullyImportedCustomer, newCustomer.FullName));
            }

            dbContext.Customers.AddRange(customersToPersist);
            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportBookings(TravelAgencyContext dbContext, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<ImportBookingDto>? bookingDtos = JsonConvert
                .DeserializeObject<ImportBookingDto[]>(jsonString);

            if (bookingDtos == null)
            {
                return sb.ToString();
            }

            ICollection<Booking> bookingsToPersist = new List<Booking>();
            IEnumerable<Customer> existingCustomers = dbContext
                .Customers
                .AsNoTracking()
                .ToArray();
            IEnumerable<TourPackage> existingTourPackages = dbContext
                .TourPackages
                .AsNoTracking()
                .ToArray();

            foreach (ImportBookingDto bookingDto in bookingDtos)
            {
                if(!IsValid(bookingDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isBookingDateValid = DateTime.TryParseExact(
                    bookingDto.BookingDate,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime bookingDate);

                if (!isBookingDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                int? customerId = existingCustomers
                    .FirstOrDefault(c => c.FullName == bookingDto.CustomerName)?.Id;
                int? tourPackageId = existingTourPackages
                    .FirstOrDefault(tp => tp.PackageName == bookingDto.TourPackageName)?.Id;

                if (customerId == null || tourPackageId == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Booking newBooking = new Booking()
                {
                    BookingDate = bookingDate,
                    CustomerId = customerId.Value,
                    TourPackageId = tourPackageId.Value
                };

                bookingsToPersist.Add(newBooking);

                sb.AppendLine(string.Format(
                    SuccessfullyImportedBooking,
                    bookingDto.TourPackageName,
                    bookingDate.ToString("yyyy-MM-dd")));
            }

            dbContext.Bookings.AddRange(bookingsToPersist);
            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validateContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                string currValidationMessage = validationResult.ErrorMessage;
            }

            return isValid;
        }
    }
}
