using Restaurant.DataAccess.DTOs;
using Restaurant.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business
{
    public class MenuItemIngrediant


    {
        public enum enMode { AddNew, Update };
        public enMode Mode = enMode.AddNew;


        public MenueItemIngrediantDTO menueItemIngrediantDTO => new MenueItemIngrediantDTO(IngrediantID, ItemID
            , InventoryID, QuantityUsed , Unit);



        public int IngrediantID { get; set; }
        public int ItemID { get; set; }
        public int InventoryID { get; set; }
        public decimal QuantityUsed { get; set; }
        public string Unit { get; set; }

        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? UpdatedAt { get; set; } = null;

        public MenuItemIngrediant(MenueItemIngrediantDTO menueItemDTO, enMode mode = enMode.AddNew)
        {
            
            IngrediantID = menueItemDTO.IngrediantID;
            ItemID = menueItemDTO.ItemID;
            InventoryID = menueItemDTO.InventoryID;
            QuantityUsed = menueItemDTO.QuantityUsed;
            Unit = menueItemDTO.Unit;

            Mode = mode;
        }


        private async Task<BusinessResult<bool>> _AddNewMenuItemIngrediant()
        {
            try
            {

                AddNewOutputDTO addNewOutputDTO = await MenueItemIngrediantDataAccess.AddNewMenueItemIngrediant(menueItemIngrediantDTO);

                    IngrediantID = addNewOutputDTO.ID;
                    CreatedAt = addNewOutputDTO.CreatedAt;
                    UpdatedAt = addNewOutputDTO.CreatedAt;

                if (IngrediantID > 0)
                    return BusinessResult<bool>.SuccessResult("Menu Item Ingrediant Created Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Menu Item Ingrediant Creation Fail"
                        , BusinessResult<bool>.enError.ServerError);
            }
            catch (SqlHelper.DataAccessException )
            {
                throw;
            }
            catch (Exception )
            {
                throw;
            }
        }

        public static async Task<BusinessResult<List<MenueItemIngrediantDTO>>> GetMenuItemIngrediantsByMenuItemID(int menuItemID)
        {
            try
            {
               
                if (!await _IsMenuItemIDExist(menuItemID))
                    return BusinessResult<List<MenueItemIngrediantDTO>>.FailureResult
                            ($"Menu Item with ID {menuItemID} not Exist,please enter another one."
                            ,BusinessResult<List<MenueItemIngrediantDTO>>.enError.BadRequest);

                List<MenueItemIngrediantDTO> MenuItemIgrediants =
                    await MenueItemIngrediantDataAccess.GetIngradientsByMenueItemID(menuItemID);

                return BusinessResult<List<MenueItemIngrediantDTO>>
                    .SuccessResult("Igrediants Returned Successfully.", MenuItemIgrediants);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Menu Item Ingrediants By Menu Item ID.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Menu Item Ingrediants By Menu Item ID.", ex);
            }

        }


        public static async Task<BusinessResult<List<MenueItemIngrediantDTO>>> GetMenuItemIngrediantsByMenuItemName(string ItemName)
        {
            try
            {

                

                List<MenueItemIngrediantDTO> MenuItemIgrediants =
                    await MenueItemIngrediantDataAccess.GetIngradientsByMenueItemName(ItemName);

                return BusinessResult<List<MenueItemIngrediantDTO>>
                    .SuccessResult("Igrediants Returned Successfully.", MenuItemIgrediants);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Menu Item Ingrediants By Menu Item ID.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Menu Item Ingrediants By Menu Item ID.", ex);
            }

        }

        public static async Task<BusinessResult<MenueItemIngrediantReportDTO>> GetMenuItemIngrediantByInventoryID(int inventoryID)
        {
            try
            {

                if (!await _IsInventoryIDExist(inventoryID))
                    return BusinessResult<MenueItemIngrediantReportDTO>.FailureResult
                            ($"inventory with ID {inventoryID} not Exist,please enter another one."
                            ,BusinessResult<MenueItemIngrediantReportDTO>.enError.BadRequest);

                MenueItemIngrediantReportDTO menuItemIgrediantReportDTO =
                    await MenueItemIngrediantDataAccess.GetMenueItemIngrediantByInventoryID(inventoryID);

                return BusinessResult<MenueItemIngrediantReportDTO>
                    .SuccessResult("Igrediants Returned Successfully.", menuItemIgrediantReportDTO);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Menu Item Ingrediants By Inventory ID.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Menu Item Ingrediants By Inventory ID.", ex);
            }

        }
        public static async Task<BusinessResult<MenuItemIngrediant>> Find(int id)
        {
            try
            {

                MenueItemIngrediantDTO menueItemIngrediantDTO = await MenueItemIngrediantDataAccess.GetMenueItemIngradientByIngradientID(id);

                if (menueItemIngrediantDTO.IngrediantID != 0)
                    return BusinessResult<MenuItemIngrediant>.SuccessResult("Menu Item Ingrediant Found successfully",
                        new MenuItemIngrediant(menueItemIngrediantDTO, enMode.Update));

                else
                    return BusinessResult<MenuItemIngrediant>.FailureResult($"Menu Item Ingrediant with ID {id} Not Found"
                        ,BusinessResult<MenuItemIngrediant>.enError.NotFound);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Menu Item Ingrediant By Ingrediant ID.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Menu Item Ingrediant By Ingrediant ID.", ex);
            }

        }


        private async Task<BusinessResult<bool>> _UpdateMenuItemIngrediant()
        {
            try
            {
                int rowsAffected = await MenueItemIngrediantDataAccess.Update(menueItemIngrediantDTO);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Menu Item Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Menu Item Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException )
            {
                throw;
            }
            catch (Exception )
            {
                throw;
            }
        }


        public async Task<BusinessResult<bool>> Delete()
        {
            try
            {

                int rowsAffected = 
                    await MenueItemIngrediantDataAccess.DeleteMenuItemIngredientByID(IngrediantID);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Menu Item Ingrediant Deleted Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Delete Menu Item Ingrediant Fail."
                        , BusinessResult<bool>.enError.ServerError);

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
                if(!await _IsMenuItemIDExist(ItemID))
                    return BusinessResult<bool>.FailureResult
                            ($"Menu Item with ID {ItemID} not Exist,please enter another one."
                            ,BusinessResult<bool>.enError.BadRequest);

                if (!await _IsInventoryIDExist(InventoryID))
                    return BusinessResult<bool>.FailureResult
                                ($"Inventory with ID {InventoryID} not Exist ,please enter another one."
                                ,BusinessResult<bool>.enError.BadRequest);


                if (await MenueItemIngrediantDataAccess.IsInventoryIDUsed(IngrediantID, InventoryID))
                    return BusinessResult<bool>.FailureResult
                                ("Inventory ID Already Linked to Ingrediant ,please enter another one."
                                ,BusinessResult<bool>.enError.Conflict);


                switch (Mode)
                {
                    case enMode.AddNew:

                        var result = await _AddNewMenuItemIngrediant();

                        if (result.Success)
                            Mode = enMode.Update;

                        return result;

                    case enMode.Update:

                        return await _UpdateMenuItemIngrediant();

                }

                return BusinessResult<bool>.FailureResult("Add/Update Menu Item Ingrediant Operation Failed."
                    ,BusinessResult<bool>.enError.ServerError);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Add/Update Menu Item Ingrediant.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Add/Update Menu Item Ingrediant.", ex);
            }

        }


        private static  async Task<bool> _IsMenuItemIDExist (int menuItemID) =>
           await MenueItemDataAccess.IsMenuItemExist(menuItemID);


        private static async Task<bool> _IsInventoryIDExist(int inventoryID) =>
          await InventoryDataAccess.IsInventoryRecordExist(inventoryID);

    }
}
