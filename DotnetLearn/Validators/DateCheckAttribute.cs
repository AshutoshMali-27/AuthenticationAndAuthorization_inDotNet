using System.ComponentModel.DataAnnotations;

namespace DotnetLearn.Validators
{
    public class DateCheckAttribute :ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var date = (DateTime?)value;
            if(date < DateTime.Now)
            {
                return new ValidationResult("The date must be greter than or equal to today date");
            }

            return ValidationResult.Success;
        }
    }

}
