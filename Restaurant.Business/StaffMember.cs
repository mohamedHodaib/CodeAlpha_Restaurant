using Restaurant.DataAccess.DTOs;
using Restaurant.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.DataAccess.DTOs.StaffMember;
using System.Security.Cryptography;

namespace Restaurant.Business
{
    public class StaffMember


    {
        public enum enMode { AddNew, Update };
        public enMode Mode = enMode.AddNew;


        public StaffMemberOutputDTO staffMemberOuptutDTO => new StaffMemberOutputDTO(ID,Role
            ,FirstName,LastName,Email,Phone,IsActive);


        private StaffMemberIncryptedInputDTO staffMemberIncryptedInputDTO => new StaffMemberIncryptedInputDTO
            (UserName,ComputeHash(Password),ID, Role
            , FirstName, LastName, Email, Phone, IsActive);


        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public enRole Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }

        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? UpdatedAt { get; set; } = null;

        public StaffMember(StaffMemberInputDTO staffDTO, enMode mode = enMode.AddNew)
        {

            ID = staffDTO.ID;
            UserName = staffDTO.UserName;
            Password = staffDTO.Password;
            Role = staffDTO.Role;
            FirstName = staffDTO.FirstName;
            LastName = staffDTO.LastName;
            Email = staffDTO.Email;
            Phone = staffDTO.Phone;
            IsActive = staffDTO.IsActive;

            Mode = mode;
        }

