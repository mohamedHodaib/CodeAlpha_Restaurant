using Restaurant.DataAccess.DTOs.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs.StaffMember
{
    public enum enRole { Admin = 1,KitchenStaff,Waiter}
    public class StaffMemberOutputDTO
    {
        public int ID { get; set; }
        public enRole Role { get; set; }
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(100, MinimumLength = 2
            , ErrorMessage = "First Name Must Not be Empty and cannot exceed 100.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(100, MinimumLength = 2
            , ErrorMessage = "Last Name Must Not be Empty and cannot exceed 100.")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Incorrect Email.")]
        public string? Email { get; set; }

        [CustomPhone(ErrorMessage = "Incorrect Phone Number")]
        public string? Phone { get; set; }
        public bool IsActive { get; set; }

        public StaffMemberOutputDTO()
        {
            ID = 0;
            Role = enRole.KitchenStaff;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = null;
            Phone = null;
            IsActive = true;
        }


        public StaffMemberOutputDTO(int id ,enRole role, string firstName, string lastName
            , string? email, string? phone, bool isActive)
        {
            ID = id;
            Role = role;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            IsActive = isActive;
        }

    }
}
