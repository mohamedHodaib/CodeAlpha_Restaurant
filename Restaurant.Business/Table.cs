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
    public class Table
    {
        public enum enMode { AddNew, Update };
        public enMode Mode = enMode.AddNew;




        public TableDTO tableDTO => new TableDTO(ID,Number,Capacity,CurrentStatus,Location);


        public int ID { get; set; }
        public string Number { get; set; }
        public int Capacity { get; set; }
        public enTableStatus CurrentStatus { get; set; }
        public string Location { get; set; }

        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? UpdatedAt { get; set; } = null;

        public Table(TableDTO tableDTO, enMode mode = enMode.AddNew)
        {
            ID = tableDTO.ID;
            Number = tableDTO.Number;
            Capacity = tableDTO.Capacity;

            if (mode == enMode.AddNew)
                CurrentStatus = enTableStatus.Available;
            else
                CurrentStatus = tableDTO.CurrentStatus;

            Location = tableDTO.Location;

            Mode = mode;
        }


        private async Task<BusinessResult<bool>> _AddNewTable()
        {
            try
            {

                AddNewOutputDTO addNewOutputDTO = await TableDataAccess.AddNewTable(tableDTO);

                ID = addNewOutputDTO.ID;
                CreatedAt = addNewOutputDTO.CreatedAt;
                UpdatedAt = addNewOutputDTO.CreatedAt;

                if (ID > 0)
                    return BusinessResult<bool>.SuccessResult("Table Record  Created Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Table Record Creation Fail",
                        BusinessResult<bool>.enError.ServerError);
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

        public static async Task<BusinessResult<PaginatedDTO<TableDTO>>> GetAllTables(int pageNumber, int pageSize)
        {
            try
            {



                PaginatedDTO<TableDTO> PaginatedTables =
                    await TableDataAccess.GetAllTables(pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<TableDTO>>
                    .SuccessResult("Table Records Returned Successfully.", PaginatedTables);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving All Tables.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving All Tables.", ex);
            }

        }


        public static async Task<BusinessResult<PaginatedDTO<TableDTO>>> GetTablesByStatus(enTableStatus status, int pageNumber, int pageSize)
        {
            try
            {

                PaginatedDTO<TableDTO> PaginatedTables =
                    await TableDataAccess.GetTablesByStatus(status,pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<TableDTO>>
                    .SuccessResult("Table Records Returned Successfully.", PaginatedTables);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving  Tables By Status.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Tables By Status.", ex);
            }
        }


        public static async Task<BusinessResult<PaginatedDTO<TableDTO>>> GetTablesByCapacity(int capacity, int pageNumber, int pageSize)
        {
            try
            {

                PaginatedDTO<TableDTO> PaginatedTables =
                    await TableDataAccess.GetTablesByCapacity(capacity, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<TableDTO>>
                    .SuccessResult("Table Records Returned Successfully.", PaginatedTables);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving  Tables By Capacity.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Tables By Capacity.", ex);
            }
        }

        public static async Task<BusinessResult<PaginatedDTO<TableDTO>>> GetAvailableTablesWithMinCapacity(int minCapacity, int pageNumber, int pageSize)
        {
            try
            {

                PaginatedDTO<TableDTO> PaginatedTables =
                    await TableDataAccess.GetAvailableTablesWithMinCapacity(minCapacity, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<TableDTO>>
                    .SuccessResult("Table Records Returned Successfully.", PaginatedTables);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving  Tables By Capacity.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Tables By Capacity.", ex);
            }
        }


        public static async Task<BusinessResult<PaginatedDTO<TableDTO>>> GetTablesByMinCapacityAndStatus(int capacity,enTableStatus status, int pageNumber, int pageSize)
        {
            try
            {

                PaginatedDTO<TableDTO> PaginatedTables =
                    await TableDataAccess.GetTablesByMinCapacityAndStatus(capacity,status, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<TableDTO>>
                    .SuccessResult("Table Records Returned Successfully.", PaginatedTables);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving  Tables By Capacity And Status.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Tables By Capacity And Status.", ex);
            }
        }


        public static async Task<BusinessResult<PaginatedDTO<TableDTO>>> GetTablesByLocation(string location, int pageNumber, int pageSize)
        {
            try
            {

                PaginatedDTO<TableDTO> PaginatedTables =
                    await TableDataAccess.GetTablesByLocation(location, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<TableDTO>>
                    .SuccessResult("Table Records Returned Successfully.", PaginatedTables);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving  Tables By Location.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Tables By Location.", ex);
            }
        }


        

        public static async Task<BusinessResult<Table>> Find(int id)
        {
            try
            {
                TableDTO tableDTO = await TableDataAccess.GetTableByID(id);

                if (tableDTO.ID != 0)
                    return BusinessResult<Table>.SuccessResult("Table  Found successfully",
                        new Table(tableDTO, enMode.Update));

                else
                    return BusinessResult<Table>.FailureResult($"Table with ID {id} Not Found"
                        ,BusinessResult<Table>.enError.NotFound);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Table By ID.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Table By ID.", ex);
            }

        }

        public static async Task<BusinessResult<Table>> Find(string number)
        {
            try
            {
                TableDTO tableDTO = await TableDataAccess.GetTableByTableNumber(number);

                if (tableDTO.ID != 0)
                    return BusinessResult<Table>.SuccessResult("Table  Found successfully",
                        new Table(tableDTO, enMode.Update));

                else
                    return BusinessResult<Table>.FailureResult($"Table with Number {number} Not Found"
                        ,BusinessResult<Table>.enError.NotFound);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Table By Number.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Table By Number.", ex);
            }
        }


        private async Task<BusinessResult<bool>> _UpdateTable()
        {
            try
            {
                int rowsAffected = await TableDataAccess.UpdateTable(tableDTO);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Table Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Table Record Fail."
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


        public static async Task<BusinessResult<bool>> UpdateTableStatus(int? id, enTableStatus status)
        {
            try
            {
                if(!await IsTableIDExist(id))
                    return BusinessResult<bool>.FailureResult("Table is not exist"
                        , BusinessResult<bool>.enError.BadRequest);

                int rowsAffected = await TableDataAccess.UpdateStatus(id, status);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Table Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Table Record Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Table Status.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Table Status.", ex);
            }
        }


        public static async Task<BusinessResult<bool>> UpdateTableCapacity(int id, int capacity)
        {
            try
            {
                if (!await IsTableIDExist(id))
                    return BusinessResult<bool>.FailureResult("Table is not exist"
                        , BusinessResult<bool>.enError.BadRequest);

                int rowsAffected = await TableDataAccess.UpdateCapacity(id, capacity);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Table Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Table Record Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Table Capacity.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Table Capacity.", ex);
            }
        }


        public static async Task<BusinessResult<bool>> UpdateTableLocation(int id, string location)
        {
            try
            {
                if (!await IsTableIDExist(id))
                    return BusinessResult<bool>.FailureResult("Table is not exist"
                        , BusinessResult<bool>.enError.BadRequest);

                int rowsAffected = await TableDataAccess.UpdateTableLocation(id, location);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Table Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Table Record Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Table Location.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Table Location.", ex);
            }
        }


        public async Task<BusinessResult<bool>> Delete()
        {
            try
            {

                int rowsAffected =
                    await TableDataAccess.DeleteTable(ID);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Table Deleted Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Delete Table  Fail."
                        , BusinessResult<bool>.enError.BadRequest);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Deleting Table.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Deleting Table.", ex);
            }

        }

        public async Task<BusinessResult<bool>> Save()
        {
            try
            {
                if (await _IsTableNumberUsed(ID,Number))
                    return BusinessResult<bool>.FailureResult("Table Number  already used use another One"
                        , BusinessResult<bool>.enError.Conflict);
               

                switch (Mode)
                {
                    case enMode.AddNew:

                        var result = await _AddNewTable();

                        if (result.Success)
                            Mode = enMode.Update;

                        return result;

                    case enMode.Update:

                        return await _UpdateTable();

                }

                return BusinessResult<bool>.FailureResult("Add/Update Table Operation Failed."
                    , BusinessResult<bool>.enError.ServerError);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Add/Update Table.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Add/Update Table.", ex);
            }

        }



        public static async Task<bool> IsTableIDExist(int? tableID) =>
          await TableDataAccess.IsTableExitst(tableID);


        private static async Task<bool> _IsTableNumberUsed(int id, string number) =>
          await TableDataAccess.IsNumberUsed(id,number);
    }
}
