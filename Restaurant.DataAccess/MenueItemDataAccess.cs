using Microsoft.Data.SqlClient;
using Restaurant.DataAccess.DTOs;
using System.Data;

namespace Restaurant.DataAccess
{
    public class MenueItemDataAccess
    {
        #region Public Methods


        public static async Task<AddNewOutputDTO> AddNewMenueItem(MenueItemDTO menueItem)
        {
            
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@item_id", SqlDbType.Int) { Direction = ParameterDirection.Output },
                    new SqlParameter("@CurrentTime", SqlDbType.DateTime) { Direction = ParameterDirection.Output },

                    new SqlParameter("@name", SqlDbType.NVarChar) { Value = menueItem.Name },
                    new SqlParameter("@price", SqlDbType.Decimal) { Value = menueItem.Price },
                    new SqlParameter("@description", SqlDbType.NVarChar) { Value = menueItem.Description },
                    new SqlParameter("@category", SqlDbType.NVarChar) { Value = menueItem.Category },
                    new SqlParameter("@is_available", SqlDbType.Bit) { Value = menueItem.IsAvailable }
                };

                var outputValues = await SqlHelper.ExecuteWithOutputParametersAsync("SP_AddNewMenueItem", parameters);

                    AddNewOutputDTO addNewItemOutputDTO = new AddNewOutputDTO();

                if (outputValues.TryGetValue("item_id", out object ID) && ID != null)
                    addNewItemOutputDTO.ID = (int)ID;
                else
                    throw new SqlHelper.DataAccessException("Failed to retrieve new MenuItem ID from stored procedure.");



                if (outputValues.TryGetValue("CurrentTime", out object createdAtObj))
                    addNewItemOutputDTO.CreatedAt = Convert.ToDateTime(createdAtObj);

                else
                    throw new SqlHelper.DataAccessException("Failed to retrieve CreatedAt timestamp for new MenuItem.");



