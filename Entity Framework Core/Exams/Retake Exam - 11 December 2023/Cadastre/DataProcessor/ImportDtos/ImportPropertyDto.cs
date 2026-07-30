using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Cadastre.Common.ValidationConstants;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("Property")]
    public class ImportPropertyDto
    {
        [Required]
        [StringLength(PropertyIdentifierMaxLength, MinimumLength = PropertyIdentifierMinLength)]
        [XmlElement("PropertyIdentifier")]
        public string PropertyIdentifier { get; set; } = null!;

        [Required]
        [Range(0, int.MaxValue)]
        [XmlElement("Area")]
        public string Area { get; set; } = null!;

        [StringLength(PropertyDetailsMaxLength, MinimumLength = PropertyDetailsMinLength)]
        [XmlElement("Details")]
        public string? Details { get; set; }

        [Required]
        [StringLength(PropertyAddressMaxLength, MinimumLength = PropertyAddressMinLength)]
        [XmlElement("Address")]
        public string Address { get; set; } = null!;

        [Required]
        [XmlElement("DateOfAcquisition")]
        public string DateOfAcquisition { get; set; } = null!;
    }
}
