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
    public class TableDataAccess
    {
        #region Public Methods


        public static async Task<AddNewOutputDTO> AddNewTable(TableDTO tableDTO)
        {

            try
            {

                var parameters = new[]
                {
                    new SqlParameter("@table_id", SqlDbType.Int) { Direction = ParameterDirection.Output },
                    new SqlParameter("@CurrentTime", SqlDbType.DateTime) { Direction = ParameterDirection.Output },

                    new SqlParameter("@table_number", SqlDbType.NVarChar) { Value = tableDTO.Number },
                    new SqlParameter("@capacity", SqlDbType.Int) { Value = tableDTO.Capacity },
                    new SqlParameter("@current_status", SqlDbType.TinyInt){ Value = tableDTO.CurrentStatus },
                    new SqlParameter("@location", SqlDbType.NVarChar)
                    { Value = tableDTO.Location == null ? DBNull.Value : tableDTO.Location }
                };

                var outputValues = await SqlHelper.ExecuteWithOutputParametersAsync("SP_AddNewTable", parameters);

                AddNewOutputDTO addNewTableOutputDTO = new AddNewOutputDTO();

                if (outputValues.TryGetValue("table_id", out object ID) && ID != null)
                    addNewTableOutputDTO.ID = (int)ID;
                else
                    throw new SqlHelper.DataAccessException("Failed to retrieve new Table ID from stored procedure.");



                if (outputValues.TryGetValue("CurrentTime", out object createdAtObj))
                    addNewTableOutputDTO.CreatedAt = Convert.ToDateTime(createdAtObj);

                else
                    throw new SqlHelper.DataAccessException("Failed to retrieve CreatedAt timestamp for new Table.");


                return addNewTableOutputDTO;
            }
            catch
            {
                throw;
            }
        }



        public static async Task<PaginatedDTO<TableDTO>> GetAllTables(int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalTablesParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<TableDTO> paginatedTablesDTO = new PaginatedDTO<TableDTO>();

                paginatedTablesDTO.PageSize = pageSize;
                paginatedTablesDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedTables", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedTablesDTO.Items.Add(MapReaderToTableDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalTablesParam);

                paginatedTablesDTO.TotalItems =
                       TotalTablesParam.Value == DBNull.Value ? 0 : (int)TotalTablesParam.Value;

                return paginatedTablesDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<TableDTO>> GetTablesByStatus(enTableStatus status,int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter statusParam = new SqlParameter("@current_status", SqlDbType.TinyInt) { Value = (byte) status };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalTablesParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<TableDTO> paginatedTablesDTO = new PaginatedDTO<TableDTO>();

                paginatedTablesDTO.PageSize = pageSize;
                paginatedTablesDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedTablesByStatus", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedTablesDTO.Items.Add(MapReaderToTableDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalTablesParam, statusParam);

                paginatedTablesDTO.TotalItems =
                       TotalTablesParam.Value == DBNull.Value ? 0 : (int)TotalTablesParam.Value;

                return paginatedTablesDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<TableDTO>> GetTablesByCapacity(int capacity, int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter capacityParam = new SqlParameter("@capacity", SqlDbType.Int) { Value = capacity };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalTablesParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<TableDTO> paginatedTablesDTO = new PaginatedDTO<TableDTO>();

                paginatedTablesDTO.PageSize = pageSize;
                paginatedTablesDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedTablesByCapacity", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedTablesDTO.Items.Add(MapReaderToTableDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalTablesParam, capacityParam);

                paginatedTablesDTO.TotalItems =
                       TotalTablesParam.Value == DBNull.Value ? 0 : (int)TotalTablesParam.Value;

                return paginatedTablesDTO;
            }
            catch
            {
                throw;
            }
        }



        public static async Task<PaginatedDTO<TableDTO>> GetTablesByMinCapacityAndStatus(int capacity,enTableStatus status, int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter capacityParam = new SqlParameter("@capacity", SqlDbType.Int) { Value = capacity };
                SqlParameter statusParam = new SqlParameter("@current_status", SqlDbType.TinyInt) { Value = (byte)status };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalTablesParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<TableDTO> paginatedTablesDTO = new PaginatedDTO<TableDTO>();

                paginatedTablesDTO.PageSize = pageSize;
                paginatedTablesDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedTablesByMinCapacityAndStatus", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedTablesDTO.Items.Add(MapReaderToTableDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalTablesParam, capacityParam,statusParam);

                paginatedTablesDTO.TotalItems =
                       TotalTablesParam.Value == DBNull.Value ? 0 : (int)TotalTablesParam.Value;

                return paginatedTablesDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<TableDTO>> GetTablesByLocation(string location, int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter locationParam = new SqlParameter("@location", SqlDbType.NVarChar) { Value = location };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalTablesParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<TableDTO> paginatedTablesDTO = new PaginatedDTO<TableDTO>();

                paginatedTablesDTO.PageSize = pageSize;
                paginatedTablesDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedTablesByLocation", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedTablesDTO.Items.Add(MapReaderToTableDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalTablesParam, locationParam);

                paginatedTablesDTO.TotalItems =
                       TotalTablesParam.Value == DBNull.Value ? 0 : (int)TotalTablesParam.Value;

                return paginatedTablesDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<TableDTO>> GetAvailableTablesWithMinCapacity(int minCapacity, int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter minCapacityParam = new SqlParameter("@MinimumCapacity", SqlDbType.NVarChar) { Value = minCapacity };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalTablesParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<TableDTO> paginatedTablesDTO = new PaginatedDTO<TableDTO>();

                paginatedTablesDTO.PageSize = pageSize;
                paginatedTablesDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedAvailableTablesByMinCapacity", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedTablesDTO.Items.Add(MapReaderToTableDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalTablesParam, minCapacityParam);

                paginatedTablesDTO.TotalItems =
                       TotalTablesParam.Value == DBNull.Value ? 0 : (int)TotalTablesParam.Value;

                return paginatedTablesDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<TableDTO> GetTableByID(int id)
        {
            try
            {
                var param = new SqlParameter("@table_id", SqlDbType.Int) { Value = id };

                TableDTO tableDTO = new TableDTO();
                await SqlHelper.ExecuteReaderAsync("SP_GetTableByID", reader =>
                {
                    if (reader.Read())
                    {
                        tableDTO = MapReaderToTableDTO(reader, id);
                    }
                }, param);

                return tableDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<TableDTO> GetTableByTableNumber(string number)
        {
            try
            {
                var param = new SqlParameter("@table_number", SqlDbType.NVarChar) { Value = number };

                TableDTO tableDTO = new TableDTO();
                await SqlHelper.ExecuteReaderAsync("SP_GetTableByTableNumber", reader =>
                {
                    if (reader.Read())
                    {
                        tableDTO = MapReaderToTableDTO(reader);
                    }
                }, param);

                return tableDTO;
            }
            catch
            {
                throw;
            }
        }


        


        public static async Task<int> UpdateTable( TableDTO tableDTO)
        {

            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@table_id", SqlDbType.Int) { Value = tableDTO.ID },
                   new SqlParameter("@table_number", SqlDbType.NVarChar) { Value = tableDTO.Number },
                    new SqlParameter("@capacity", SqlDbType.Int) { Value = tableDTO.Capacity },
                    new SqlParameter("@current_status", SqlDbType.TinyInt) { Value = tableDTO.CurrentStatus},
                    new SqlParameter("@location", SqlDbType.NVarChar)
                    { Value = tableDTO.Location == null ? DBNull.Value : tableDTO.Location }

                };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateTable", parameters);

            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> UpdateStatus(int? id,enTableStatus status)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@table_id", SqlDbType.Int) { Value = id };
                 SqlParameter statusParam = new SqlParameter("@current_status", SqlDbType.TinyInt) { Value = (byte)status };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateTableStatus", idParam,statusParam);
            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> UpdateCapacity(int id, int capacity)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@table_id", SqlDbType.Int) { Value = id };
                SqlParameter capacityParam = new SqlParameter("@capacity", SqlDbType.Int) { Value = capacity };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateTableCapacity", idParam, capacityParam);
            }
            catch
            {
                throw;
            }
        }

        public static async Task<bool> IsNumberUsed(int id,string number)
        {

            try
            {

                SqlParameter tableIDParam = new SqlParameter("@table_id", SqlDbType.Int) { Value = id };
                SqlParameter tableNumberParam = new SqlParameter("@table_number", SqlDbType.NVarChar) { Value = number };
                SqlParameter isUsedParam = new SqlParameter("@IsUsed", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsNumberUsed", tableIDParam, tableNumberParam, isUsedParam);

            }
            catch
            {
                throw;
            }
        }

        public static async Task<bool> IsCapacitySufficientForGuests(int id,int guests)
        {

            try
            {

                SqlParameter tableIDParam = new SqlParameter("@table_id", SqlDbType.Int) { Value = id };
                SqlParameter guestsParam = new SqlParameter("@number_of_guests", SqlDbType.Int) { Value = guests };
                SqlParameter isSufficientParam = new SqlParameter("@IsSufficient", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsCapacitySufficientForGuests"
                    ,tableIDParam,guestsParam,isSufficientParam);

            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> UpdateTableLocation(int id, string location)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@table_id", SqlDbType.Int) { Value = id };
                SqlParameter locationParam = new SqlParameter("@location", SqlDbType.NVarChar) { Value = location };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateTableLocation", idParam, locationParam);
            }
            catch
            {
                throw;
            }
        }


        public static async Task<bool> IsTableAvailable(string number)
        {

            try
            {

                SqlParameter tableNumberParam = new SqlParameter("@table_number", SqlDbType.NVarChar) { Value = number };
                SqlParameter isAvailableParam = new SqlParameter("@is_available", SqlDbType.Bit) { Direction = ParameterDirection.Output };


                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsTableAvailable", tableNumberParam, isAvailableParam);

            }
            catch
            {
                throw;
            }
        }


        public static async Task<bool> IsTableExitst(int? id)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@table_id", SqlDbType.NVarChar) { Value = id };
                SqlParameter isExistParam = new SqlParameter("@IsExist", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsTableExist",idParam,isExistParam);

            }
            catch
            {
                throw;
            }
        }



        public static async Task<int> DeleteTable(int id)
        {

            try
            {

                var tableIDParam = new SqlParameter("@table_id", SqlDbType.Int) { Value = id };

                return await SqlHelper.ExecuteNonQueryAsync("SP_DeleteTable", tableIDParam);

            }
            catch
            {
                throw;
            }
        }





        #endregion

        #region Private Helper Methods



        private static TableDTO MapReaderToTableDTO(IDataReader reader, int? id = null)
        {
            return new TableDTO(
                id ?? reader.GetInt32(reader.GetOrdinal("table_id")),
                reader.GetString(reader.GetOrdinal("table_number")),
                reader.GetInt32(reader.GetOrdinal("capacity")),
                (enTableStatus)reader.GetByte(reader.GetOrdinal("current_status")),
                reader.GetString(reader.GetOrdinal("location"))
                );
        }

        #endregion
    }
}
