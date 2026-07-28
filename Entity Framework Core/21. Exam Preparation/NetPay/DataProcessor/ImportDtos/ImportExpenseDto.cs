using System.ComponentModel.DataAnnotations;
using static NetPay.Common.ValidationConstants;

namespace NetPay.DataProcessor.ImportDtos
{
    public class ImportExpenseDto
    {
        [Required]
        [StringLength(ExpenseNameMaxLength, MinimumLength = ExpenseNameMinLength)]
        public string ExpenseName { get; set; } = null!;

        // [Required]
        [Range(typeof(decimal), ExpenseAmountRangeMinValue, ExpenseAmountRangeMaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public string DueDate { get; set; } = null!;

        [Required]
        public string PaymentStatus { get; set; } = null!;

        // [Required]
        public int HouseholdId { get; set; }

        // [Required]
        public int ServiceId { get; set; }
    }
}
