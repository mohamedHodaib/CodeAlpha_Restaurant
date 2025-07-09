using Restaurant.DataAccess.DTOs;
using Restaurant.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Restaurant.Business
{
    public class Inventory
    {
        public enum enMode { AddNew, Update };
        public enMode Mode = enMode.AddNew;


        public InventoryDTO inventoryDTO => new InventoryDTO(ID,ItemName,Quantity
            ,Unit,MinStockLevel,Supplier,LastRestockDate);



        public int ID { get; set; }
        public string ItemName { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public decimal MinStockLevel { get; set; }
        public string? Supplier { get; set; }
        public DateOnly? LastRestockDate { get; set; }

        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? UpdatedAt { get; set; } = null;

        public Inventory(InventoryDTO inventoryDTO, enMode mode = enMode.AddNew)
        {

            ID = inventoryDTO.ID;
            ItemName = inventoryDTO.ItemName;
            Quantity = inventoryDTO.Quantity;
            Unit = inventoryDTO.Unit;
            MinStockLevel = inventoryDTO.MinStockLevel;
            Supplier = inventoryDTO.Supplier;
            LastRestockDate = inventoryDTO.LastRestockDate;

            Mode = mode;
        }


        private async Task<BusinessResult<bool>> _AddNewInventoryItem()
        {
            try
            {

                AddNewOutputDTO addNewOutputDTO = await InventoryDataAccess.AddNewInventory(inventoryDTO);

                ID = addNewOutputDTO.ID;
                CreatedAt = addNewOutputDTO.CreatedAt;
                UpdatedAt = addNewOutputDTO.CreatedAt;

                if (ID > 0)
                    return BusinessResult<bool>.SuccessResult("Inventory Item  Created Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Inventory Item  Creation Fail"
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

        public static async Task<BusinessResult<PaginatedDTO<InventoryDTO>>> GetAllInventories( int pageNumber, int pageSize)
        {
            try
            {

               

                PaginatedDTO<InventoryDTO> PaginatedInventories =
                    await InventoryDataAccess.GetAllInventories(pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<InventoryDTO>>
                    .SuccessResult("Inventory Records Returned Successfully.", PaginatedInventories);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Inventory Items.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Inventory Items.", ex);
            }

        }


        public static async Task<BusinessResult<PaginatedDTO<InventoryDTO>>> GetLowStockItems(int pageNumber, int pageSize)
        {
            try
            {

                PaginatedDTO<InventoryDTO> PaginatedInventories =
                    await InventoryDataAccess.GetLowStockItems(pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<InventoryDTO>>
                    .SuccessResult("Inventory Records Returned Successfully.", PaginatedInventories);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Low Stock Items", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Low Stock Items.", ex);
            }

        }


        public static async Task<BusinessResult<PaginatedDTO<InventoryDTO>>> GetInventoryItemsBySupplier(string supplier,int pageNumber, int pageSize)
        {
            try
            {



                PaginatedDTO<InventoryDTO> PaginatedInventories =
                    await InventoryDataAccess.GetInventoryItemsBySupplier(supplier,pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<InventoryDTO>>
                    .SuccessResult("Inventory Records Returned Successfully.", PaginatedInventories);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Inventory Items By Supplier", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Inventory Items By Supplier.", ex);
            }

        }


        public static async Task<BusinessResult<PaginatedDTO<InventoryDTO>>> GetInventoryItemsByUnit(string unit, int pageNumber, int pageSize)
        {
            try
            {



                PaginatedDTO<InventoryDTO> PaginatedInventories =
                    await InventoryDataAccess.GetInventoryItemsByUnit(unit, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<InventoryDTO>>
                    .SuccessResult("Inventory Records Returned Successfully.", PaginatedInventories);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Inventory Items By Unit", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Inventory Items By Unit.", ex);
            }

        }


        public static async Task<BusinessResult<PaginatedDTO<InventoryDTO>>> SearchInventoryItemsByName(string name, int pageNumber, int pageSize)
        {
            try
            {



                PaginatedDTO<InventoryDTO> PaginatedInventories =
                    await InventoryDataAccess.SearchInventoryItemsByName(name, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<InventoryDTO>>
                    .SuccessResult("Inventory Records Returned Successfully.", PaginatedInventories);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Search Inventory Items By Name", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Search Inventory Items By Name.", ex);
            }

        }


        public static async Task<BusinessResult<Inventory>> Find(int inventoryID)
        {
            try
            {

                InventoryDTO inventoryDTO = await InventoryDataAccess.GetInventoryItemByID(inventoryID);    

                if (inventoryDTO.ID != 0)
                    return BusinessResult<Inventory>.SuccessResult("Inventory Item  Found successfully",
                        new Inventory(inventoryDTO, enMode.Update));

                else
                    return BusinessResult<Inventory>.FailureResult($"Inventory Item with ID {inventoryID} Not Found"
                        ,BusinessResult<Inventory>.enError.NotFound);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Inventory Item  By  inventory ID.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Inventory Item  By  inventory ID.", ex);
            }

        }


        public static async Task<BusinessResult<Inventory>> Find(string name)
        {
            try
            {
                InventoryDTO inventoryDTO = await InventoryDataAccess.GetInventoryItemByName(name);

                if (inventoryDTO.ID != 0)
                    return BusinessResult<Inventory>.SuccessResult("Inventory Item  Found successfully",
                        new Inventory(inventoryDTO, enMode.Update));

                else
                    return BusinessResult<Inventory>.FailureResult($"Inventory Item with name {name} Not Found"
                        , BusinessResult<Inventory>.enError.NotFound);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Inventory Item  By  Name.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Inventory Item  By  Name.", ex);
            }

        }

        public static async Task<BusinessResult<bool>> UpdateQuantity(int id, decimal newQuantity)
        {

            try
            {
                if(!await _IsInventoryIDExist(id))
                    return BusinessResult<bool>.FailureResult("Inventory Item not Exist."
                        , BusinessResult<bool>.enError.NotFound);

                int rowsAffected = await InventoryDataAccess.UpdateQuantity(id,newQuantity);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Inventory Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Inventory Record Fail."
                        ,BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Inventory Quantity.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Inventory Quantity.", ex);
            }
        }


        public static async Task<BusinessResult<bool>> UpdateMinStockLevel(int id, decimal newMinStockLevel)
        {

            try
            {
                if (!await _IsInventoryIDExist(id))
                    return BusinessResult<bool>.FailureResult("Inventory Item not Exist."
                        , BusinessResult<bool>.enError.NotFound);

                int rowsAffected = await InventoryDataAccess.UpdateQuantity(id, newMinStockLevel);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Inventory Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Inventory Record Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Inventory Item Min Stock Level.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Inventory Item Min Stock Level.", ex);
            }
        }


        public static async Task<BusinessResult<bool>> UpdateSupplier(int id, string supplier)
        {

            try
            {
                if (!await _IsInventoryIDExist(id))
                    return BusinessResult<bool>.FailureResult("Inventory Item not Exist."
                        , BusinessResult<bool>.enError.NotFound);

                int rowsAffected = await InventoryDataAccess.UpdateSupplier(id, supplier);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Inventory Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Inventory Record Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Inventory Item Supplier.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Inventory Item Supplier", ex);
            }
        }


        public static async Task<BusinessResult<bool>> UpdateUnit(int id, string newUnit)
        {

            try
            {
                if (!await _IsInventoryIDExist(id))
                    return BusinessResult<bool>.FailureResult("Inventory Item not Exist."
                        , BusinessResult<bool>.enError.NotFound);

                int rowsAffected = await InventoryDataAccess.UpdateUnit(id, newUnit);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Inventory Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Inventory Record Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Inventory Item Unit.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Inventory Item Unit.", ex);
            }
        }



        private async Task<BusinessResult<bool>> _UpdateInventory()
        {
            try
            {
                int rowsAffected = await InventoryDataAccess.UpdateInventoryItem(inventoryDTO);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Inventory Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Inventory Record Fail."
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


        public async Task<BusinessResult<bool>> Delete()
        {
            try
            {

                int rowsAffected =
                    await InventoryDataAccess.DeleteInventory(ID);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Inventory Item Deleted Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Delete Inventory Item Ingrediant Fail."
                        , BusinessResult<bool>.enError.ServerError);


            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return BusinessResult<bool>.FailureResult("Inventory Item Cannot Deleted because has Related Data."
                        , BusinessResult<bool>.enError.Conflict);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Deleting Menu Item Ingrediant.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Deleting Menu Item Ingrediant.", ex);
            }

        }

        public async Task<BusinessResult<bool>> Save()
        {
            try
            {

                if (await InventoryDataAccess.IsInventoryItemNameUsed(ID,ItemName))
                    return BusinessResult<bool>.FailureResult
                                ("Inventory Name Already Used ,please enter another one."
                                , BusinessResult<bool>.enError.Conflict);


                switch (Mode)
                {
                    case enMode.AddNew:

                        var result = await _AddNewInventoryItem();

                        if (result.Success)
                            Mode = enMode.Update;

                        return result;

                    case enMode.Update:

                        return await _UpdateInventory();

                }

                return BusinessResult<bool>.FailureResult("Add/Update Inventory Operation Failed."
                    , BusinessResult<bool>.enError.ServerError);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Add/Update Inventory.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Add/Update Inventory.", ex);
            }

        }



        private static async Task<bool> _IsInventoryIDExist(int inventoryID) =>
          await InventoryDataAccess.IsInventoryRecordExist(inventoryID);

    }
}
