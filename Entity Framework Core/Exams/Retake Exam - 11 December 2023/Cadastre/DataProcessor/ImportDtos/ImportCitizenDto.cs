using System.ComponentModel.DataAnnotations;
using static Cadastre.Common.ValidationConstants;

namespace Cadastre.DataProcessor.ImportDtos
{
    public class ImportCitizenDto
    {
        [Required]
        [StringLength(CitizenLastNameMaxLength, MinimumLength = CitizenFirstNameMinLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(CitizenLastNameMaxLength, MinimumLength = CitizenLastNameMinLength)]
        public string LastName { get; set; } = null!;

        [Required]
        public string BirthDate { get; set; } = null!;

        [Required]
        public string MaritalStatus { get; set; } = null!;

        public ICollection<int> Properties { get; set; } 
            = Array.Empty<int>();
    }
}
