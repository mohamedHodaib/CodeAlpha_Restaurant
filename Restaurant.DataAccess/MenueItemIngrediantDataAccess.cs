using Microsoft.Data.SqlClient;
using Restaurant.DataAccess.DTOs;
using System.Data;

namespace Restaurant.DataAccess
{
    public class MenueItemIngrediantDataAccess
    {
        #region Public Methods


        public static async Task<AddNewOutputDTO> AddNewMenueItemIngrediant(MenueItemIngrediantDTO menueItemIngrediantDTO)
        {

            try
            {

                var parameters = new[]
                {
                    new SqlParameter("@ingrediant_id", SqlDbType.Int) { Direction = ParameterDirection.Output },
                    new SqlParameter("@CurrentTime", SqlDbType.DateTime) { Direction = ParameterDirection.Output },

                    new SqlParameter("@item_id", SqlDbType.Int) { Value = menueItemIngrediantDTO.ItemID },
                    new SqlParameter("@inventory_id", SqlDbType.Int) { Value = menueItemIngrediantDTO.InventoryID },
                    new SqlParameter("@quantity_used", SqlDbType.Decimal) { Value = menueItemIngrediantDTO.QuantityUsed },
                    new SqlParameter("@unit", SqlDbType.NVarChar) { Value = menueItemIngrediantDTO.Unit },
                };

                var outputValues = await SqlHelper.ExecuteWithOutputParametersAsync("SP_AddNewMenueItemIngrediant", parameters);

                AddNewOutputDTO addNewItemIngrediantOutputDTO = new AddNewOutputDTO();

                if (outputValues.TryGetValue("ingrediant_id", out object ID) && ID != null)
                    addNewItemIngrediantOutputDTO.ID = (int)ID;
                else
                    throw new SqlHelper.DataAccessException("Failed to retrieve new Menue Item Ingredient ID from stored procedure.");



                if (outputValues.TryGetValue("CurrentTime", out object createdAtObj))
                    addNewItemIngrediantOutputDTO.CreatedAt = Convert.ToDateTime(createdAtObj);

                else
                    throw new SqlHelper.DataAccessException("Failed to retrieve CreatedAt timestamp for new MenuItem.");



                return addNewItemIngrediantOutputDTO;
            }
            catch
            {
                throw;
            }
        }



        public static async Task<List<MenueItemIngrediantDTO>> GetIngradientsByMenueItemID(int menueItemID)
        {
            try
            {
                var menueItemIDParam =  new SqlParameter("@item_id", SqlDbType.Int) { Value = menueItemID };



                List<MenueItemIngrediantDTO> MenuItemIngredientDTOs = new ();

            

                await SqlHelper.ExecuteReaderAsync("SP_GetIngradientsByMenueItemID", reader =>
                {
                    while (reader.Read())
                    {
                        MenuItemIngredientDTOs.Add(MapReaderToMenueItemIngrediant(reader));
                    }
                }, menueItemIDParam);

                

                return MenuItemIngredientDTOs;
            }
            catch
            {
                throw;
            }
        }

        public static async Task<List<MenueItemIngrediantDTO>> GetIngradientsByMenueItemName(string menuItemName)
        {
            try
            {
                var menueItemNameParam = new SqlParameter("@menu_item_name", SqlDbType.NVarChar) { Value = menuItemName };



                List<MenueItemIngrediantDTO> MenuItemIngredientDTOs = new();



                await SqlHelper.ExecuteReaderAsync("SP_GetIngradientsByMenueItemName", reader =>
                {
                    while (reader.Read())
                    {
                        MenuItemIngredientDTOs.Add(MapReaderToMenueItemIngrediant(reader));
                    }
                }, menueItemNameParam);



                return MenuItemIngredientDTOs;
            }
            catch
            {
                throw;
            }
        }




