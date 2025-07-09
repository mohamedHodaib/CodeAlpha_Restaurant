using Restaurant.DataAccess.DTOs;
using Restaurant.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using Restaurant.DataAccess.DTOs.Order;

namespace Restaurant.Business
{
    public class Reservation
    {
        public enum enMode { AddNew, Update };
        public enMode Mode = enMode.AddNew;


        public ReservationDTO reservationDTO => new ReservationDTO(ReservationID,CustomerID,
            TableID,ReservationTime,NumberOfGuests,Status,SpecialRequests);


        public int ReservationID { get; set; }
        public int? CustomerID { get; set; }
        public int TableID { get; set; }
        public DateTime ReservationTime { get; set; }
        public int NumberOfGuests { get; set; }
        public enReservationStatus? Status { get; set; }
        public string? SpecialRequests { get; set; }

        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? UpdatedAt { get; set; } = null;

        public Reservation(ReservationDTO staffDTO, enMode mode = enMode.AddNew)
        {

            ReservationID = staffDTO.ReservationID;
            CustomerID = staffDTO.CustomerID;
            TableID = staffDTO.TableID;
            ReservationTime = staffDTO.ReservationTime;
            NumberOfGuests = staffDTO.NumberOfGuests;
            Status = staffDTO.Status;
            SpecialRequests = staffDTO.SpecialRequests;

            Mode = mode;
        }


