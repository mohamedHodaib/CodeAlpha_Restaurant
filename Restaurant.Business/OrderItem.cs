using Restaurant.DataAccess.DTOs.Order;
using Restaurant.DataAccess.DTOs;
using Restaurant.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business
{
    public class OrderItem
    {
        public static async Task<BusinessResult<bool>> AddNewItemToOrder(int orderID,OrderItemInputDTO orderItemInputDTO)
        {
            try
            {
                if (!await OrderDataAccess.IsOrderExist(orderID))
                    return BusinessResult<bool>.FailureResult("Order  Is Not Exist.",
                        BusinessResult<bool>.enError.BadRequest);

                if (await OrderDataAccess.IsOrderServed(orderID))
                    return BusinessResult<bool>.FailureResult("Served Orders Cannot Be Updated.",
                        BusinessResult<bool>.enError.BadRequest);

                if (await OrderDataAccess.IsOrderPaid(orderID))
                    return BusinessResult<bool>.FailureResult("Paid Orders Cannot Be Updated.",
                        BusinessResult<bool>.enError.BadRequest);

                if (await OrderDataAccess.IsOrderCancelled(orderID))
                    return BusinessResult<bool>.FailureResult("Cancelled Orders Cannot Be Updated."
                        , BusinessResult<bool>.enError.BadRequest);

                AddNewOutputDTO addNewOutputDTO =
                    await OrderItemDataAccess.AddNewOrderItem(orderID, orderItemInputDTO);

                int newOrderItemID = addNewOutputDTO.ID;

                if (newOrderItemID > 0)
                    return BusinessResult<bool>.SuccessResult("Order Item Record  Created Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Order Item Record Creation Fail",
                        BusinessResult<bool>.enError.ServerError);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Add New Order Item .", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Add New Order Item.", ex);
            }
        }


        public static async Task<BusinessResult<bool>> UpdateOrderItemQuantity(int orderID,int orderItemID,int newQuantity)
        {
            
                try
                {
                         if (!await OrderDataAccess.IsOrderExist(orderID))
                             return BusinessResult<bool>.FailureResult("Order  Is Not Exist.",
                                 BusinessResult<bool>.enError.BadRequest);

                         if (!await OrderItemDataAccess.IsOrderItemExist(orderItemID))
                             return BusinessResult<bool>.FailureResult("Order Item Is Not Exist.");

                         if (await OrderDataAccess.IsOrderServed(orderID))
                             return BusinessResult<bool>.FailureResult("Served Orders Cannot Be Updated.",
                                 BusinessResult<bool>.enError.BadRequest);

                         if (await OrderDataAccess.IsOrderPaid(orderID))
                             return BusinessResult<bool>.FailureResult("Paid Orders Cannot Be Updated.",
                                 BusinessResult<bool>.enError.BadRequest);

                         if (await OrderDataAccess.IsOrderCancelled(orderID))
                             return BusinessResult<bool>.FailureResult("Cancelled Orders Cannot Be Updated."
                                 , BusinessResult<bool>.enError.BadRequest);

               

                            int rowsAffected = await OrderItemDataAccess.UpdateQuantity(orderItemID, newQuantity);

                        if (rowsAffected > 0)
                            return BusinessResult<bool>.SuccessResult("Order Item Record Updated Successfully.");

                        else
                            return BusinessResult<bool>.FailureResult("Update Order Item Record Fail.",
                                BusinessResult<bool>.enError.ServerError);

                }
                catch (SqlHelper.DataAccessException ex)
                {
                    throw new ApplicationException("A database error occurred during Update Order  .", ex);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("A unexpected error occurred during Update Order  .", ex);
                }
           
        }
        

        private static async Task<List<OrderItemOutputDTO>> GetOrderItems(int orderID)
        {
            var result =  await OrderItemDataAccess.GetOrderItemsDetailsByOrderID(orderID);

            return result;
        }


        internal static async Task<BusinessResult<bool>> InventoryRollBack(int  orderID)
        {

            try
            {
                List<OrderItemOutputDTO> items = await GetOrderItems(orderID);

                if (items.Count == 0)
                    return BusinessResult<bool>.FailureResult("Inventory RollBackFail");

                foreach (var item in items)
                {
                    var restult = await MenuItemIngrediant.GetMenuItemIngrediantsByMenuItemName(item.ItemName);

                    if (!restult.Success)
                        return BusinessResult<bool>.FailureResult("Inventory RollBackFail");

                    foreach (var ingrediant in restult.Data)
                    {
                        return await Inventory.UpdateQuantity(ingrediant.InventoryID, item.Quantity * ingrediant.QuantityUsed);
                    }
                }

                return BusinessResult<bool>.SuccessResult("Inventory RollBack Successfully");

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Inventory RollBack  .", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Inventory RollBack.", ex);
            }

        }

        public static async Task<BusinessResult<bool>> DeleteOrderItem(int orderID, int orderItemID)
        {

            try
            {
                if (!await OrderDataAccess.IsOrderExist(orderID))
                    return BusinessResult<bool>.FailureResult("Order  Is Not Exist.",
                        BusinessResult<bool>.enError.BadRequest);

                if (!await OrderItemDataAccess.IsOrderItemExist(orderItemID))
                    return BusinessResult<bool>.FailureResult("Order Item Is Not Exist.");

                if (await OrderDataAccess.IsOrderServed(orderID))
                    return BusinessResult<bool>.FailureResult("Served Orders Cannot Be Updated.",
                        BusinessResult<bool>.enError.BadRequest);

                if (await OrderDataAccess.IsOrderPaid(orderID))
                    return BusinessResult<bool>.FailureResult("Paid Orders Cannot Be Updated.",
                        BusinessResult<bool>.enError.BadRequest);

                if (await OrderDataAccess.IsOrderCancelled(orderID))
                    return BusinessResult<bool>.FailureResult("Cancelled Orders Cannot Be Updated."
                        , BusinessResult<bool>.enError.BadRequest);


                int rowsAffected = await OrderItemDataAccess.DeleteOrderItem (orderItemID);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Order Item Record Deleted Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Deleted Order  Record Fail."
                        ,BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Order  .", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Order  .", ex);
            }

        }

    }
}
