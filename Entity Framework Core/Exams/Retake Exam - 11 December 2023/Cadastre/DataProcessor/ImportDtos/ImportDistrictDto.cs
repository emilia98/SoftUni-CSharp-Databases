using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Cadastre.Common.ValidationConstants;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("District")]
    public class ImportDistrictDto
    {
        [Required]
        [XmlAttribute("Region")]
        public string Region { get; set; } = null!;

        [Required]
        [StringLength(DistrictNameMaxLength, MinimumLength = DistrictNameMinLength)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(DistrictPostalCodeLength, MinimumLength = DistrictPostalCodeLength)]
        [RegularExpression(DistrictPostalCodeRegexPattern)]
        [XmlElement("PostalCode")]

        public string PostalCode { get; set; } = null!;

        [XmlArray("Properties")]
        [XmlArrayItem("Property")]
        public ImportPropertyDto[] Properties { get; set; } = Array.Empty<ImportPropertyDto>();
    }
}
