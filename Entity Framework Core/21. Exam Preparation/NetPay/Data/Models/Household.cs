using System.ComponentModel.DataAnnotations;
using static NetPay.Common.ValidationConstants;

namespace NetPay.Data.Models
{
    public class Household
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(HouseholdContactPersonMaxLength)]
        public string ContactPerson { get; set; } = null!;

        [MaxLength(HouseholdEmailMaxLength)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(HouseholdPhoneNumberLength)]
        public string PhoneNumber { get; set; } = null!;

        // Many expenses will be added to the household -> List()
        public virtual ICollection<Expense> Expenses { get; set; }
            = new List<Expense>();
    }
}