                return addNewItemOutputDTO;
            }
            catch
            {
                throw;
            }
        }



        public static async Task<PaginatedDTO<MenueItemDTO>> GetMenueItems(int pageNumber,int pageSize)
        {
            try
            {

                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.NVarChar) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Decimal) { Value = pageSize };
                SqlParameter TotalItemsParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };
                

                PaginatedDTO<MenueItemDTO> paginatedMenuItemsDTO = new PaginatedDTO<MenueItemDTO>();

                paginatedMenuItemsDTO.PageSize = pageSize;
                paginatedMenuItemsDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedMenueItems", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedMenuItemsDTO.Items.Add( MapReaderToMenueItem(reader));
                    }
                },pageNumberParam,pageSizeParam,TotalItemsParam);

             paginatedMenuItemsDTO.TotalItems =
                    TotalItemsParam.Value == DBNull.Value ? 0 : (int)TotalItemsParam.Value;

                return paginatedMenuItemsDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<MenueItemDTO> GetMenueItemByID(int id)
        {
           

            try
            {
                var param = new SqlParameter("@item_id", SqlDbType.Int) { Value = id };

                MenueItemDTO menueItemDTO = null;
                await SqlHelper.ExecuteReaderAsync("SP_GetMenueItemByID", reader =>
                {
                    if (reader.Read())
                    {
                        menueItemDTO = MapReaderToMenueItem(reader, id);
                    }
                }, param);

                return menueItemDTO;
            }
            catch
            {
                throw;
            }
        }

        public static async Task<PaginatedDTO<MenueItemDTO>> GetMenueItemsByCategory(string category, int pageNumber, int pageSize)
        {
            try
            {
                var categoryParam = new SqlParameter("@category", SqlDbType.NVarChar) { Value = category };

                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.NVarChar) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Decimal) { Value = pageSize };
                SqlParameter TotalItemsParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<MenueItemDTO> paginatedMenuItemsDTO = new PaginatedDTO<MenueItemDTO>();

                paginatedMenuItemsDTO.PageSize = pageSize;
                paginatedMenuItemsDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedMenueItemsByCategory", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedMenuItemsDTO.Items.Add(MapReaderToMenueItem(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalItemsParam, categoryParam);

                paginatedMenuItemsDTO.TotalItems =
                       TotalItemsParam.Value == DBNull.Value ? 0 : (int)TotalItemsParam.Value;

                return paginatedMenuItemsDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<MenueItemDTO>> GetAvailableMenuItems(int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.NVarChar) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Decimal) { Value = pageSize };
                SqlParameter TotalItemsParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<MenueItemDTO> paginatedMenuItemsDTO = new PaginatedDTO<MenueItemDTO>();

                paginatedMenuItemsDTO.PageSize = pageSize;
                paginatedMenuItemsDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedAvailableMenuItems", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedMenuItemsDTO.Items.Add(MapReaderToMenueItem(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalItemsParam);

                paginatedMenuItemsDTO.TotalItems =
                       TotalItemsParam.Value == DBNull.Value ? 0 : (int)TotalItemsParam.Value;

                return paginatedMenuItemsDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<MenueItemDTO>> GetUnAvailableMenuItems(int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.NVarChar) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Decimal) { Value = pageSize };
                SqlParameter TotalItemsParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<MenueItemDTO> paginatedMenuItemsDTO = new PaginatedDTO<MenueItemDTO>();

                paginatedMenuItemsDTO.PageSize = pageSize;
                paginatedMenuItemsDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedUnAvailableMenuItems", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedMenuItemsDTO.Items.Add(MapReaderToMenueItem(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalItemsParam);

                paginatedMenuItemsDTO.TotalItems =
                       TotalItemsParam.Value == DBNull.Value ? 0 : (int)TotalItemsParam.Value;

                return paginatedMenuItemsDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<MenueItemDTO> GetMenueItemByName(string name)
        {
                try
                {
                    var nameParam = new SqlParameter("@name", SqlDbType.NVarChar) { Value = name };

                    MenueItemDTO menueItemDTO = null;
                    await SqlHelper.ExecuteReaderAsync("SP_GetMenueItemByName", reader =>
                    {
                        if (reader.Read())
                        {
                            menueItemDTO = MapReaderToMenueItem(reader);
                        }
                    }, nameParam);

                    return menueItemDTO;
                }
                catch
                {
                    throw;
                }
            }


        public static async Task<PaginatedDTO<MenueItemDTO>> GetMenueItemsByDescription(string description, int pageSize, int pageNumber)
        {
            try
            {
                var desParam = new SqlParameter("@description", SqlDbType.NVarChar) { Value = description };

                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.NVarChar) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Decimal) { Value = pageSize };
                SqlParameter TotalItemsParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<MenueItemDTO> paginatedMenuItemsDTO = new PaginatedDTO<MenueItemDTO>();

                paginatedMenuItemsDTO.PageSize = pageSize;
                paginatedMenuItemsDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedMenueItemsByDescription", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedMenuItemsDTO.Items.Add(MapReaderToMenueItem(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalItemsParam, desParam);

                paginatedMenuItemsDTO.TotalItems =
                       TotalItemsParam.Value == DBNull.Value ? 0 : (int)TotalItemsParam.Value;

                return paginatedMenuItemsDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<bool> IsMenuItemExist(int id)
        {

            try
            {

                SqlParameter itemIDParam = new SqlParameter("@item_id", SqlDbType.Int) { Value = id };
                SqlParameter isExistParam = new SqlParameter("@IsExist", SqlDbType.Bit) { Direction = ParameterDirection.Output };


                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsMenuItemExist", itemIDParam, isExistParam);

            }
            catch
            {
                throw;
            }
        }

        public static async Task<bool> IsMenuItemExistAndAvailable(int id)
        {

            try
            {

                SqlParameter itemIDParam = new SqlParameter("@item_id", SqlDbType.Int) { Value = id };
                SqlParameter isExistParam = new SqlParameter("@IsExist", SqlDbType.Bit) { Direction = ParameterDirection.Output };


                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsMenuItemExistAndAvailable", itemIDParam, isExistParam);

            }
            catch
            {
                throw;
            }
        }


        public static async Task<bool> IsMenuItemNameUsed(int id, string name)
        {

            try
            {
                SqlParameter idParam = new SqlParameter("@item_id", SqlDbType.Int) { Value = id };
                SqlParameter itemNameParam = new SqlParameter("@name", SqlDbType.NVarChar) { Value = name };
                SqlParameter isUsedParam = new SqlParameter("@IsUsed", SqlDbType.Bit) { Direction = ParameterDirection.Output };


                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsMenuItemNameUsed"
                    ,idParam, itemNameParam,isUsedParam);

            }
            catch
            {
                throw;
            }
        }

        public static async Task<int> Update(MenueItemDTO menueItem)
        {

            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@item_id", SqlDbType.Int) { Value = menueItem.ID },
                    new SqlParameter("@name", SqlDbType.NVarChar) { Value = menueItem.Name },
                    new SqlParameter("@price", SqlDbType.Decimal) { Value = menueItem.Price },
                    new SqlParameter("@description", SqlDbType.NVarChar) { Value = menueItem.Description },
                    new SqlParameter("@category", SqlDbType.NVarChar) { Value = menueItem.Category },
                    new SqlParameter("@is_available", SqlDbType.Bit) { Value = menueItem.IsAvailable }
                };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateMenueItemByID", parameters);
                
            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> Update(int id, bool isAvailable)
        {

            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@item_id", SqlDbType.Int) { Value = id },
                    new SqlParameter("@is_available", SqlDbType.Bit) { Value = isAvailable }
                };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateMenueItemAvailabilityStatusByID", parameters);
            }
            catch
            {
                throw;
            }
        }

       
        public static async Task<int> Delete (int id)
        {

            try
            {

                var itemIDParam = new SqlParameter("@item_id", SqlDbType.Int) { Value = id };
               
                return await SqlHelper.ExecuteNonQueryAsync("SP_DeleteMenueItemByID", itemIDParam);

            }
            catch
            {
                throw;
            }
        }
      

        #endregion

        #region Private Helper Methods

        
        private static MenueItemDTO MapReaderToMenueItem(IDataReader reader, int? id = null)
        {
            int descriptionOrdinal = reader.GetOrdinal("description");
            return new MenueItemDTO(
                id ?? reader.GetInt32(reader.GetOrdinal("item_id")),
                reader.GetString(reader.GetOrdinal("name")),
                reader.IsDBNull(descriptionOrdinal) ? null : reader.GetString(descriptionOrdinal),
                reader.GetDecimal(reader.GetOrdinal("price")),
                reader.GetString(reader.GetOrdinal("category")),
                reader.GetBoolean(reader.GetOrdinal("is_available"))
                );
        }

        #endregion
    }
}
