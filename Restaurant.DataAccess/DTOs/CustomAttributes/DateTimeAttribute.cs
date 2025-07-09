using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs.CustomAttributes
{
    internal class DateTimeAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Perform regex validation
            if (Convert.ToDateTime(value) <= DateTime.Now)
            {
                // Use the ErrorMessage property defined on the attribute, or a default.
                return new ValidationResult(ErrorMessage ?? "Date Time Is Not Valid");
            }

            return ValidationResult.Success; // Validation passed
        }
    }
}
