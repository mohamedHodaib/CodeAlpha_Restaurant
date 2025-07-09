using Restaurant.DataAccess.DTOs.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Restaurant.DataAccess.DTOs.StaffMember
{
    public class StaffMemberInputDTO : StaffMemberOutputDTO
    {
        [Required(ErrorMessage = "User Name is required.")]
        [StringLength(255, MinimumLength = 5
            , ErrorMessage = "User Name Must Not be At Least 5 Characters and cannot exceed 255.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Password(ErrorMessage = "Password Must Not be At Least 6 Characters and cannot exceed 255.")]
        public string Password { get; set; }

        public StaffMemberInputDTO()
        {
            UserName = "";
            Password = "";
        }


        public StaffMemberInputDTO(string userName, string password, int id, enRole role, string firstName, string lastName
            , string? email, string? phone, bool isActive)
            : base(id,role,firstName,lastName,email,phone,isActive)
        {
            UserName = userName;
            Password = password;
        }
    }
}
