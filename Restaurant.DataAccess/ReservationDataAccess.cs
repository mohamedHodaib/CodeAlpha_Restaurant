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
    public class ReservationDataAccess
    {
        #region Public Methods


        public static async Task<AddNewOutputDTO> AddNewReservation(ReservationDTO reservationDTO)
        {

            try
            {

                var parameters = new[]
                {
                    new SqlParameter("@reservation_id", SqlDbType.Int) 
                    { Direction = ParameterDirection.Output },
                    new SqlParameter("@CurrentTime", SqlDbType.DateTime) 
                    { Direction = ParameterDirection.Output },
                    new SqlParameter("@customer_id", SqlDbType.Int)
                    { Value = reservationDTO.CustomerID == null ? DBNull.Value : reservationDTO.CustomerID },
                     new SqlParameter("@table_id", SqlDbType.Int)
                    { Value =  reservationDTO.TableID },
                    new SqlParameter("@reservation_time", SqlDbType.DateTime) { Value = reservationDTO.ReservationTime },
                    new SqlParameter("@number_of_guests", SqlDbType.Int) { Value = reservationDTO.NumberOfGuests },
                    new SqlParameter("@special_requests", SqlDbType.NVarChar)
                    { Value = reservationDTO.SpecialRequests == null ? DBNull.Value : reservationDTO.SpecialRequests },
                    new SqlParameter("@status", SqlDbType.TinyInt)
                    { Value = reservationDTO.Status == null ? DBNull.Value : (byte)reservationDTO.Status }
                };

                var outputValues = await SqlHelper.ExecuteWithOutputParametersAsync("SP_AddNewReservation", parameters);

                AddNewOutputDTO addNewReservationOutputDTO = new AddNewOutputDTO();

                if (outputValues.TryGetValue("reservation_id", out object ID) && ID != null)
                    addNewReservationOutputDTO.ID = (int)ID;
                else
                    throw new SqlHelper.DataAccessException("Failed to retrieve new Reservation ID from stored procedure.");



                if (outputValues.TryGetValue("CurrentTime", out object createdAtObj))
                    addNewReservationOutputDTO.CreatedAt = Convert.ToDateTime(createdAtObj);

                else
                    throw new SqlHelper.DataAccessException("Failed to retrieve CreatedAt timestamp for new Reservation.");



                return addNewReservationOutputDTO;
            }
            catch
            {
                throw;
            }
        }



        public static async Task<PaginatedDTO<ReservationDTO>> GetAllReservations(int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalReservationsParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<ReservationDTO> paginatedReservationsDTO = new PaginatedDTO<ReservationDTO>();

                paginatedReservationsDTO.PageSize = pageSize;
                paginatedReservationsDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedAllReservations", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedReservationsDTO.Items.Add(MapReaderToReservationDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalReservationsParam);

                paginatedReservationsDTO.TotalItems =
                       TotalReservationsParam.Value == DBNull.Value ? 0 : (int)TotalReservationsParam.Value;

                return paginatedReservationsDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<ReservationDTO>> GetReservationsByCustomerID(int customerID,int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter customerIDParam = new SqlParameter("@customer_id", SqlDbType.Int) { Value = customerID };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalReservationsParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<ReservationDTO> paginatedReservationsDTO = new PaginatedDTO<ReservationDTO>();

                paginatedReservationsDTO.PageSize = pageSize;
                paginatedReservationsDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedReservationsByCustomerID", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedReservationsDTO.Items.Add(MapReaderToReservationDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalReservationsParam,customerIDParam);

                paginatedReservationsDTO.TotalItems =
                       TotalReservationsParam.Value == DBNull.Value ? 0 : (int)TotalReservationsParam.Value;

                return paginatedReservationsDTO;
            }
            catch
            {
                throw;
            }
        }

        public static async Task<PaginatedDTO<ReservationDTO>> GetReservationsByTableID(int tableID, int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter tableIDParam = new SqlParameter("@table_id", SqlDbType.Int) { Value = tableID };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalReservationsParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<ReservationDTO> paginatedReservationsDTO = new PaginatedDTO<ReservationDTO>();

                paginatedReservationsDTO.PageSize = pageSize;
                paginatedReservationsDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedReservationsByTableID", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedReservationsDTO.Items.Add(MapReaderToReservationDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalReservationsParam, tableIDParam);

                paginatedReservationsDTO.TotalItems =
                       TotalReservationsParam.Value == DBNull.Value ? 0 : (int)TotalReservationsParam.Value;

                return paginatedReservationsDTO;
            }
            catch
            {
                throw;
            }
        }



        public static async Task<PaginatedDTO<ReservationDTO>> GetReservationsByStatus(enReservationStatus status, int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter statusParam = new SqlParameter("@status", SqlDbType.TinyInt) { Value = (byte)status };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalReservationsParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<ReservationDTO> paginatedReservationsDTO = new PaginatedDTO<ReservationDTO>();

                paginatedReservationsDTO.PageSize = pageSize;
                paginatedReservationsDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedReservationsByStatus", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedReservationsDTO.Items.Add(MapReaderToReservationDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalReservationsParam, statusParam);

                paginatedReservationsDTO.TotalItems =
                       TotalReservationsParam.Value == DBNull.Value ? 0 : (int)TotalReservationsParam.Value;

                return paginatedReservationsDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<ReservationDTO>> GetReservationsWithinPeriod
            (DateOnly startDate,DateOnly endDate,int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter startDateParam = new SqlParameter("@StartDate", SqlDbType.Date) { Value = startDate };
                SqlParameter endDateParam = new SqlParameter("@EndDate", SqlDbType.Date) { Value = endDate };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalReservationsParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<ReservationDTO> paginatedReservationsDTO = new PaginatedDTO<ReservationDTO>();

                paginatedReservationsDTO.PageSize = pageSize;
                paginatedReservationsDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedReservationsWithinPeriod", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedReservationsDTO.Items.Add(MapReaderToReservationDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, 
                TotalReservationsParam, startDateParam,endDateParam);

                paginatedReservationsDTO.TotalItems =
                       TotalReservationsParam.Value == DBNull.Value ? 0 : (int)TotalReservationsParam.Value;

                return paginatedReservationsDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<ReservationDTO>> GetUpcommingReservations(int pageNumber, int pageSize)
        {
            try
            {
                
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalReservationsParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<ReservationDTO> paginatedReservationsDTO = new PaginatedDTO<ReservationDTO>();

                paginatedReservationsDTO.PageSize = pageSize;
                paginatedReservationsDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedUpcommingReservations", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedReservationsDTO.Items.Add(MapReaderToReservationDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam,TotalReservationsParam);

                paginatedReservationsDTO.TotalItems =
                       TotalReservationsParam.Value == DBNull.Value ? 0 : (int)TotalReservationsParam.Value;

                return paginatedReservationsDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<ReservationDTO>> GetPastReservations(int pageNumber, int pageSize)
        {
            try
            {

                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalReservationsParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<ReservationDTO> paginatedReservationsDTO = new PaginatedDTO<ReservationDTO>();

                paginatedReservationsDTO.PageSize = pageSize;
                paginatedReservationsDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedPastReservations", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedReservationsDTO.Items.Add(MapReaderToReservationDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalReservationsParam);

                paginatedReservationsDTO.TotalItems =
                       TotalReservationsParam.Value == DBNull.Value ? 0 : (int)TotalReservationsParam.Value;

                return paginatedReservationsDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<ReservationDTO>> GetReservationsByNumberOfGuests(int numberOfGuests,int pageNumber, int pageSize)
        {
            try
            {

                SqlParameter numberOfGuestsParam = new SqlParameter("@number_of_guests", SqlDbType.NVarChar) { Value = numberOfGuests };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalReservationsParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<ReservationDTO> paginatedReservationsDTO = new PaginatedDTO<ReservationDTO>();

                paginatedReservationsDTO.PageSize = pageSize;
                paginatedReservationsDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedReservationsByNumberOfGuests", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedReservationsDTO.Items.Add(MapReaderToReservationDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalReservationsParam,numberOfGuestsParam);

                paginatedReservationsDTO.TotalItems =
                       TotalReservationsParam.Value == DBNull.Value ? 0 : (int)TotalReservationsParam.Value;

                return paginatedReservationsDTO;
            }
            catch
            {
                throw;
            }
        }

        public static async Task<ReservationDTO> GetReservationByID(int id)
        {
            try
            {
                var param = new SqlParameter("@ReservationID", SqlDbType.Int) { Value = id };

                ReservationDTO reservationDTO = new ReservationDTO();
                await SqlHelper.ExecuteReaderAsync("SP_GetReservationByID", reader =>
                {
                    if (reader.Read())
                    {
                        reservationDTO = MapReaderToReservationDTO(reader, id);
                    }
                }, param);

                return reservationDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> UpdateStatus(int id, enReservationStatus newStatus)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@reservation_id", SqlDbType.Int) { Value = id };
                SqlParameter statusParam = new SqlParameter("@status", SqlDbType.TinyInt)
                { Value = (byte)newStatus };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateReservationStatus", idParam, statusParam);
            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> UpdateReservationTime(int id, DateTime newReservationTime)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@reservation_id", SqlDbType.Int) { Value = id };
                SqlParameter newReservationTimeParam = new SqlParameter("@reservation_time", SqlDbType.DateTime)
                { Value = newReservationTime };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateReservationTime", idParam, newReservationTimeParam);
            }
            catch
            {
                throw;
            }
        }



        public static async Task<int> UpdateNumberOfGuests(int id, int newNumberOfGuests)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@reservation_id", SqlDbType.Int) { Value = id };
                SqlParameter numberOfGuestsParam = new SqlParameter("@number_of_guests", SqlDbType.Int)
                { Value = newNumberOfGuests };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateNumberOfGuests", idParam, numberOfGuestsParam);
            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> UpdateSpecialRequests(int id, string newSpecialRequests)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@reservation_id", SqlDbType.Int) { Value = id };
                SqlParameter newSpecialRequestsParam = new SqlParameter("@special_requests", SqlDbType.NVarChar)
                { Value = newSpecialRequests };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateSpecialRequests", idParam, newSpecialRequestsParam);
            }
            catch
            {
                throw;
            }
        }

        public static async Task<int> UpdateReservedTable(int id, int tableID)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@reservation_id", SqlDbType.Int) { Value = id };
                SqlParameter tableIDParam = new SqlParameter("@table_id", SqlDbType.Int)
                { Value = tableID };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateReservedTable", idParam, tableIDParam);
            }
            catch
            {
                throw;
            }
        }

        public static async Task<int> UpdateReservation( ReservationDTO reservationDTO)
        {

            try
            {

                var parameters = new[]
                {
                     new SqlParameter("@reservation_id", SqlDbType.Int)
                     { Value = reservationDTO.ReservationID },
                     new SqlParameter("@customer_id", SqlDbType.Int)
                     { Value = reservationDTO.CustomerID == null ? DBNull.Value : reservationDTO.CustomerID },
                     new SqlParameter("@table_id", SqlDbType.Int)
                     { Value =  reservationDTO.TableID },
                     new SqlParameter("@reservation_time", SqlDbType.DateTime) { Value = reservationDTO.ReservationTime },
                     new SqlParameter("@number_of_guests", SqlDbType.Int) { Value = reservationDTO.NumberOfGuests },
                     new SqlParameter("@special_requests", SqlDbType.NVarChar)
                     { Value = reservationDTO.SpecialRequests == null ? DBNull.Value : reservationDTO.SpecialRequests },
                     new SqlParameter("@status", SqlDbType.TinyInt)
                     { Value = reservationDTO.Status == null ? DBNull.Value : (byte) reservationDTO.Status }
                 };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateReservation", parameters);
            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> DeleteReservation(int id)
        {

            try
            {

                var idParam = new SqlParameter("@reservation_id", SqlDbType.Int) { Value = id };

                return await SqlHelper.ExecuteNonQueryAsync("SP_DeleteReservation", idParam);

            }
            catch
            {
                throw;
            }
        }



        public static async Task<bool> IsResevationTimeUsedForTable(int tableID,DateTime reservationTime)
        {

            try
            {

                SqlParameter tableIDParam = new SqlParameter("@table_id", SqlDbType.Int) { Value = tableID };
                SqlParameter reservationTimeParam = new SqlParameter("@reservation_time", SqlDbType.DateTime) { Value = reservationTime };
                SqlParameter isUsedParam = new SqlParameter("@IsUsed", SqlDbType.Bit) { Direction = ParameterDirection.Output };


                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsResevationTimeUsedForTable"
                    , tableIDParam, reservationTimeParam, isUsedParam);

            }
            catch
            {
                throw;
            }
        }

        public static async Task<bool> IsCustomerHasAlreadyExistingReservationAtReservationTime(int? customerID, DateTime reservationTime)
        {

            try
            {

                SqlParameter customerIDParam = new SqlParameter("@customer_id", SqlDbType.Int) { Value = customerID };
                SqlParameter reservationTimeParam = new SqlParameter("@reservation_time", SqlDbType.DateTime) { Value = reservationTime };
                SqlParameter isUsedParam = new SqlParameter("@IsUsed", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsCustomerHasAlreadyExistingReservationAtReservationTime"
                    , customerIDParam, reservationTimeParam,isUsedParam);

            }
            catch
            {
                throw;
            }
        }



        public static async Task<bool> IsReservationExist(int id)
        {

            try
            {

                SqlParameter tableIDParam = new SqlParameter("@reservation_id", SqlDbType.Int) { Value = id };
                SqlParameter isExistParam = new SqlParameter("@IsExist", SqlDbType.Bit) { Direction = ParameterDirection.Output };


                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsReservationExist"
                    , tableIDParam, isExistParam);

            }
            catch
            {
                throw;
            }
        }


       
        #endregion

        #region Private Helper Methods



        private static ReservationDTO MapReaderToReservationDTO(IDataReader reader, int? id = null)
        {
            //get ordinal of Optional columns
            int customerOrdinal = reader.GetOrdinal("customer_id");
            int statusOrdinal = reader.GetOrdinal("status");
            int specialRequestsOrdinal = reader.GetOrdinal("special_requests");

            return new ReservationDTO(
                id ?? reader.GetInt32(reader.GetOrdinal("reservation_id")),
                reader.IsDBNull(customerOrdinal) ? null : reader.GetInt32(customerOrdinal),
                reader.GetInt32(reader.GetOrdinal("table_id")),
                reader.GetDateTime(reader.GetOrdinal("reservation_time")),
                reader.GetInt32(reader.GetOrdinal("number_of_guests")),
                reader.IsDBNull(statusOrdinal) ? null : (enReservationStatus)reader.GetByte(statusOrdinal),
                reader.IsDBNull(specialRequestsOrdinal) ? null :
                reader.GetString(specialRequestsOrdinal)
                );
        }

        #endregion
    }
}
