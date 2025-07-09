using Microsoft.Data.SqlClient;
using Restaurant.DataAccess.DTOs;
using Restaurant.DataAccess.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess
{
    public class OrderItemDataAccess
    {

        public static async Task<AddNewOutputDTO> AddNewOrderItem(int orderID ,OrderItemInputDTO orderItemInputDTO)
        {

            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@order_item_id", SqlDbType.Int)
                    { Direction = ParameterDirection.Output },
                    new SqlParameter("@CurrentTime", SqlDbType.DateTime)
                    { Direction = ParameterDirection.Output },
                    new SqlParameter("@item_id", SqlDbType.Int)
                    { Value = orderItemInputDTO.MenuItemID },
                     new SqlParameter("@order_id", SqlDbType.Int)
                    { Value =  orderID },
                    new SqlParameter("@quantity", SqlDbType.Int)
                    { Value = orderItemInputDTO.Quantity},
                    new SqlParameter("@notes", SqlDbType.NVarChar)
                    { Value = orderItemInputDTO.Notes  == null ? DBNull.Value 
                    : orderItemInputDTO.Notes }
                };



                var outputValues = await SqlHelper.ExecuteWithOutputParametersAsync("SP_AddNewOrderItem", parameters);

                AddNewOutputDTO addNewOrderOutputDTO = new AddNewOutputDTO();

                if (outputValues.TryGetValue("order_item_id", out object ID) && ID != null)
                    addNewOrderOutputDTO.ID = (int)ID;
                else
                    throw new SqlHelper.DataAccessException("Failed to retrieve new Order ID from stored procedure.");



                if (outputValues.TryGetValue("CurrentTime", out object createdAtObj))
                    addNewOrderOutputDTO.CreatedAt = Convert.ToDateTime(createdAtObj);

                else
                    throw new SqlHelper.DataAccessException("Failed to retrieve CreatedAt timestamp for new Order.");


                return addNewOrderOutputDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<List<OrderItemOutputDTO>> GetOrderItemsDetailsByOrderID(int orderID)
        {
            try
            {
                var param = new SqlParameter("@order_id", SqlDbType.Int) { Value = orderID };

                List<OrderItemOutputDTO> orderItemOutputDTOs = new List<OrderItemOutputDTO>();
                await SqlHelper.ExecuteReaderAsync("SP_GetOrderItemsDetailsByOrderID", reader =>
                {
                    while (reader.Read())
                    {
                        orderItemOutputDTOs.Add( MapReaderToOrderDTO(reader));
                    }
                }, param);

                return orderItemOutputDTOs;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> UpdateQuantity(int orderItemID,int newQuantity)
        {

            try
            {

                SqlParameter orderItemIdParam = new SqlParameter("@order_item_id", SqlDbType.Int)
                { Value = orderItemID };
                SqlParameter quantityParam = new SqlParameter("@quantity", SqlDbType.Int)
                { Value = newQuantity };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateOrderItemQuantity", orderItemIdParam, quantityParam);
            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> UpdateOrderItemStatus(int orderItemID,byte status)
        {

            try
            {

                SqlParameter orderItemIdParam = new SqlParameter("@order_item_id", SqlDbType.Int)
                { Value = orderItemID };
                SqlParameter statusParam = new SqlParameter("@status", SqlDbType.TinyInt)
                { Value = status };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateOrderItemStatus", orderItemIdParam, statusParam);
            }
            catch
            {
                throw;
            }
        }

        public static async Task<int> DeleteOrderItem(int orderItemID)
        {

            try
            {

                var idParam = new SqlParameter("@order_item_id", SqlDbType.Int) { Value = orderItemID };

                return await SqlHelper.ExecuteNonQueryAsync("SP_DeleteOrderItem", idParam);

            }
            catch
            {
                throw;
            }
        }

        public static async Task<bool> IsOrderItemExist(int id)
        {

            try
            {

                SqlParameter orderItemIDParam = new SqlParameter("@order_item_id", SqlDbType.Int) { Value = id };
                SqlParameter isExistParam = new SqlParameter("@IsExist", SqlDbType.Bit) { Direction = ParameterDirection.Output };


                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsOrderItemExist"
                    , orderItemIDParam, isExistParam);

            }
            catch
            {
                throw;
            }
        }

        private static OrderItemOutputDTO MapReaderToOrderDTO(IDataReader reader)
        {
            //get ordinal of Optional columns

            return new OrderItemOutputDTO(
                reader.GetInt32(reader.GetOrdinal("order_item_id")),
                reader.GetString(reader.GetOrdinal("name")),
                reader.GetDecimal(reader.GetOrdinal("price_at_order")),
                reader.GetInt32(reader.GetOrdinal("quantity"))
            );
        }


    }
}
