using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TravelAgency.Data;
using TravelAgency.Data.Models.Enums;
using TravelAgency.DataProcessor.ExportDtos;
using TravelAgency.Utilities;

namespace TravelAgency.DataProcessor
{
    public class Serializer
    {
        public static string ExportGuidesWithSpanishLanguageWithAllTheirTourPackages(TravelAgencyContext dbContext)
        {
            ExportGuideWithSpanishDto[] guidesWithSpanishDtos = dbContext
                .Guides
                .AsNoTracking()
                .Include(g => g.TourPackagesGuides)
                .ThenInclude(tpg => tpg.TourPackage)
                .Where(g => g.Language == Language.Spanish)
                .OrderByDescending(g => g.TourPackagesGuides.Count())
                .ThenBy(g => g.FullName)
                .AsSplitQuery()
                .AsEnumerable()
                .Select(g => new ExportGuideWithSpanishDto()
                {
                    FullName = g.FullName,
                    TourPackages = g.TourPackagesGuides
                        .Select(tpg => tpg.TourPackage)
                        .OrderByDescending(tp => tp.Price)
                        .ThenBy(tp => tp.PackageName)
                        .Select(tp => new ExportTourPackageDto()
                        {
                            Name = tp.PackageName,
                            Description = tp.Description ?? string.Empty,
                            Price = tp.Price.ToString("F2")
                        })
                        .ToArray()
                })
                .ToArray();

            string xmlResult = XmlSerializerWrapper
                .Serialize(guidesWithSpanishDtos, "Guides");

            return xmlResult;
        }

        public static string ExportCustomersThatHaveBookedHorseRidingTourPackage(TravelAgencyContext dbContext)
        {
            /*
            ExportCustomerDto[] exportCustomersWithHorseRidingDtos = dbContext
                .Bookings
                .AsNoTracking()
                .Where(b => b.TourPackage.PackageName == "Horse Riding Tour")
                .Select(b => b.Customer)
                .AsEnumerable()
                .Select(c => new ExportCustomerDto()
                {
                    FullName = c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Bookings = c.Bookings.Select()
                }).ToArray(); */

            ExportCustomerDto[] exportCustomersWithHorseRidingDtos = dbContext
                .Customers
                .Include(c => c.Bookings)
                .ThenInclude(b => b.TourPackage)
                .AsNoTracking()
                .Where(c => c.Bookings.Any(b => b.TourPackage.PackageName == "Horse Riding Tour"))
                .AsEnumerable()
                .Select(c => new ExportCustomerDto()
                {
                    FullName = c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Bookings = c.Bookings
                        .Where(b => b.TourPackage.PackageName == "Horse Riding Tour")
                        .OrderBy(b => b.BookingDate)
                        .Select(b => new ExportBookingDto()
                        {
                            TourPackageName = b.TourPackage.PackageName,
                            Date = b.BookingDate.ToString("yyyy-MM-dd")
                        })
                        .ToArray()
                })
                .OrderByDescending(c => c.Bookings.Count())
                .ThenBy(c => c.FullName)
                .ToArray();

            string jsonResult = JsonConvert
                .SerializeObject(exportCustomersWithHorseRidingDtos, Formatting.Indented);

            return jsonResult;

        }
    }
}
