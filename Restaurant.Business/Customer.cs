using Restaurant.DataAccess.DTOs;
using Restaurant.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Restaurant.Business
{
    public class Customer

    {
        public enum enMode { AddNew, Update };
        public enMode Mode = enMode.AddNew;


        public CustomerDTO customerDTO => new CustomerDTO(ID,FirstName,LastName
            ,Email,Phone ,LoyalityPoints);



        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? LoyalityPoints { get; set; }

        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? UpdatedAt { get; set; } = null;

        public Customer(CustomerDTO customerDTO, enMode mode = enMode.AddNew)
        {

            ID = customerDTO.ID;
            FirstName = customerDTO.FirstName;
            LastName = customerDTO.LastName;
            Email = customerDTO.Email;
            Phone = customerDTO.Phone;
            if (mode == enMode.Update)
                LoyalityPoints = customerDTO.LoyalityPoints;

            else
                LoyalityPoints = 0;

                Mode = mode;
        }


        private async Task<BusinessResult<bool>> _AddNewMenuCustomer()
        {
            try
            {

                AddNewOutputDTO addNewOutputDTO = await CustomerDataAccess.AddNewCustomer(customerDTO);

                ID = addNewOutputDTO.ID;
                CreatedAt = addNewOutputDTO.CreatedAt;
                UpdatedAt = addNewOutputDTO.CreatedAt;

                if (ID > 0)
                    return BusinessResult<bool>.SuccessResult("Customer Record  Created Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Customer Record  Creation Fail"
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

        public static async Task<BusinessResult<PaginatedDTO<CustomerDTO>>> GetAllCustomers(int pageNumber, int pageSize)
        {
            try
            {



                PaginatedDTO<CustomerDTO> PaginatedCustomers =
                    await CustomerDataAccess.GetAllCustomers(pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<CustomerDTO>>
                    .SuccessResult("Customer Records Returned Successfully.", PaginatedCustomers);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Customers.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Customers.", ex);
            }

        }


        public static async Task<BusinessResult<Customer>> Find(int id)
        {
            try
            {
                CustomerDTO customerDTO = await CustomerDataAccess.GetCustomerByID(id);

                if (customerDTO.ID != 0)
                    return BusinessResult<Customer>.SuccessResult("Customer  Found successfully",
                        new Customer(customerDTO, enMode.Update));

                else
                    return BusinessResult<Customer>.FailureResult($"Customer with ID {id} Not Found"
                        ,BusinessResult<Customer>.enError.NotFound);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Customer By ID.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Customer By ID.", ex);
            }

        }


        public static async Task<BusinessResult<Customer>> GetCustomerByEmail(string email)
        {
            try
            {
                CustomerDTO customerDTO = await CustomerDataAccess.GetCustomerByEmail(email);

                if (customerDTO.ID != 0)
                    return BusinessResult<Customer>.SuccessResult("Customer Found successfully",
                        new Customer(customerDTO, enMode.Update));

                else
                    return BusinessResult<Customer>.FailureResult($"Customer with Email {email} Not Found"
                        , BusinessResult<Customer>.enError.NotFound);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Customer By Email.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Customer By Email.", ex);
            }
        }


        public static async Task<BusinessResult<Customer>> GetCustomerByPhoneNumber(string phone)
        {
            try
            {
                CustomerDTO customerDTO = await CustomerDataAccess.GetCustomerByPhoneNumber(phone);

                if (customerDTO.ID != 0)
                    return BusinessResult<Customer>.SuccessResult("Customer Found successfully",
                        new Customer(customerDTO, enMode.Update));

                else
                    return BusinessResult<Customer>.FailureResult($"Customer Not Found"
                        , BusinessResult<Customer>.enError.NotFound);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Customer By Phone.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Customer By Phone.", ex);
            }
        }

        public static async Task<BusinessResult<PaginatedDTO<CustomerDTO>>> GetCustomersByFirstName(string firstName, int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<CustomerDTO> PaginatedCustomers =
                    await CustomerDataAccess.GetCustomersByFirstName(firstName, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<CustomerDTO>>
                    .SuccessResult("Customer Records Returned Successfully.", PaginatedCustomers);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Customer By First Name.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Customer By First Name.", ex);
            }
        }


        public static async Task<BusinessResult<PaginatedDTO<CustomerDTO>>> GetCustomersByLastName(string lastName, int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<CustomerDTO> PaginatedCustomers =
                    await CustomerDataAccess.GetCustomersByLastName(lastName, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<CustomerDTO>>
                    .SuccessResult("Customer Records Returned Successfully.", PaginatedCustomers);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Customer By Last Name.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Customer By Last Name.", ex);
            }
        }


        public static async Task<BusinessResult<PaginatedDTO<CustomerDTO>>> GetCustomersByFirstNameAndLastName(string firstName, string lastName, int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<CustomerDTO> PaginatedCustomers =
                    await CustomerDataAccess.GetCustomersByFirstNameAndLastName(firstName,lastName, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<CustomerDTO>>
                    .SuccessResult("Customer Records Returned Successfully.", PaginatedCustomers);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Customer By First And Last Name.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Customer By First And Last Name.", ex);
            }
        }


        public static async Task<BusinessResult<PaginatedDTO<CustomerDTO>>> GetCustomersByLoyaltyPoints(int loyaltyPoints, int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<CustomerDTO> PaginatedCustomers =
                    await CustomerDataAccess.GetCustomersByLoyaltyPoints(loyaltyPoints, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<CustomerDTO>>
                    .SuccessResult("Customer Records Returned Successfully.", PaginatedCustomers);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Customer By Loylty Points.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Customer By Loylty Points.", ex);
            }
        }
        


        private async Task<BusinessResult<bool>> _UpdateCustomer()
        {
            try
            {
                int rowsAffected = await CustomerDataAccess.UpdateCustomer(customerDTO);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Customer Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Customer Record Fail."
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


        private async Task<BusinessResult<bool>> UpdateCustomerLoyaltyPoints(int id ,int loyaltyPoints)
        {
            try
            {
                if(!await IsCustomerIDExist(id))
                    return BusinessResult<bool>.FailureResult("Customer with id Not Exist,Update Fail."
                        ,BusinessResult<bool>.enError.BadRequest);

                int rowsAffected = await CustomerDataAccess.UpdateCustomerLoyalityPoints(id,loyaltyPoints);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Customer Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Customer Record Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Customer Loylty Points.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Customer Loylty Points.", ex);
            }
        }


        public async Task<BusinessResult<bool>> Delete()
        {
            try
            {

                int rowsAffected =
                    await CustomerDataAccess.DeleteCustomer(ID);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Customer Deleted Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Delete Customer Ingrediant Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Deleting Customer.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Deleting Customer.", ex);
            }

        }

        public async Task<BusinessResult<bool>> Save()
        {
            try
            {
                
                if (Email != null && await CustomerDataAccess.IsEmailUsed(ID,Email))
                    return BusinessResult<bool>.FailureResult
                                ("Email Already Used ,please enter another one."
                                , BusinessResult<bool>.enError.Conflict);

                if (Phone != null && await CustomerDataAccess.IsPhoneUsed(ID,Phone))
                    return BusinessResult<bool>.FailureResult
                                ("Phone Already Used ,please enter another one."
                                , BusinessResult<bool>.enError.Conflict);

                switch (Mode)
                {
                    case enMode.AddNew:

                        var result = await _AddNewMenuCustomer();

                        if (result.Success)
                            Mode = enMode.Update;

                        return result;

                    case enMode.Update:

                        return await _UpdateCustomer();

                }

                return BusinessResult<bool>.FailureResult("Add/Update Customer Operation Failed.");
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Add/Update Customer.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Add/Update Customer.", ex);
            }

        }



        public static async Task<bool> IsCustomerIDExist(int customerID) =>
          await CustomerDataAccess.IsCustomerExist(customerID);

    }
}
