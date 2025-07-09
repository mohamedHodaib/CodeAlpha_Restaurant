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
    public class InventoryDataAccess
    {
        #region Public Methods


        public static async Task<AddNewOutputDTO> AddNewInventory(InventoryDTO inventoryDTO)
        {

            try
            {

                var parameters = new[]
                {
                    new SqlParameter("@inventory_id", SqlDbType.Int) { Direction = ParameterDirection.Output },
                    new SqlParameter("@CurrentTime", SqlDbType.DateTime) { Direction = ParameterDirection.Output },
                    new SqlParameter("@item_name", SqlDbType.NVarChar) { Value = inventoryDTO.ItemName },
                    new SqlParameter("@quantity", SqlDbType.Decimal) { Value = inventoryDTO.Quantity },
                    new SqlParameter("@unit", SqlDbType.NVarChar) { Value = inventoryDTO.Unit },
                    new SqlParameter("@min_stock_level", SqlDbType.Decimal) { Value = inventoryDTO.MinStockLevel },
                    new SqlParameter("@supplier", SqlDbType.NVarChar)
                    { Value = inventoryDTO.Supplier == null ? DBNull.Value : inventoryDTO.Supplier }
                };

                var outputValues = await SqlHelper.ExecuteWithOutputParametersAsync("SP_AddNewInventoryItem", parameters);

                AddNewOutputDTO addNewInventoryOutputDTO = new AddNewOutputDTO();

                if (outputValues.TryGetValue("inventory_id", out object ID) && ID != null)
                    addNewInventoryOutputDTO.ID = (int)ID;
                else
                    throw new SqlHelper.DataAccessException("Failed to retrieve new Inventory ID from stored procedure.");



                if (outputValues.TryGetValue("CurrentTime", out object createdAtObj))
                    addNewInventoryOutputDTO.CreatedAt = Convert.ToDateTime(createdAtObj);

                else
                    throw new SqlHelper.DataAccessException("Failed to retrieve CreatedAt timestamp for new Inventory.");



             

                return addNewInventoryOutputDTO;
            }
            catch
            {
                throw;
            }
        }



        public static async Task<PaginatedDTO<InventoryDTO>> GetAllInventories(int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.NVarChar) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Decimal) { Value = pageSize };
                SqlParameter TotalInventorysParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<InventoryDTO> paginatedInventorysDTO = new PaginatedDTO<InventoryDTO>();

                paginatedInventorysDTO.PageSize = pageSize;
                paginatedInventorysDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedAllInventoryItems", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedInventorysDTO.Items.Add(MapReaderToInventoryDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalInventorysParam);

                paginatedInventorysDTO.TotalItems =
                       TotalInventorysParam.Value == DBNull.Value ? 0 : (int)TotalInventorysParam.Value;

                return paginatedInventorysDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<InventoryDTO>> GetLowStockItems( int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.NVarChar) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Decimal) { Value = pageSize };
                SqlParameter TotalInventorysParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<InventoryDTO> paginatedInventorysDTO = new PaginatedDTO<InventoryDTO>();

                paginatedInventorysDTO.PageSize = pageSize;
                paginatedInventorysDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedInventoryLowStockItems", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedInventorysDTO.Items.Add(MapReaderToInventoryDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalInventorysParam);

                paginatedInventorysDTO.TotalItems =
                       TotalInventorysParam.Value == DBNull.Value ? 0 :
                       (int)TotalInventorysParam.Value;

                return paginatedInventorysDTO;
            }
            catch
            {
                throw;
            }
        }

        public static async Task<PaginatedDTO<InventoryDTO>> GetInventoryItemsBySupplier(string supplier, int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter supplierParam = new SqlParameter("@supplier", SqlDbType.NVarChar) { Value = supplier };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.NVarChar) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Decimal) { Value = pageSize };
                SqlParameter TotalInventorysParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<InventoryDTO> paginatedInventorysDTO = new PaginatedDTO<InventoryDTO>();

                paginatedInventorysDTO.PageSize = pageSize;
                paginatedInventorysDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedInventoryItemsBySupplier", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedInventorysDTO.Items.Add(MapReaderToInventoryDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalInventorysParam, supplierParam);

                paginatedInventorysDTO.TotalItems =
                       TotalInventorysParam.Value == DBNull.Value ? 0 : (int)TotalInventorysParam.Value;

                return paginatedInventorysDTO;
            }
            catch
            {
                throw;
            }
        }



        public static async Task<PaginatedDTO<InventoryDTO>> GetInventoryItemsByUnit(string unit, int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter unitParam = new SqlParameter("@unit", SqlDbType.NVarChar) { Value = unit };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.NVarChar) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Decimal) { Value = pageSize };
                SqlParameter TotalInventorysParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<InventoryDTO> paginatedInventorysDTO = new PaginatedDTO<InventoryDTO>();

                paginatedInventorysDTO.PageSize = pageSize;
                paginatedInventorysDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedInventoryItemsByUnit", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedInventorysDTO.Items.Add(MapReaderToInventoryDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalInventorysParam, unitParam);

                paginatedInventorysDTO.TotalItems =
                       TotalInventorysParam.Value == DBNull.Value ? 0 : (int)TotalInventorysParam.Value;

                return paginatedInventorysDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<InventoryDTO>> SearchInventoryItemsByName(string name, int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter nameParam = new SqlParameter("@item_name", SqlDbType.NVarChar) { Value = name };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.NVarChar) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Decimal) { Value = pageSize };
                SqlParameter TotalInventorysParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<InventoryDTO> paginatedInventorysDTO = new PaginatedDTO<InventoryDTO>();

                paginatedInventorysDTO.PageSize = pageSize;
                paginatedInventorysDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_SearchPaginatedInventoryItemsByName", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedInventorysDTO.Items.Add(MapReaderToInventoryDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalInventorysParam, nameParam);

                paginatedInventorysDTO.TotalItems =
                       TotalInventorysParam.Value == DBNull.Value ? 0 : (int)TotalInventorysParam.Value;

                return paginatedInventorysDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<InventoryDTO> GetInventoryItemByID(int id)
        {
            try
            {
                var param = new SqlParameter("@inventory_id", SqlDbType.Int) { Value = id };

                InventoryDTO inventoryDTO = new InventoryDTO();
                await SqlHelper.ExecuteReaderAsync("SP_GetInventoryByID", reader =>
                {
                    if (reader.Read())
                    {
                        inventoryDTO = MapReaderToInventoryDTO(reader, id);
                    }
                }, param);

                return inventoryDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<InventoryDTO> GetInventoryItemByName(string name)
        {
            try
            {
                var userNameParam = new SqlParameter("@item_name", SqlDbType.NVarChar) { Value = name };

                InventoryDTO inventoryDTO = new InventoryDTO();
                await SqlHelper.ExecuteReaderAsync("SP_GetInventoryItemByName", reader =>
                {
                    if (reader.Read())
                    {
                        inventoryDTO = MapReaderToInventoryDTO(reader);
                    }
                }, userNameParam);

                return inventoryDTO;
            }
            catch
            {
                throw;
            }
        }


		public static async Task<bool> IsInventoryRecordExist(int id)
		{

			try
			{

				SqlParameter inventoryIdParam = new SqlParameter("@inventory_id", SqlDbType.Int) { Value = id };
                SqlParameter isExistParam = new SqlParameter("@IsExist", SqlDbType.Bit) { Direction = ParameterDirection.Output };


                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsInventoryRecordExist"
                    ,inventoryIdParam,isExistParam);

			}
			catch
			{
				throw;
			}
		}

		public static async Task<int> UpdateQuantity(int id, decimal newQuantity)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@inventory_id", SqlDbType.Int) { Value = id };
                SqlParameter quantityParam = new SqlParameter("@quantity", SqlDbType.NVarChar) 
                { Value = newQuantity };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateQuantity", idParam, quantityParam);
            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> UpdateMinStockLevel(int id, decimal newMinStockLevel)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@inventory_id", SqlDbType.Int) { Value = id };
                SqlParameter minStockLeverParam = new SqlParameter("@min_stock_level", SqlDbType.NVarChar)
                { Value = newMinStockLevel };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateMinStockLevel", idParam, minStockLeverParam);
            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> UpdateSupplier(int id, string supplier)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@inventory_id", SqlDbType.Int) { Value = id };
                SqlParameter supplierParam = new SqlParameter("@supplier", SqlDbType.NVarChar)
                { Value = supplier };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateQuantity", idParam, supplierParam);
            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> UpdateUnit(int id, string newUnit)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@inventory_id", SqlDbType.Int) { Value = id };
                SqlParameter unitParam = new SqlParameter("@unit", SqlDbType.NVarChar)
                { Value = newUnit };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateUnit", idParam, unitParam);
            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> UpdateInventoryItem(InventoryDTO inventoryDTO)
        {

            try
            {

                var parameters = new[]
                 {
                    new SqlParameter("@inventory_id", SqlDbType.Int) { Value = inventoryDTO.ID },
                    new SqlParameter("@quantity", SqlDbType.Decimal) { Value = inventoryDTO.Quantity },
                    new SqlParameter("@unit", SqlDbType.NVarChar) { Value = inventoryDTO.Unit },
                    new SqlParameter("@min_stock_level", SqlDbType.Decimal) { Value = inventoryDTO.MinStockLevel },
                    new SqlParameter("@supplier", SqlDbType.NVarChar)
                    { Value = inventoryDTO.Supplier == null ? DBNull.Value : inventoryDTO.Supplier }
                };


                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateInventoryItem", parameters);
            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> DeleteInventory(int id)
        {

            try
            {

                var idParam = new SqlParameter("@inventory_id", SqlDbType.Int) { Value = id };

                return await SqlHelper.ExecuteNonQueryAsync("SP_DeleteInventoryItem", idParam);

            }
            catch
            {
                throw;
            }
        }



        public static async Task<bool> IsInventoryItemNameUsed(int id,  string name)
        {

            try
            {
                SqlParameter idParam = new SqlParameter("@inventory_id", SqlDbType.Int) { Value = id };
                SqlParameter itemNameParam = new SqlParameter("@item_name", SqlDbType.NVarChar) { Value = name };
                SqlParameter isUsedParam = new SqlParameter("@IsUsed", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsInventoryItemNameUsed", idParam, itemNameParam,isUsedParam);

            }
            catch
            {
                throw;
            }
        }




        #endregion

        #region Private Helper Methods



        private static InventoryDTO MapReaderToInventoryDTO(IDataReader reader, int? id = null)
        {
            //get ordinal of Optional columns
            int supplierOrdinal = reader.GetOrdinal("supplier");
            int lastRestockDateOrdinal = reader.GetOrdinal("last_restock_date");

            return new InventoryDTO(
                id ?? reader.GetInt32(reader.GetOrdinal("inventory_id")),
                reader.GetString(reader.GetOrdinal("item_name")),
                reader.GetDecimal(reader.GetOrdinal("quantity")),
                reader.GetString(reader.GetOrdinal("unit")),
                reader.GetDecimal(reader.GetOrdinal("min_stock_level")),
                reader.IsDBNull(supplierOrdinal) ? null : reader.GetString(supplierOrdinal),
                reader.IsDBNull(lastRestockDateOrdinal) ?  null :
                DateOnly.FromDateTime(reader.GetDateTime(lastRestockDateOrdinal))
                );
        }

        #endregion
    }
}
