using Microsoft.Data.SqlClient;
using Restaurant.DataAccess.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess
{
    public class CustomerDataAccess
    {
        #region Public Methods


        public static async Task<AddNewOutputDTO> AddNewCustomer(CustomerDTO customerDTO)
        {

            try
            {

                var parameters = new[]
                {
                    new SqlParameter("@customer_id", SqlDbType.Int) { Direction = ParameterDirection.Output },
                    new SqlParameter("@CurrentTime", SqlDbType.DateTime) { Direction = ParameterDirection.Output },
                    new SqlParameter("@first_name", SqlDbType.NVarChar) { Value = customerDTO.FirstName },
                    new SqlParameter("@last_name", SqlDbType.NVarChar) { Value = customerDTO.LastName },
                    new SqlParameter("@email", SqlDbType.NVarChar)
                    { Value = customerDTO.Email == null ? DBNull.Value : customerDTO.Email },
                    new SqlParameter("@phone_number", SqlDbType.NVarChar) { Value = customerDTO.Phone }
                };

                var outputValues = await SqlHelper.ExecuteWithOutputParametersAsync("SP_AddNewCustomer", parameters);

                AddNewOutputDTO addNewCustomerOutputDTO = new AddNewOutputDTO();

                if (outputValues.TryGetValue("customer_id", out object ID) && ID != null)
                    addNewCustomerOutputDTO.ID = (int)ID;
                else
                    throw new SqlHelper.DataAccessException("Failed to retrieve new Customer ID from stored procedure.");



                if (outputValues.TryGetValue("CurrentTime", out object createdAtObj))
                    addNewCustomerOutputDTO.CreatedAt = Convert.ToDateTime(createdAtObj);

                else
                    throw new SqlHelper.DataAccessException("Failed to retrieve CreatedAt timestamp for new Customer.");

               


                return addNewCustomerOutputDTO;
            }
            catch
            {
                throw;
            }
        }



        public static async Task<PaginatedDTO<CustomerDTO>> GetAllCustomers(int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.NVarChar) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Decimal) { Value = pageSize };
                SqlParameter TotalCustomersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<CustomerDTO> paginatedCustomersDTO = new PaginatedDTO<CustomerDTO>();

                paginatedCustomersDTO.PageSize = pageSize;
                paginatedCustomersDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedCustomers", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedCustomersDTO.Items.Add(MapReaderToCustomerDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalCustomersParam);

                paginatedCustomersDTO.TotalItems =
                       TotalCustomersParam.Value == DBNull.Value ? 0 : (int)TotalCustomersParam.Value;

                return paginatedCustomersDTO;
            }
            catch
            {
                throw;
            }
        }





        public static async Task<CustomerDTO> GetCustomerByID(int id)
        {
            try
            {
                var param = new SqlParameter("@customer_id", SqlDbType.Int) { Value = id };

                CustomerDTO customerDTO = new CustomerDTO();
                await SqlHelper.ExecuteReaderAsync("SP_GetCustomerByID", reader =>
                {
                    if (reader.Read())
                    {
                        customerDTO = MapReaderToCustomerDTO(reader, id);
                    }
                }, param);

                return customerDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<CustomerDTO> GetCustomerByEmail(string email)
        {
            try
            {
                var param = new SqlParameter("@email", SqlDbType.NVarChar) { Value = email };

                CustomerDTO customerDTO = new CustomerDTO();
                await SqlHelper.ExecuteReaderAsync("SP_GetCustomerByEmail", reader =>
                {
                    if (reader.Read())
                    {
                        customerDTO = MapReaderToCustomerDTO(reader);
                    }
                }, param);

                return customerDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<CustomerDTO> GetCustomerByPhoneNumber(string phone)
        {
            try
            {
                var param = new SqlParameter("@phone_number", SqlDbType.NVarChar) { Value = phone };

                CustomerDTO customerDTO = new CustomerDTO();
                await SqlHelper.ExecuteReaderAsync("SP_GetCustomerByPhoneNumber", reader =>
                {
                    if (reader.Read())
                    {
                        customerDTO = MapReaderToCustomerDTO(reader);
                    }
                }, param);

                return customerDTO;
            }
            catch
            {
                throw;
            }
        }

        public static async Task<PaginatedDTO<CustomerDTO>> GetCustomersByFirstName(string firstName,int pageNumber,int pageSize)
        {
            try
            {
                var fNameParam = new SqlParameter("@first_name", SqlDbType.NVarChar) { Value = firstName };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.NVarChar) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Decimal) { Value = pageSize };
                SqlParameter TotalCustomersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<CustomerDTO> paginatedCustomersDTO = new PaginatedDTO<CustomerDTO>();

                paginatedCustomersDTO.PageSize = pageSize;
                paginatedCustomersDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedCustomersByFirstName", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedCustomersDTO.Items.Add(MapReaderToCustomerDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalCustomersParam,fNameParam);

                paginatedCustomersDTO.TotalItems =
                       TotalCustomersParam.Value == DBNull.Value ? 0 : (int)TotalCustomersParam.Value;

                return paginatedCustomersDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<CustomerDTO>> GetCustomersByLastName(string lastName, int pageNumber, int pageSize)
        {
            try
            {
                var lNameParam = new SqlParameter("@last_name", SqlDbType.NVarChar) { Value = lastName };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.NVarChar) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Decimal) { Value = pageSize };
                SqlParameter TotalCustomersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<CustomerDTO> paginatedCustomersDTO = new PaginatedDTO<CustomerDTO>();

                paginatedCustomersDTO.PageSize = pageSize;
                paginatedCustomersDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedCustomersByLastName", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedCustomersDTO.Items.Add(MapReaderToCustomerDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalCustomersParam, lNameParam);

                paginatedCustomersDTO.TotalItems =
                       TotalCustomersParam.Value == DBNull.Value ? 0 : (int)TotalCustomersParam.Value;

                return paginatedCustomersDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<CustomerDTO>> GetCustomersByFirstNameAndLastName(string firstName,string lastName, int pageNumber, int pageSize)
        {
            try
            {
                var fNameParam = new SqlParameter("@first_name", SqlDbType.NVarChar) { Value = firstName };
                var lNameParam = new SqlParameter("@last_name", SqlDbType.NVarChar) { Value = lastName };

                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.NVarChar) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Decimal) { Value = pageSize };
                SqlParameter TotalCustomersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<CustomerDTO> paginatedCustomersDTO = new PaginatedDTO<CustomerDTO>();

                paginatedCustomersDTO.PageSize = pageSize;
                paginatedCustomersDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedCustomersByFirstNameAndLastName", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedCustomersDTO.Items.Add(MapReaderToCustomerDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalCustomersParam, fNameParam,lNameParam);

                paginatedCustomersDTO.TotalItems =
                       TotalCustomersParam.Value == DBNull.Value ? 0 : (int)TotalCustomersParam.Value;

                return paginatedCustomersDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<CustomerDTO>> GetCustomersByLoyaltyPoints(int loyaltyPoints, int pageNumber, int pageSize)
        {
            try
            {
                var loyaltyPointsParam = new SqlParameter("@loyalty_points", SqlDbType.NVarChar) { Value = loyaltyPoints };

                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.NVarChar) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Decimal) { Value = pageSize };
                SqlParameter TotalCustomersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<CustomerDTO> paginatedCustomersDTO = new PaginatedDTO<CustomerDTO>();

                paginatedCustomersDTO.PageSize = pageSize;
                paginatedCustomersDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetCustomerByLoyaltyPoints", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedCustomersDTO.Items.Add(MapReaderToCustomerDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalCustomersParam, loyaltyPointsParam);

                paginatedCustomersDTO.TotalItems =
                       TotalCustomersParam.Value == DBNull.Value ? 0 : (int)TotalCustomersParam.Value;

                return paginatedCustomersDTO;
            }
            catch
            {
                throw;
            }
        }


        

        public static async Task<int> UpdateCustomer( CustomerDTO customerDTO)
        {

            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@customer_id", SqlDbType.Int) { Value = customerDTO.ID },
                    new SqlParameter("@first_name", SqlDbType.NVarChar)
                    { Value = customerDTO.FirstName },
                    new SqlParameter("@last_name", SqlDbType.NVarChar)
                    { Value = customerDTO.LastName },
                    new SqlParameter("@email", SqlDbType.NVarChar)
                    { Value = customerDTO.Email },
                    new SqlParameter("@phone_number", SqlDbType.NVarChar)
                    { Value = customerDTO.Phone }

                };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateCustomer", parameters);

            }
            catch
            {
                throw;
            }
        }

        public static async Task<bool> IsEmailUsed(int id,string email)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@customer_id", SqlDbType.Int) { Value = id };
                SqlParameter emailParam = new SqlParameter("@email", SqlDbType.NVarChar) { Value = email };
                SqlParameter isUsedParam = new SqlParameter("@IsUsed", SqlDbType.Bit) { Direction = ParameterDirection.Output };


                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsCustomerEmailUsed",idParam, emailParam,isUsedParam);

            }
            catch
            {
                throw;
            }
        }


        public static async Task<bool> IsPhoneUsed(int id, string phone)
        {

            try
            {
                SqlParameter idParam = new SqlParameter("@customer_id", SqlDbType.Int) { Value = id };
                SqlParameter phoneParam = new SqlParameter("@phone_number", SqlDbType.NVarChar) { Value = phone };
                SqlParameter isUsedParam = new SqlParameter("@IsUsed", SqlDbType.Bit) { Direction = ParameterDirection.Output };


                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsCustomerPhoneNumberUsed"
                    ,idParam, phoneParam, isUsedParam);

            }
            catch
            {
                throw;
            }
        }


        public static async Task<bool> IsCustomerExist(int id)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@customer_id", SqlDbType.Int) { Value = id };
                SqlParameter isExistParam = new SqlParameter("@IsExist", SqlDbType.Bit) { Direction = ParameterDirection.Output };


                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsCustomerExist", idParam,isExistParam);

            }
            catch
            {
                throw;
            }
        }




        public static async Task<int> UpdateCustomerLoyalityPoints(int id, int loyaltyPoints)
        {

            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@customer_id", SqlDbType.Int) { Value = id },
                    new SqlParameter("@loyalty_points", SqlDbType.Int) { Value = loyaltyPoints }

                };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateCustomerLoyalityPoints", parameters);

            }
            catch
            {
                throw;
            }
        }

        public static async Task<int> DeleteCustomer(int id)
        {

            try
            {

                var customerIDParam = new SqlParameter("@customer_id", SqlDbType.Int) { Value = id };

                return await SqlHelper.ExecuteNonQueryAsync("SP_DeleteCustomer", customerIDParam);

            }
            catch
            {
                throw;
            }
        }


        #endregion

        #region Private Helper Methods



        private static CustomerDTO MapReaderToCustomerDTO(IDataReader reader, int? id = null)
        {
            int emailOrdinal = reader.GetOrdinal("email");
            int phoneNumber = reader.GetOrdinal("phone_number");
            int loyaltyPoints = reader.GetOrdinal("loyalty_points");


            return new CustomerDTO(
                    id ?? reader.GetInt32(reader.GetOrdinal("customer_id")),
                    reader.GetString(reader.GetOrdinal("first_name")),
                    reader.GetString(reader.GetOrdinal("last_name")),
                    reader.IsDBNull(emailOrdinal) ? null : reader.GetString(emailOrdinal),
                    reader.IsDBNull(phoneNumber) ? null : reader.GetString(phoneNumber),
                    reader.IsDBNull(loyaltyPoints) ? null : reader.GetInt32(loyaltyPoints)
            );
        }

        #endregion
    }
}
