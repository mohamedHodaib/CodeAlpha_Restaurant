using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs.StaffMember
{
    public class StaffMemberIncryptedInputDTO:StaffMemberOutputDTO
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }


        public StaffMemberIncryptedInputDTO()
        {
           UserName = string.Empty;
            PasswordHash = string.Empty;
        }


        public StaffMemberIncryptedInputDTO(string userName, string passwordHash, int id, enRole role, string firstName, string lastName
            , string? email, string? phone, bool isActive)
            : base(id, role, firstName, lastName, email, phone, isActive)
        {
            UserName = userName;
            PasswordHash = passwordHash;
        }

        
    }
}