        private async Task<BusinessResult<bool>> _AddNewReservation()
        {
            try
            {

                AddNewOutputDTO addNewOutputDTO = 
                    await ReservationDataAccess.AddNewReservation(reservationDTO);

                ReservationID = addNewOutputDTO.ID;
                CreatedAt = addNewOutputDTO.CreatedAt;
                UpdatedAt = addNewOutputDTO.CreatedAt;

                if (ReservationID > 0)
                    return BusinessResult<bool>.SuccessResult("Reservation Record  Created Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Reservation Record Creation Fail"
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

        public static async Task<BusinessResult<PaginatedDTO<ReservationDTO>>> GetAllReservations(int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<ReservationDTO> PaginatedReservations =
                    await ReservationDataAccess.GetAllReservations(pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<ReservationDTO>>
                    .SuccessResult("Reservation Records Returned Successfully.", PaginatedReservations);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Reservations.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Reservations.", ex);
            }

        }


        public static async Task<BusinessResult<PaginatedDTO<ReservationDTO>>> GetReservationsByCustomerID(int customerID, int pageNumber, int pageSize)
        {
            try
            {
                if (!await Customer.IsCustomerIDExist(customerID))
                    return BusinessResult<PaginatedDTO<ReservationDTO>>.FailureResult("Customer with id Not Exist."
                        , BusinessResult<PaginatedDTO<ReservationDTO>>.enError.BadRequest);

                PaginatedDTO<ReservationDTO> PaginatedReservations =
                    await ReservationDataAccess.GetReservationsByCustomerID(customerID, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<ReservationDTO>>
                    .SuccessResult("Reservation Records Returned Successfully.", PaginatedReservations);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Reservations.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Reservations.", ex);
            }
        }

        public static async Task<BusinessResult<PaginatedDTO<ReservationDTO>>> GetReservationsByTableID(int tableID, int pageNumber, int pageSize)
        {
            try
            {
                if (!await Table.IsTableIDExist(tableID))
                    return BusinessResult<PaginatedDTO<ReservationDTO>>.FailureResult("Table is not exist"
                        , BusinessResult<PaginatedDTO<ReservationDTO>>.enError.BadRequest);

                PaginatedDTO<ReservationDTO> PaginatedReservations =
                    await ReservationDataAccess.GetReservationsByTableID(tableID, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<ReservationDTO>>
                    .SuccessResult("Reservation Records Returned Successfully.", PaginatedReservations);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Reservations.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Reservations.", ex);
            }
        }


        public static async Task<BusinessResult<PaginatedDTO<ReservationDTO>>> GetReservationsByStatus(enReservationStatus status, int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<ReservationDTO> PaginatedReservations =
                    await ReservationDataAccess.GetReservationsByStatus(status, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<ReservationDTO>>
                    .SuccessResult("Reservation Records Returned Successfully.", PaginatedReservations);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Reservations.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Reservations.", ex);
            }
        }



        public static async Task<BusinessResult<PaginatedDTO<ReservationDTO>>> GetReservationsWithinPeriod(DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<ReservationDTO> PaginatedReservations =
                    await ReservationDataAccess.GetReservationsWithinPeriod(startDate,endDate, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<ReservationDTO>>
                    .SuccessResult("Reservation Records Returned Successfully.", PaginatedReservations);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Reservations.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Reservations.", ex);
            }
        }


        public static async Task<BusinessResult<PaginatedDTO<ReservationDTO>>> GetUpcommingReservations( int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<ReservationDTO> PaginatedReservations =
                    await ReservationDataAccess.GetUpcommingReservations(pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<ReservationDTO>>
                    .SuccessResult("Reservation Records Returned Successfully.", PaginatedReservations);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Reservations.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Reservations.", ex);
            }
        }

        public static async Task<BusinessResult<PaginatedDTO<ReservationDTO>>> GetPastReservations(int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<ReservationDTO> PaginatedReservations =
                    await ReservationDataAccess.GetPastReservations(pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<ReservationDTO>>
                    .SuccessResult("Reservation Records Returned Successfully.", PaginatedReservations);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Reservations.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Reservations.", ex);
            }
        }



        public static async Task<BusinessResult<PaginatedDTO<ReservationDTO>>> GetReservationsByNumberOfGuests(int numberOfGuests,int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<ReservationDTO> PaginatedReservations =
                    await ReservationDataAccess.GetReservationsByNumberOfGuests(numberOfGuests,pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<ReservationDTO>>
                    .SuccessResult("Reservation Records Returned Successfully.", PaginatedReservations);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Reservations.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Reservations.", ex);
            }
        }



        public static async Task<BusinessResult<Reservation>> Find(int id)
        {
            try
            {
                ReservationDTO reservationDTO = await ReservationDataAccess.GetReservationByID(id);

                if (reservationDTO.ReservationID != 0)
                    return BusinessResult<Reservation>.SuccessResult("Reservation  Found successfully",
                       new Reservation(reservationDTO,enMode.Update));

                else
                    return BusinessResult<Reservation>.FailureResult($"Reservation with ID {id} Not Found",
                        BusinessResult<Reservation>.enError.NotFound);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Reservation By ID.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Reservation By ID.", ex);
            }

        }


        private async Task<BusinessResult<bool>> _UpdateReservation()
        {
            try
            {
                if (!await _IsReservationIDExist(ReservationID))
                    return BusinessResult<bool>.FailureResult("Reservation is not exist");

                int rowsAffected = await ReservationDataAccess.UpdateReservation(reservationDTO);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Reservation Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Reservation Record Fail."
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


        public async Task<BusinessResult<bool>> Cancel()
        {
            return await UpdateStatus(ReservationID,enReservationStatus.Cancelled);
        }


        public async Task<BusinessResult<bool>> MarkAsNoShow()
        {
            if (ReservationTime < DateTime.Now)
                return await UpdateStatus(ReservationID, enReservationStatus.NoShow);

            else
                return BusinessResult<bool>.FailureResult("This Reservation not Passed Yet",
                    BusinessResult<bool>.enError.BadRequest);
        }

        public static async Task<BusinessResult<bool>> UpdateStatus(int ID,enReservationStatus status)
        {
            try
            {
                if (!await _IsReservationIDExist(ID))
                    return BusinessResult<bool>.FailureResult("Reservation is not exist");

                int rowsAffected = await ReservationDataAccess.UpdateStatus(ID, status);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Reservation Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Reservation Record Fail."
                        ,BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Reservation Status.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Reservation Status .", ex);
            }
        }


        public static async Task<BusinessResult<bool>> UpdateTime(int ID, DateTime time)
        {
            try
            {
                if (!await _IsReservationIDExist(ID))
                    return BusinessResult<bool>.FailureResult("Reservation is not exist");

                int rowsAffected = await ReservationDataAccess.UpdateReservationTime(ID, time);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Reservation Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Reservation Record Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Reservation time.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Reservation time .", ex);
            }
        }


        public static async Task<BusinessResult<bool>> UpdateNumberOfGuests(int ID, int numberOfGuests)
        {
            try
            {
                if (!await _IsReservationIDExist(ID))
                    return BusinessResult<bool>.FailureResult("Reservation is not exist"
                        ,BusinessResult<bool>.enError.BadRequest);

                BusinessResult<Reservation> reservation = await Find(ID);
                if(!reservation.Success)
                    return BusinessResult<bool>.FailureResult("Update Number of Guests Fail."
                        , BusinessResult<bool>.enError.ServerError);

                if (!await TableDataAccess.IsCapacitySufficientForGuests(reservation.Data.TableID, numberOfGuests))
                    return BusinessResult<bool>.FailureResult("Table Capacity is not Sufficient for Guests."
                        ,BusinessResult<bool>.enError.BadRequest);


                int rowsAffected = await ReservationDataAccess.UpdateNumberOfGuests(ID, numberOfGuests);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Reservation Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Reservation Record Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Reservation .", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Reservation  .", ex);
            }
        }


        public static async Task<BusinessResult<bool>> UpdateSpecialRequests(int ID, string specialRequests)
        {
            try
            {
                if (!await _IsReservationIDExist(ID))
                    return BusinessResult<bool>.FailureResult("Reservation is not exist"
                        ,BusinessResult<bool>.enError.BadRequest);


                int rowsAffected = await ReservationDataAccess.UpdateSpecialRequests(ID, specialRequests);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Reservation Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Reservation Record Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Reservation .", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Reservation  .", ex);
            }
        }


        public static async Task<BusinessResult<bool>> UpdateReservedTable(int ID, int  tableID)
        {
            try
            {

                if (!await _IsReservationIDExist(ID))
                    return BusinessResult<bool>.FailureResult("Reservation is not exist"
                        ,BusinessResult<bool>.enError.BadRequest);

                int rowsAffected = await ReservationDataAccess.UpdateReservedTable(ID, tableID);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Reservation Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Reservation Record Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Reservation .", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Reservation  .", ex);
            }
        }

        public async Task<BusinessResult<bool>> Delete()
        {
            try
            {

                int rowsAffected =
                    await ReservationDataAccess.DeleteReservation(ReservationID);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Reservation Deleted Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Delete Reservation Ingrediant Fail."
                        ,BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Deleting Reservation.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Deleting Reservation.", ex);
            }

        }



        public async Task<BusinessResult<bool>> Save()
        {
            try
            {

                if (!await Table.
                   IsTableIDExist(TableID))
                    return BusinessResult<bool>.FailureResult
                        ("Table Is Not Exist,choose another one."
                        , BusinessResult<bool>.enError.BadRequest);

                if (CustomerID != null && !await Customer.
                   IsCustomerIDExist(CustomerID.Value))
                    return BusinessResult<bool>.FailureResult
                        ("Customer Is Not Registered in the system."
                        , BusinessResult<bool>.enError.BadRequest);

                if (await ReservationDataAccess.
                    IsResevationTimeUsedForTable(ReservationID, ReservationTime))
                    return BusinessResult<bool>.FailureResult
                        ("Table Is Reserved at This Time,choose another table."
                        , BusinessResult<bool>.enError.Conflict);

                if (await ReservationDataAccess.
                    IsCustomerHasAlreadyExistingReservationAtReservationTime(CustomerID, ReservationTime))
                    return BusinessResult<bool>.FailureResult("You Already Have Reservation at This Time,choose another Time."
                        , BusinessResult<bool>.enError.Conflict);


                if (!await TableDataAccess.IsCapacitySufficientForGuests(TableID,NumberOfGuests))
                    return BusinessResult<bool>.FailureResult("Table Capacity is not Sufficient for Guests."
                        , BusinessResult<bool>.enError.BadRequest);

                switch (Mode)
                {
                    case enMode.AddNew:

                        var result = await _AddNewReservation();

                        if (result.Success)
                            Mode = enMode.Update;

                        return result;

                    case enMode.Update:

                        return await _UpdateReservation();

                }

                return BusinessResult<bool>.FailureResult("Add/Update Reservation Operation Failed."
                    ,BusinessResult<bool>.enError.ServerError);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Add/Update Reservation.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Add/Update Reservation.", ex);
            }

        }



        private static async Task<bool> _IsReservationIDExist(int id) =>
          await ReservationDataAccess.IsReservationExist(id);

    }
}
