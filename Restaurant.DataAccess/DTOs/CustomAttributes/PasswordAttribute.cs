using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs.CustomAttributes
{
    public class PasswordAttribute : ValidationAttribute
    {
        private readonly string _regexPattern ;

        // Example: Egyptian phone numbers often start with 010, 011, 012, 015 and are 11 digits
        // Or a more general international pattern
        public PasswordAttribute()
        {
            // Default pattern if none is provided. This is a very basic example.
            // For production, use a more robust regex or a dedicated library.
            // Example for Egyptian mobile numbers: ^01[0125][0-9]{8}$
            _regexPattern = @"^(?=.*[a-zA-Z].*[a-zA-Z])(?=.*\d.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':
                            ""\\|,.<>/?~`].*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>/?~`])[A-Za-z0-9!@#
                            $%^&*()_+\-=\[\]{};':""\\|,.<>/?~`]{6,}$";
            // Basic: starts with optional +, then 7-20 digits/spaces/hyphens/parentheses
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
           

            var password = value.ToString();

            if (password == "")
            {
                // Treat empty or whitespace-only as invalid if it's required (or not allowed for optional)
                // If [Required] is also on the property, it handles empty/null.
                return new ValidationResult(ErrorMessage ?? "Password cannot be empty or whitespace.");
            }

            // Perform regex validation
            if (!Regex.IsMatch(password, _regexPattern))
            {
                // Use the ErrorMessage property defined on the attribute, or a default.
                return new ValidationResult(ErrorMessage ?? "The Password must" +
                    " contain at least two letter and at least two digit and at least two SpecialCharacter.");
            }

            return ValidationResult.Success; // Validation passed
        }
    }
}