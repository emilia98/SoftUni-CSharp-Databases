namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using Cadastre.Data.Enumerations;
    using Cadastre.Data.Models;
    using Cadastre.DataProcessor.ImportDtos;
    using Cadastre.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid Data!";
        private const string SuccessfullyImportedDistrict =
            "Successfully imported district - {0} with {1} properties.";
        private const string SuccessfullyImportedCitizen =
            "Succefully imported citizen - {0} {1} with {2} properties.";

        public static string ImportDistricts(CadastreContext dbContext, string xmlDocument)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<ImportDistrictDto>? districtDtos = XmlSerializerWrapper
                .Deserialize<ImportDistrictDto[]>(xmlDocument, "Districts");

            if (districtDtos == null)
            {
                return sb.ToString();
            }

            IEnumerable<District> existingDistricts = dbContext
                .Districts
                .AsNoTracking()
                .ToArray();
            IEnumerable<Property> existingProperties = dbContext
                .Properties
                .AsNoTracking()
                .ToArray();

            ICollection<District> districtsToPersist = new List<District>();
            ICollection<Property> propertiesToPersist = new List<Property>();

            foreach (ImportDistrictDto districtDto in districtDtos)
            {
                if (!IsValid(districtDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                // validate the region
                bool isRegionValidDef = Enum.TryParse<Region>(
                    districtDto.Region, 
                    out Region region);

                if (!isRegionValidDef)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (existingDistricts.Any(d => d.Name == districtDto.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                District newDistrict = new District()
                {
                    Name = districtDto.Name,
                    PostalCode = districtDto.PostalCode,
                    Region = region
                };

                districtsToPersist.Add(newDistrict);

                foreach (ImportPropertyDto propertyDto in districtDto.Properties)
                {
                    if (!IsValid(propertyDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isDateOfAquistitionValid = DateTime
                        .TryParseExact(
                            propertyDto.DateOfAcquisition,
                            "dd/MM/yyyy",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out DateTime dateOfAquisition
                        );
                    bool isAreaValid = int.TryParse(propertyDto.Area, out int area);

                    if (!isDateOfAquistitionValid || !isAreaValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    // if (existingDistricts.Any(d => d.Properties.))
                    /* If we import properties only on new districts, this will work */
                    if (newDistrict.Properties.Any(p => p.PropertyIdentifier == propertyDto.PropertyIdentifier) 
                        || existingProperties.Any(p => p.PropertyIdentifier == propertyDto.PropertyIdentifier)
                        // || propertiesToPersist.Any(p => p.PropertyIdentifier == propertyDto.PropertyIdentifier)
                        )
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (newDistrict.Properties.Any(p => p.Address == propertyDto.Address)
                        || existingProperties.Any(p => p.Address == propertyDto.Address)
                        // || propertiesToPersist.Any(p => p.PropertyIdentifier == propertyDto.PropertyIdentifier)
                        )
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Property newProperty = new Property()
                    {
                        PropertyIdentifier = propertyDto.PropertyIdentifier,
                        Address = propertyDto.Address,
                        Area = area,
                        Details = propertyDto.Details,
                        DateOfAcquisition = dateOfAquisition,
                        DistrictId = newDistrict.Id,
                        District = newDistrict
                    };

                    newDistrict.Properties.Add(newProperty);
                }

                // dbContext.Properties.AddRange();

                sb.AppendLine(string.Format(SuccessfullyImportedDistrict, newDistrict.Name, newDistrict.Properties.Count()));
            }

            dbContext.Districts.AddRange(districtsToPersist);
            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<ImportCitizenDto>? citizenDtos =
                JsonConvert.DeserializeObject<ImportCitizenDto[]>(jsonDocument);

            if (citizenDtos == null)
            {
                return sb.ToString();
            }

            ICollection<Citizen> citizensToPersist = new List<Citizen>();
            IEnumerable<Property> existingProperties = dbContext.Properties
                .AsNoTracking()
                .ToArray();
            // ICollection<PropertyCitizen> propertyCitizensToPersist = new List<PropertyCitizen>();

            foreach (ImportCitizenDto citizenDto in citizenDtos)
            {
                if (!IsValid(citizenDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isBirthDateValid = DateTime
                    .TryParseExact(citizenDto.BirthDate,
                    "dd-MM-yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime citizenBirthDate);
                bool isMaritalStatusValid = Enum
                    .TryParse<MaritalStatus>(
                        citizenDto.MaritalStatus,
                        out MaritalStatus citizenMaritalStatus
                    );

                if (!isBirthDateValid || !isMaritalStatusValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Citizen newCitizen = new Citizen()
                {
                    FirstName = citizenDto.FirstName,
                    LastName = citizenDto.LastName,
                    BirthDate = citizenBirthDate,
                    MaritalStatus = citizenMaritalStatus
                };

                foreach (int propertyId in citizenDto.Properties.Distinct())
                {
                    if (!existingProperties.Any(p => p.Id == propertyId))
                    {
                        continue;
                    }

                    PropertyCitizen newPropertyCitizen = new PropertyCitizen()
                    {
                        PropertyId = propertyId,
                        Citizen = newCitizen
                    };
                    newCitizen.PropertiesCitizens.Add(newPropertyCitizen);
                }

                citizensToPersist.Add(newCitizen);
                sb.AppendLine(string.Format(
                    SuccessfullyImportedCitizen,
                    newCitizen.FirstName,
                    newCitizen.LastName,
                    newCitizen.PropertiesCitizens.Count()));
            }

            dbContext.Citizens.AddRange(citizensToPersist);
            dbContext.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
