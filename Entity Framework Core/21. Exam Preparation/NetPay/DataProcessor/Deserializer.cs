using Microsoft.EntityFrameworkCore;
using NetPay.Data;
using NetPay.Data.Models;
using NetPay.Data.Models.Enums;
using NetPay.DataProcessor.ImportDtos;
using NetPay.Utilities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace NetPay.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedHousehold = "Successfully imported household. Contact person: {0}";
        private const string SuccessfullyImportedExpense = "Successfully imported expense. {0}, Amount: {1}";

        public static string ImportHouseholds(NetPayContext dbContext, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<ImportHouseholdDto>? householdDtos = XmlSerializerWrapper
                .Deserialize<ImportHouseholdDto[]>(xmlString, "Households");

            if (householdDtos == null)
            {
                /* Return empty output and do not conitnue to import */
                return sb.ToString();
            }

            IEnumerable<Household> existingHouseholds = dbContext
                .Households
                .AsNoTracking()
                .ToArray();

            ICollection<Household> householdsToPersist = new List<Household>();
            
            foreach (ImportHouseholdDto householdDto in householdDtos)
            {
                if (!IsValid(householdDto))
                {
                    /* Skip the entity and append an error message */
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isDuplicate = existingHouseholds
                    .Any(h => h.ContactPerson == householdDto.ContactPerson
                           || (householdDto.Email != null && householdDto.Email == h.Email)
                           || h.PhoneNumber == householdDto.PhoneNumber);

                isDuplicate |= householdsToPersist
                    .Any(h => h.ContactPerson == householdDto.ContactPerson
                           || (householdDto.Email != null && householdDto.Email == h.Email)
                           || h.PhoneNumber == householdDto.PhoneNumber);
                
                if (isDuplicate)
                {
                    /* Skip the entity and append duplication error message */
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                Household household = new Household()
                {
                    ContactPerson = householdDto.ContactPerson,
                    Email = householdDto.Email,
                    PhoneNumber = householdDto.PhoneNumber
                };

                householdsToPersist.Add(household);
                sb.AppendLine(string.Format(SuccessfullyImportedHousehold, household.ContactPerson));
            }

            dbContext.Households.AddRange(householdsToPersist);
            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportExpenses(NetPayContext dbContext, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<ImportExpenseDto>? expenseDtos = 
                JsonConvert.DeserializeObject<ImportExpenseDto[]>(jsonString);

            if (expenseDtos == null)
            {
                /* Return empty output and do not continue the import */
                return sb.ToString();
            }

            IEnumerable<int> validHouseholdIds = dbContext.Households
                .AsNoTracking()
                .Select(h => h.Id)
                .ToArray();
            IEnumerable<int> validServiceIds = dbContext.Services
                .AsNoTracking()
                .Select(s => s.Id)
                .ToArray();

            ICollection<Expense> expensesToPersist = new List<Expense>();

            foreach (ImportExpenseDto expenseDto in expenseDtos)
            {
                if (!IsValid(expenseDto))
                {
                    /* Skip the entity and append error message */
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isDueDateValid = DateTime
                    .TryParseExact(expenseDto.DueDate, 
                        "yyyy-MM-dd", 
                        CultureInfo.InvariantCulture, 
                        DateTimeStyles.None, 
                        out DateTime expenseDueDate);
                bool isPaymentStatusValidDef = Enum
                        .TryParse<PaymentStatus>(
                            expenseDto.PaymentStatus, 
                            out PaymentStatus expensePaymentStatus);

                if (!isDueDateValid || !isPaymentStatusValidDef)
                {
                    /* Skip the entity and append error message */
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!validHouseholdIds.Contains(expenseDto.HouseholdId)
                    || !validServiceIds.Contains(expenseDto.ServiceId))
                {
                    /* Skip the entity and append error message */
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Expense newExpense = new Expense()
                {
                    ExpenseName = expenseDto.ExpenseName,
                    Amount = expenseDto.Amount,
                    DueDate = expenseDueDate,
                    PaymentStatus = expensePaymentStatus,
                    HouseholdId = expenseDto.HouseholdId,
                    ServiceId = expenseDto.ServiceId
                };

                expensesToPersist.Add(newExpense);

                sb.AppendLine(string.Format(SuccessfullyImportedExpense,
                    newExpense.ExpenseName,
                    newExpense.Amount.ToString("F2")));
            }

            dbContext.Expenses.AddRange(expensesToPersist);
            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            foreach(var result in validationResults)
            {
                string currvValidationMessage = result.ErrorMessage;
            }

            return isValid;
        }
    }
}
