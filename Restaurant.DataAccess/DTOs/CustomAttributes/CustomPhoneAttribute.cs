using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs.CustomAttributes
{
    public class CustomPhoneAttribute : ValidationAttribute
    {
        // You can define a regex pattern here, or pass it in the constructor
        private readonly string _regexPattern;

        // Example: Egyptian phone numbers often start with 010, 011, 012, 015 and are 11 digits
        // Or a more general international pattern
        public CustomPhoneAttribute()
        {
            // Default pattern if none is provided. This is a very basic example.
            // For production, use a more robust regex or a dedicated library.
            // Example for Egyptian mobile numbers: ^01[0125][0-9]{8}$
            _regexPattern = @"^(\+20|01)[0125][0-9]{8}$"; // Basic: starts with optional +, then 7-20 digits/spaces/hyphens/parentheses
        }

        public CustomPhoneAttribute(string regexPattern)
        {
            _regexPattern = regexPattern;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                // If the field is optional, [Required] attribute should handle null.
                // If it's not required, null is valid here.
                return ValidationResult.Success;
            }

            var phoneNumber = value.ToString();

            if (phoneNumber == "")
            {
                // Treat empty or whitespace-only as invalid if it's required (or not allowed for optional)
                // If [Required] is also on the property, it handles empty/null.
                return new ValidationResult(ErrorMessage ?? "Phone number cannot be empty or whitespace.");
            }

            // Perform regex validation
            if (!Regex.IsMatch(phoneNumber, _regexPattern))
            {
                // Use the ErrorMessage property defined on the attribute, or a default.
                return new ValidationResult(ErrorMessage ?? "The phone number format is invalid.");
            }

            return ValidationResult.Success; // Validation passed
        }
    }
}
