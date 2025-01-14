using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateTimeValue)
            {
                if (dateTimeValue < DateTime.Now)
                {
                    return new ValidationResult("Departure time cannot be in the past.");
                }
            }
            return ValidationResult.Success!;
        }
    }
}