        private static string ComputeHash(string input)
        {
            //SHA is Secutred Hash Algorithm.
            // Create an instance of the SHA-256 algorithm
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash value from the UTF-8 encoded input string
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));


                // Convert the byte array to a lowercase hexadecimal string
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        private async Task<BusinessResult<bool>> _AddNewStaffMember()
        {
            try
            {

                AddNewOutputDTO addNewOutputDTO = await StaffMemberDataAccess.AddNewStaffMember(staffMemberIncryptedInputDTO);

                ID = addNewOutputDTO.ID;
                CreatedAt = addNewOutputDTO.CreatedAt;
                UpdatedAt = addNewOutputDTO.CreatedAt;

                if (ID > 0)
                    return BusinessResult<bool>.SuccessResult("Staff Member Record  Created Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Staff Member Record  Creation Fail"
                        ,BusinessResult<bool>.enError.ServerError);
            }
            catch (SqlHelper.DataAccessException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<BusinessResult<PaginatedDTO<StaffMemberOutputDTO>>> GetAllStaffMembers(int pageNumber, int pageSize)
        {
            try
            {



                PaginatedDTO<StaffMemberOutputDTO> PaginatedStaffMembers =
                    await StaffMemberDataAccess.GetAllStaffMembers(pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<StaffMemberOutputDTO>>
                    .SuccessResult("StaffMember Records Returned Successfully.", PaginatedStaffMembers);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Staff Members.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Staff Members.", ex);
            }

        }




        public static async Task<BusinessResult<StaffMemberOutputDTO>> Find(int id)
        {
            try
            {
                StaffMemberOutputDTO staffDTO = await StaffMemberDataAccess.GetStaffMemberByID(id);

                if (staffDTO.ID != 0)
                    return BusinessResult<StaffMemberOutputDTO>.SuccessResult("Staff Member  Found successfully",
                        staffDTO);

                else
                    return BusinessResult<StaffMemberOutputDTO>.FailureResult($"Staff Member with ID {id} Not Found"
                        ,BusinessResult<StaffMemberOutputDTO>.enError.NotFound);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Staff Member By ID.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find StaffMember By ID.", ex);
            }

        }


        public static async Task<BusinessResult<PaginatedDTO<StaffMemberOutputDTO>>> GetStaffMembersByRole(enRole role,int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<StaffMemberOutputDTO> staffDTO
                    = await StaffMemberDataAccess.GetStaffMembersByRole(role,pageNumber,pageSize);

                    return BusinessResult<PaginatedDTO<StaffMemberOutputDTO>>
                    .SuccessResult($"Staff Member With Role {role} Retrieved.",staffDTO);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Staff Members By Role.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Staff Members By Role.", ex);
            }

        }



        public static async Task<BusinessResult<StaffMemberOutputDTO>> GetStaffMemberByEmail(string email)
        {
            try
            {
                StaffMemberOutputDTO staffDTO = await StaffMemberDataAccess.GetStaffMemberByEmail(email);

                if (staffDTO.ID != 0)
                    return BusinessResult<StaffMemberOutputDTO>.SuccessResult("Staff Member Found successfully",
                        staffDTO);

                else
                    return BusinessResult<StaffMemberOutputDTO>.FailureResult($"Staff Member with Email {email} Not Found"
                        ,BusinessResult<StaffMemberOutputDTO>.enError.NotFound);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Staff Member By Email.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Staff Member By Email.", ex);
            }
        }


        public static async Task<BusinessResult<StaffMemberOutputDTO>> GetStaffMemberByPhoneNumber(string phone)
        {
            try
            {
                StaffMemberOutputDTO staffDTO = await StaffMemberDataAccess.GetStaffMemberByPhone(phone);

                if (staffDTO.ID != 0)
                    return BusinessResult<StaffMemberOutputDTO>.SuccessResult("StaffMember Found successfully",
                        staffDTO);

                else
                    return BusinessResult<StaffMemberOutputDTO>.FailureResult($"StaffMember Not Found"
                        , BusinessResult<StaffMemberOutputDTO>.enError.NotFound);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Staff Member By Phone.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Staff Member By Phone.", ex);
            }
        }

        public static async Task<BusinessResult<PaginatedDTO<StaffMemberOutputDTO>>> GetStaffMemberByFirstName(string firstName, int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<StaffMemberOutputDTO> PaginatedStaffMembers =
                    await StaffMemberDataAccess.GetStaffMemberByFirstName(firstName, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<StaffMemberOutputDTO>>
                    .SuccessResult("StaffMember Records Returned Successfully.", PaginatedStaffMembers);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Staff Member By First Name.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Staff Member By First Name.", ex);
            }
        }


        public static async Task<BusinessResult<PaginatedDTO<StaffMemberOutputDTO>>> GetStaffMemberByLastName(string lastName, int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<StaffMemberOutputDTO> PaginatedStaffMembers =
                    await StaffMemberDataAccess.GetStaffMemberByLastName(lastName, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<StaffMemberOutputDTO>>
                    .SuccessResult("StaffMember Records Returned Successfully.", PaginatedStaffMembers);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Staff Member By Last Name.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Staff Member By Last Name.", ex);
            }
        }


        public static async Task<BusinessResult<PaginatedDTO<StaffMemberOutputDTO>>> GetStaffMemberByFirstNameAndLastName(string firstName, string lastName, int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<StaffMemberOutputDTO> PaginatedStaffMembers =
                    await StaffMemberDataAccess.GetStaffMemberByFirstNameAndLastName(firstName, lastName, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<StaffMemberOutputDTO>>
                    .SuccessResult("StaffMember Records Returned Successfully.", PaginatedStaffMembers);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Staff Member By First And Last Name.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred during Find Staff Member By First And Last Name.", ex);
            }
        }

        

        public static async Task<BusinessResult<bool>> Authenticate(string username, string password)
        {
            try
            {

                StaffMemberIncryptedInputDTO staffMemberIncrypted = await StaffMemberDataAccess.GetStaffMemberByUserName(username);

                if (staffMemberIncrypted != null)
                {
                    if (ComputeHash(password).Equals(staffMemberIncrypted.PasswordHash))
                    {
                        if(staffMemberIncrypted.IsActive)
                        return BusinessResult<bool>.SuccessResult("Login Successfully.");

                        else
                        return BusinessResult<bool>.FailureResult("This Staff Member Account is not Active.",
                            BusinessResult<bool>.enError.Unauthorized);
                    }

                    else
                        return BusinessResult<bool>.FailureResult("Incorrect Password",
                            BusinessResult<bool>.enError.Unauthorized);
                }

                else
                    return BusinessResult<bool>.FailureResult("UserName Is Not Exist"
                        , BusinessResult<bool>.enError.BadRequest);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Staff Member Authentication .", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Staff Member Authentication  .", ex);
            }
        }


        private async Task<BusinessResult<bool>> _UpdateStaffMember()
        {
            try
            {
                int rowsAffected = await StaffMemberDataAccess.UpdateStaffMember(staffMemberIncryptedInputDTO);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Staff Member Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Staff Member Record Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static async Task<BusinessResult<bool>> Activate(int id)
        {
            try
            {
                int rowsAffected = await StaffMemberDataAccess.UpdateStatus(id,true);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Staff Member Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Staff Member Record Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Staff Activate Member .", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Staff Activate Member .", ex);
            }
        }


        public static async Task<BusinessResult<bool>> DeActivate(int id)
        {
            try
            {
                int rowsAffected = await StaffMemberDataAccess.UpdateStatus(id, false);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Staff Member Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Staff Member Record Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Staff DeActivate Member .", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Staff DeActivate Member .", ex);
            }
        }


        public static async Task<BusinessResult<bool>> UpdatePassword(int id ,string password)
        {
            try
            {
                if(!await _IsStaffMemberIDExist(id))
                    return BusinessResult<bool>.FailureResult("Staff Member is not exist"
                        , BusinessResult<bool>.enError.BadRequest);

                int rowsAffected = await StaffMemberDataAccess.UpdatePasswordHash(id, ComputeHash(password));

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Staff Member Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Staff Member Record Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Staff  Member .", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Staff  Member .", ex);
            }
        }


        public static async Task<BusinessResult<bool>> Delete(int id)
        {
            try
            {

                int rowsAffected =
                    await StaffMemberDataAccess.DeleteStaffMember(id);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("StaffMember Deleted Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Delete StaffMember Ingrediant Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Deleting Staff Member.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Deleting Staff Member.", ex);
            }

        }



        public async Task<BusinessResult<bool>> Save()
        {
            try
            {

                if (Email != null && await StaffMemberDataAccess.IsEmailUsed(ID, Email))
                    return BusinessResult<bool>.FailureResult
                                ("Email Already Used ,please enter another one.",
                                BusinessResult<bool>.enError.Conflict);

                if (Phone != null && await StaffMemberDataAccess.IsPhoneUsed(ID, Phone))
                    return BusinessResult<bool>.FailureResult
                                ("Phone Already Used ,please enter another one.",
                                BusinessResult<bool>.enError.Conflict);

                if (await StaffMemberDataAccess.IsUserNameUsed(ID, UserName))
                    return BusinessResult<bool>.FailureResult
                                ("UserName Already Used ,please enter another one."
                                ,BusinessResult<bool>.enError.Conflict);

                switch (Mode)
                {
                    case enMode.AddNew:

                        var result = await _AddNewStaffMember();

                        if (result.Success)
                            Mode = enMode.Update;

                        return result;

                    case enMode.Update:

                        return await _UpdateStaffMember();

                }

                return BusinessResult<bool>.FailureResult("Add/Update Staff Member Operation Failed."
                    ,BusinessResult<bool>.enError.ServerError);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Add/Update Staff Member.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Add/Update Staff Member.", ex);
            }

        }



        private static async Task<bool> _IsStaffMemberIDExist(int staffID) =>
          await StaffMemberDataAccess.IsStaffMemberExitst(staffID);

    }
}
