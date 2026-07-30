using Cadastre.Data;
using Cadastre.DataProcessor.ExportDtos;
using Cadastre.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace Cadastre.DataProcessor
{
    public class Serializer
    {
        public static string ExportPropertiesWithOwners(CadastreContext dbContext)
        {
            ExportPropertyDto[] propertiesWithOwners = dbContext
                .Properties
                .AsNoTracking()
                .Where(p => DateTime.Compare(p.DateOfAcquisition, new DateTime(2000, 1, 1)) > 0)
                .OrderByDescending(p => p.DateOfAcquisition)
                .ThenBy(p => p.PropertyIdentifier)
                .Select(p => new ExportPropertyDto()
                {
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    Address = p.Address,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy"),
                    Owners = p.PropertiesCitizens
                        .OrderBy(pc => pc.Citizen.LastName)
                        .Select(pc => new ExportOwnerDto()
                        {
                            LastName = pc.Citizen.LastName,
                            MaritalStatus = pc.Citizen.MaritalStatus.ToString()
                        })
                        // .OrderBy(pc => pc.LastName)
                        .ToArray()
                })
                .ToArray();

            string jsonResult = JsonConvert
                .SerializeObject(propertiesWithOwners, Formatting.Indented);

            return jsonResult;
        }

        public static string ExportFilteredPropertiesWithDistrict(CadastreContext dbContext)
        {
            ExportFilteredPropertyDto[] filteredProperties = dbContext
                .Properties
                .AsNoTracking()
                .Where(p => p.Area >= 100)
                .OrderByDescending(p => p.Area)
                .ThenBy(p => p.DateOfAcquisition)
                .Select(p => new ExportFilteredPropertyDto()
                {
                    PostalCode = p.District.PostalCode,
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area.ToString(),
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy")
                })
                .ToArray();

            string xmlResult = XmlSerializerWrapper
                .Serialize(filteredProperties, "Properties");

            return xmlResult;
        }
    }
}
