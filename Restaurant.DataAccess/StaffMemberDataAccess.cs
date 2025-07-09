using Microsoft.Data.SqlClient;
using Restaurant.DataAccess.DTOs;
using Restaurant.DataAccess.DTOs.StaffMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Restaurant.DataAccess.DTOs.StaffMember.StaffMemberOutputDTO;

namespace Restaurant.DataAccess
{
    public class StaffMemberDataAccess
    {
        #region Public Methods


        public static async Task<AddNewOutputDTO> AddNewStaffMember(StaffMemberIncryptedInputDTO staffMemberDTO)
        {

            try
            {

                var parameters = new[]
                {
                    new SqlParameter("@staff_id", SqlDbType.Int) { Direction = ParameterDirection.Output },
                    new SqlParameter("@CurrentTime", SqlDbType.DateTime) { Direction = ParameterDirection.Output },

                    new SqlParameter("@username", SqlDbType.NVarChar) { Value = staffMemberDTO.UserName },
                    new SqlParameter("@password", SqlDbType.NVarChar) { Value = staffMemberDTO.PasswordHash },
                    new SqlParameter("@role", SqlDbType.TinyInt) { Value = (byte)staffMemberDTO.Role },
                    new SqlParameter("@first_name", SqlDbType.NVarChar) { Value = staffMemberDTO.FirstName },
                    new SqlParameter("@last_name", SqlDbType.NVarChar) { Value = staffMemberDTO.LastName },
                    new SqlParameter("@email", SqlDbType.NVarChar)
                    { Value = staffMemberDTO.Email == null ? DBNull.Value : staffMemberDTO.Email },
                    new SqlParameter("@phone_number", SqlDbType.NVarChar)
                    { Value = staffMemberDTO.Phone == null ? DBNull.Value : staffMemberDTO.Phone }
                };

                var outputValues = await SqlHelper.ExecuteWithOutputParametersAsync("SP_AddNewStaffMember", parameters);

                AddNewOutputDTO addNewStaffMemberOutputDTO = new AddNewOutputDTO();

                if (outputValues.TryGetValue("staff_id", out object ID) && ID != null)
                    addNewStaffMemberOutputDTO.ID = (int)ID;
                else
                    throw new SqlHelper.DataAccessException("Failed to retrieve new StaffMember ID from stored procedure.");



                if (outputValues.TryGetValue("CurrentTime", out object createdAtObj))
                    addNewStaffMemberOutputDTO.CreatedAt = Convert.ToDateTime(createdAtObj);

                else
                    throw new SqlHelper.DataAccessException("Failed to retrieve CreatedAt timestamp for new StaffMember.");



            


                return addNewStaffMemberOutputDTO;
            }
            catch
            {
                throw;
            }
        }



        public static async Task<PaginatedDTO<StaffMemberOutputDTO>> GetAllStaffMembers(int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.NVarChar) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Decimal) { Value = pageSize };
                SqlParameter TotalStaffMembersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<StaffMemberOutputDTO> paginatedStaffMembersDTO = new PaginatedDTO<StaffMemberOutputDTO>();

                paginatedStaffMembersDTO.PageSize = pageSize;
                paginatedStaffMembersDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedAllStaff", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedStaffMembersDTO.Items.Add(MapReaderToStaffMemberDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalStaffMembersParam);

                paginatedStaffMembersDTO.TotalItems =
                       TotalStaffMembersParam.Value == DBNull.Value ? 0 : (int)TotalStaffMembersParam.Value;

                return paginatedStaffMembersDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<StaffMemberOutputDTO>> GetStaffMembersByRole(enRole role, int pageNumber, int pageSize)
        {
            try
            { 
                SqlParameter roleParam = new SqlParameter("@role", SqlDbType.TinyInt) { Value = (byte)role };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalStaffMembersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<StaffMemberOutputDTO> paginatedStaffMembersDTO = new PaginatedDTO<StaffMemberOutputDTO>();

                paginatedStaffMembersDTO.PageSize = pageSize;
                paginatedStaffMembersDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedStaffMembersByRole", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedStaffMembersDTO.Items.Add(MapReaderToStaffMemberDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalStaffMembersParam, roleParam);

                paginatedStaffMembersDTO.TotalItems =
                       TotalStaffMembersParam.Value == DBNull.Value ? 0 : (int)TotalStaffMembersParam.Value;

                return paginatedStaffMembersDTO;
            }
            catch
            {
                throw;
            }
        }

