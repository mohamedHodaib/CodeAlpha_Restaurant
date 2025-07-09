using Restaurant.DataAccess.DTOs.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs
{
    public class CustomerDTO
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(100, MinimumLength = 2
            , ErrorMessage = "First Name Must Not be Empty and cannot exceed 100.")]
        public string FirstName {  get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(100, MinimumLength = 2
            , ErrorMessage = "Last Name Must Not be Empty and cannot exceed 100.")]
        public string LastName {  get; set; }

        [EmailAddress( ErrorMessage = "Incorrect Email.")]
        public string? Email { get; set; }

        [CustomPhone(ErrorMessage = "Incorrect Phone Number")]
        public string? Phone { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Loyality Points must be non-negative.")]
        public int? LoyalityPoints { get; set; }

        public CustomerDTO() 
        { 
            ID = 0;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = null;
            Phone = null;
            LoyalityPoints = null;
        }

        
        public CustomerDTO(int id,string firstName,string lastName
            ,string? email,string? phone,int? loyalitPoints)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            LoyalityPoints = loyalitPoints;
        }

    }
}