        public static async Task<MenueItemIngrediantReportDTO> GetMenueItemIngrediantByInventoryID(int inventoryID)
        {
            try
            {
                var inventoryIDParam = new SqlParameter("@inventory_id", SqlDbType.Int) { Value = inventoryID };



                MenueItemIngrediantReportDTO paginatedMenuItemIngredientDTO = new MenueItemIngrediantReportDTO();

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedMenueItemIngrediantByInventoryID", reader =>
                {
                    if (reader.Read())
                    {
                        paginatedMenuItemIngredientDTO = MapReaderToMenueItemIngrediantReport(reader);
                    }
                } ,inventoryIDParam);

               

                return paginatedMenuItemIngredientDTO;
            }
            catch
            {
                throw;
            }
        }



        public static async Task<MenueItemIngrediantDTO> GetMenueItemIngradientByIngradientID(int id)
        {
            try
            {
                var param = new SqlParameter("@menue_item_ingradient_id", SqlDbType.Int) { Value = id };

                MenueItemIngrediantDTO menueItemIngrediantDTO = null;
                await SqlHelper.ExecuteReaderAsync("SP_GetMenueItemIngradientByIngradientID", reader =>
                {
                    if (reader.Read())
                    {
                        menueItemIngrediantDTO = MapReaderToMenueItemIngrediant(reader, id);
                    }
                }, param);

                return menueItemIngrediantDTO;
            }
            catch
            {
                throw;
            }
        }



        public static async Task<bool> IsInventoryIDUsed(int ingrediantID,int inventoryID)
        {

            try
            {

                SqlParameter inventoryIDParam = new SqlParameter("@inventory_id", SqlDbType.Int) { Value = inventoryID };
                SqlParameter ingrediantIDParam = new SqlParameter("@ingrediant_id", SqlDbType.Int) { Value = ingrediantID };
                SqlParameter isUsedParam = new SqlParameter("@IsUsed", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsInventoryIDUsed",
                    ingrediantIDParam, inventoryIDParam,isUsedParam);

            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> Update(MenueItemIngrediantDTO menueItemIngredientDTO)
        {

            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@item_id", SqlDbType.Int) { Value = menueItemIngredientDTO.ItemID },
                    new SqlParameter("@quantity_used", SqlDbType.Decimal) 
                    { Value = menueItemIngredientDTO.QuantityUsed },
                    new SqlParameter("@unit", SqlDbType.NVarChar) { Value = menueItemIngredientDTO.Unit },
                  
                };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateMenueItemIngredientByItemID", parameters);

            }
            catch
            {
                throw;
            }
        }



        public static async Task<int> DeleteMenuItemIngredientByID(int id)
        {

            try
            {

                var ingredientIDParam = new SqlParameter("@ingredient_id", SqlDbType.Int) { Value = id };

                return await SqlHelper.ExecuteNonQueryAsync("SP_DeleteMenuItemIngredientByID", ingredientIDParam);

            }
            catch
            {
                throw;
            }
        }


        #endregion

        #region Private Helper Methods

     


        private static MenueItemIngrediantReportDTO MapReaderToMenueItemIngrediantReport(IDataReader reader, int? id = null)
        {
            return new MenueItemIngrediantReportDTO(
                id ?? reader.GetInt32(reader.GetOrdinal("menu_item_ingredient_id")),
                reader.GetString(reader.GetOrdinal("menue_item_name")),
                reader.GetString(reader.GetOrdinal("menue_item_ingrediant_name")),
                reader.GetDecimal(reader.GetOrdinal("quantity_used")),
                reader.GetString(reader.GetOrdinal("unit"))
                );
        }


        private static MenueItemIngrediantDTO MapReaderToMenueItemIngrediant(IDataReader reader, int? id = null)
        {
            return new MenueItemIngrediantDTO(
                id ?? reader.GetInt32(reader.GetOrdinal("menu_item_ingredient_id")),
                reader.GetInt32(reader.GetOrdinal("item_id")),
                reader.GetInt32(reader.GetOrdinal("inventory_id")),
                reader.GetDecimal(reader.GetOrdinal("quantity_used")),
                reader.GetString(reader.GetOrdinal("unit"))
               );
        }

        #endregion
    }
}
