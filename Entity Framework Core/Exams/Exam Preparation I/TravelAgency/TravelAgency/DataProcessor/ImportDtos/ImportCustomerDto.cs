using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static TravelAgency.Common.ValidationConstants;

namespace TravelAgency.DataProcessor.ImportDtos
{
    [XmlType("Customer")]
    public class ImportCustomerDto
    {
        [Required]
        [StringLength(CustomerFullNameMaxLength, MinimumLength = CustomerFullNameMinLength)]
        [XmlElement("FullName")]
        public string FullName { get; set; } = null!;

        [Required]
        [StringLength(CustomerEmailMaxLength, MinimumLength = CustomerEmailMinLength)]
        [XmlElement("Email")]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(CustomerPhoneNumberLength, MinimumLength = CustomerPhoneNumberLength)]
        [RegularExpression(CustomerPhoneNumberRegexPattern)]
        [XmlAttribute("phoneNumber")]
        public string PhoneNumber { get; set; } = null!;
    }
}