        public static async Task<PaginatedDTO<StaffMemberOutputDTO>> GetStaffMembersByStatus(bool status, int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter statusParam = new SqlParameter("@status", SqlDbType.Bit) { Value = status };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalStaffMembersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<StaffMemberOutputDTO> paginatedStaffMembersDTO = new PaginatedDTO<StaffMemberOutputDTO>();

                paginatedStaffMembersDTO.PageSize = pageSize;
                paginatedStaffMembersDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedStaffMembersByStatus", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedStaffMembersDTO.Items.Add(MapReaderToStaffMemberDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalStaffMembersParam, statusParam);

                paginatedStaffMembersDTO.TotalItems =
                       TotalStaffMembersParam.Value == DBNull.Value ? 0 : (int)TotalStaffMembersParam.Value;

                return paginatedStaffMembersDTO;
            }
            catch
            {
                throw;
            }
        }


        


        public static async Task<StaffMemberOutputDTO> GetStaffMemberByID(int id)
        {
            try
            {
                var param = new SqlParameter("@staff_id", SqlDbType.Int) { Value = id };

                StaffMemberOutputDTO staffMemberDTO = new StaffMemberOutputDTO();
                await SqlHelper.ExecuteReaderAsync("SP_GetStaffMemberByID", reader =>
                {
                    if (reader.Read())
                    {
                        staffMemberDTO = MapReaderToStaffMemberDTO(reader, id);
                    }
                }, param);

                return staffMemberDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<StaffMemberIncryptedInputDTO> GetStaffMemberByUserName(string userName)
        {
            try
            {
                var userNameParam = new SqlParameter("@username", SqlDbType.NVarChar) { Value = userName };

                StaffMemberIncryptedInputDTO staffMemberDTO = new StaffMemberIncryptedInputDTO();
                await SqlHelper.ExecuteReaderAsync("SP_GetStaffMemberByUserName", reader =>
                {
                    if (reader.Read())
                    {
                        staffMemberDTO = MapReaderToStaffMemberIncryptedDTO(reader);
                    }
                }, userNameParam);

                return staffMemberDTO;
            }
            catch
            {
                throw;
            }
        }
        

        public static async Task<PaginatedDTO<StaffMemberOutputDTO>> GetStaffMemberByFirstName(string firstName, int pageNumber, int pageSize)
        {
            try
            {
                var fNameParam = new SqlParameter("@first_name", SqlDbType.NVarChar) { Value = firstName };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalStaffMembersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<StaffMemberOutputDTO> paginatedStaffMembersDTO = new PaginatedDTO<StaffMemberOutputDTO>();

                paginatedStaffMembersDTO.PageSize = pageSize;
                paginatedStaffMembersDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedStaffMembersByFirstName", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedStaffMembersDTO.Items.Add(MapReaderToStaffMemberDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalStaffMembersParam, fNameParam);

                paginatedStaffMembersDTO.TotalItems =
                       TotalStaffMembersParam.Value == DBNull.Value ? 0 : (int)TotalStaffMembersParam.Value;

                return paginatedStaffMembersDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<StaffMemberOutputDTO>> GetStaffMemberByLastName(string lastName, int pageNumber, int pageSize)
        {
            try
            {
                var lNameParam = new SqlParameter("@last_name", SqlDbType.NVarChar) { Value = lastName };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalStaffMembersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<StaffMemberOutputDTO> paginatedStaffMembersDTO = new PaginatedDTO<StaffMemberOutputDTO>();

                paginatedStaffMembersDTO.PageSize = pageSize;
                paginatedStaffMembersDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedStaffMembersByLastName", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedStaffMembersDTO.Items.Add(MapReaderToStaffMemberDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalStaffMembersParam, lNameParam);

                paginatedStaffMembersDTO.TotalItems =
                       TotalStaffMembersParam.Value == DBNull.Value ? 0 : (int)TotalStaffMembersParam.Value;

                return paginatedStaffMembersDTO;
            }
            catch
            {
                throw;
            }
        }



        public static async Task<PaginatedDTO<StaffMemberOutputDTO>> GetStaffMemberByFirstNameAndLastName(string firstName,string lastName, int pageNumber, int pageSize)
        {
            try
            {
                var fNameParam = new SqlParameter("@first_name", SqlDbType.NVarChar) { Value = firstName };
                var lNameParam = new SqlParameter("@last_name", SqlDbType.NVarChar) { Value = lastName };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalStaffMembersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<StaffMemberOutputDTO> paginatedStaffMembersDTO = new PaginatedDTO<StaffMemberOutputDTO>();

                paginatedStaffMembersDTO.PageSize = pageSize;
                paginatedStaffMembersDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedStaffMembersByFirstNameAndLastName", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedStaffMembersDTO.Items.Add(MapReaderToStaffMemberDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalStaffMembersParam, fNameParam,lNameParam);

                paginatedStaffMembersDTO.TotalItems =
                       TotalStaffMembersParam.Value == DBNull.Value ? 0 : (int)TotalStaffMembersParam.Value;

                return paginatedStaffMembersDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<StaffMemberOutputDTO> GetStaffMemberByEmail(string email)
        {
            try
            {
                var param = new SqlParameter("@email", SqlDbType.NVarChar) { Value = email };

                StaffMemberOutputDTO staffMemberDTO = new StaffMemberOutputDTO();
                await SqlHelper.ExecuteReaderAsync("SP_GetStaffMemberByEmail", reader =>
                {
                    if (reader.Read())
                    {
                        staffMemberDTO = MapReaderToStaffMemberDTO(reader);
                    }
                }, param);

                return staffMemberDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<StaffMemberOutputDTO> GetStaffMemberByPhone(string Phone)
        {
            try
            {
                var param = new SqlParameter("@phone_number", SqlDbType.NVarChar) { Value = Phone };

                StaffMemberOutputDTO staffMemberDTO = new StaffMemberOutputDTO();
                await SqlHelper.ExecuteReaderAsync("SP_GetStaffMemberByPhoneNumber", reader =>
                {
                    if (reader.Read())
                    {
                        staffMemberDTO = MapReaderToStaffMemberDTO(reader);
                    }
                }, param);

                return staffMemberDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> UpdateStaffMember(StaffMemberIncryptedInputDTO staffMemberDTO)
        {

            try
            {
                var parameters = new[]
                {
                   new SqlParameter("@staff_id", SqlDbType.Int) { Value = staffMemberDTO.ID },
                    new SqlParameter("@role", SqlDbType.TinyInt) { Value = (byte)staffMemberDTO.Role },
                    new SqlParameter("@first_name", SqlDbType.NVarChar) { Value = staffMemberDTO.FirstName },
                    new SqlParameter("@last_name", SqlDbType.NVarChar) { Value = staffMemberDTO.LastName },
                    new SqlParameter("@email", SqlDbType.NVarChar)
                    { Value = staffMemberDTO.Email == null ? DBNull.Value : staffMemberDTO.Email },
                    new SqlParameter("@phone_number", SqlDbType.NVarChar)
                    { Value = staffMemberDTO.Phone == null ? DBNull.Value : staffMemberDTO.Phone }

                };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateStaffMember", parameters);

            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> UpdateStatus(int id, bool status)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@staff_id", SqlDbType.Int) { Value = id };
                SqlParameter statusParam = new SqlParameter("@status", SqlDbType.Bit) { Value = status };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateStaffMemberStatus", idParam, statusParam);
            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> UpdatePasswordHash(int id, string PasswordHash)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@staff_id", SqlDbType.Int) { Value = id };
                SqlParameter passwordHashParam = new SqlParameter("@password_hash", SqlDbType.NVarChar) { Value = PasswordHash };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdatePasswordHash", idParam, passwordHashParam);
            }
            catch
            {
                throw;
            }
        }

        public static async Task<bool> IsUserNameUsed(int id, string username)
        {

            try
            {
                SqlParameter idParam = new SqlParameter("@staff_id", SqlDbType.Int) { Value = id };
                SqlParameter usernameParam = new SqlParameter("@username", SqlDbType.NVarChar) { Value = username };
                SqlParameter isUsedParam = new SqlParameter("@IsUsed", SqlDbType.Bit) { Direction = ParameterDirection.Output };


                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsUserNameUsed", idParam, usernameParam,isUsedParam);

            }
            catch
            {
                throw;
            }
        }

        public static async Task<bool> IsStaffMemberExitst(int id)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@staff_id", SqlDbType.NVarChar) { Value = id };
                SqlParameter isExistParam = new SqlParameter("@IsExist", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsStaffMemberExist", idParam,isExistParam);

            }
            catch
            {
                throw;
            }
        }



        public static async Task<bool> IsEmailUsed(int id, string email)
        {

            try
            {
                SqlParameter idParam = new SqlParameter("@staff_id", SqlDbType.Int) { Value = id };
                SqlParameter staffMemberEmailParam = new SqlParameter("@email", SqlDbType.NVarChar) { Value = email };
                SqlParameter isUsedParam = new SqlParameter("@IsUsed", SqlDbType.Bit) { Direction = ParameterDirection.Output };


                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsStaffEmailUsed", idParam, staffMemberEmailParam,isUsedParam);

            }
            catch
            {
                throw;
            }
        }


        public static async Task<bool> IsPhoneUsed(int id, string email)
        {

            try
            {
                SqlParameter idParam = new SqlParameter("@staff_id", SqlDbType.Int) { Value = id };
                SqlParameter staffMemberPhoneNumberParam = new SqlParameter("@phone_number", SqlDbType.NVarChar) { Value = email };
                SqlParameter isUsedParam = new SqlParameter("@IsUsed", SqlDbType.Bit) { Direction = ParameterDirection.Output };


                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsStaffPhoneUsed", idParam, staffMemberPhoneNumberParam, isUsedParam);

            }
            catch
            {
                throw;
            }
        }

        public static async Task<bool> IsActive(int id)
        {
            SqlParameter idParam = new SqlParameter("@staff_id", SqlDbType.NVarChar) { Value = id };
            SqlParameter isExistParam = new SqlParameter("@IsActive", SqlDbType.Bit) { Direction = ParameterDirection.Output };

            return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsStaffMemberActive", idParam, isExistParam);
        }

        public static async Task<int> DeleteStaffMember(int id)
        {

            try
            {

                var staffMemberIDParam = new SqlParameter("@staff_id", SqlDbType.Int) { Value = id };

                return await SqlHelper.ExecuteNonQueryAsync("SP_DeleteStaffMember", staffMemberIDParam);

            }
            catch
            {
                throw;
            }
        }





        #endregion

        #region Private Helper Methods



        private static StaffMemberOutputDTO MapReaderToStaffMemberDTO(IDataReader reader, int? id = null)
        {

            int emailOrdinal = reader.GetOrdinal("email");
            int phoneNumber = reader.GetOrdinal("phone_number");
            return new StaffMemberOutputDTO(
                id ?? reader.GetInt32(reader.GetOrdinal("staff_id")),
                (enRole)reader.GetByte(reader.GetOrdinal("role")),
                reader.GetString(reader.GetOrdinal("first_name")),
                reader.GetString(reader.GetOrdinal("last_name")),
                reader.IsDBNull(emailOrdinal) ? null : reader.GetString(emailOrdinal),
                reader.IsDBNull(phoneNumber) ? null : reader.GetString(phoneNumber),
                reader.GetBoolean(reader.GetOrdinal("is_active"))
                );
        }


        private static StaffMemberIncryptedInputDTO MapReaderToStaffMemberIncryptedDTO(IDataReader reader, int? id = null)
        {

            int emailOrdinal = reader.GetOrdinal("email");
            int phoneNumber = reader.GetOrdinal("phone_number");
            return new StaffMemberIncryptedInputDTO(
                reader.GetString(reader.GetOrdinal("username")),
                reader.GetString(reader.GetOrdinal("password")),
                id ?? reader.GetInt32(reader.GetOrdinal("staff_id")),
                (enRole)reader.GetByte(reader.GetOrdinal("role")),
                reader.GetString(reader.GetOrdinal("first_name")),
                reader.GetString(reader.GetOrdinal("last_name")),
                reader.IsDBNull(emailOrdinal) ? null : reader.GetString(emailOrdinal),
                reader.IsDBNull(phoneNumber) ? null : reader.GetString(phoneNumber),
                reader.GetBoolean(reader.GetOrdinal("is_active"))
                );
        }

        
        #endregion
    }
}
