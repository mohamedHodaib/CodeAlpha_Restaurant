using Restaurant.DataAccess.DTOs;
using Restaurant.DataAccess.DTOs.Order;
using Restaurant.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Restaurant.DataAccess.DTOs.Sales;
using System.Data;
using static Restaurant.DataAccess.DTOs.Order.OrderDTO;
using System.Transactions;
using System.Linq.Expressions;

namespace Restaurant.Business
{
    public class Order

    {
        public enum enMode { AddNew, Update };
        public enMode Mode = enMode.AddNew;


        public OrderDetailsInputDTO orderDetailsDTO => new OrderDetailsInputDTO(OrderID,CustomerID,TableID,StaffID
            ,TotalAmount,Status,PaymentStatus,PaymentMethod,Notes,Items);


        public int OrderID { get; set; }
        public int? CustomerID { get; set; }
        public int? TableID { get; set; }
        public int? StaffID { get; set; }
        public decimal TotalAmount { get; set; }
        public enOrderStatus Status { get; set; }
        public bool PaymentStatus { get; set; }
        public enPaymentMethod? PaymentMethod { get; set; }
        public string? Notes { get; set; }

        public List<OrderItemInputDTO> Items { get; set; }
        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? UpdatedAt { get; set; } = null;

        public Order(OrderDetailsInputDTO orderDTO, enMode mode = enMode.AddNew)
        {

            OrderID = orderDTO.OrderID;
            CustomerID = orderDTO.CustomerID;
            TableID = orderDTO.TableID;
            StaffID = orderDTO.StaffID;

            if(mode == enMode.AddNew)
            {
                Status = enOrderStatus.Pending;
                PaymentStatus = false;
                PaymentMethod = null;
            }
            else
            {
                Status = orderDTO.Status;
                PaymentStatus = orderDTO.PaymentStatus;
                PaymentMethod = orderDTO.PaymentMethod;

            }
            Notes = orderDTO.Notes;
            Items = orderDTO.Items;

            Mode = mode;
        }


        private async Task<BusinessResult<bool>> _AddNewOrder()
        {
            try
            {

                AddNewOutputDTO addNewOutputDTO =
                    await OrderDataAccess.AddNewOrder(orderDetailsDTO);

                OrderID = addNewOutputDTO.ID;
                CreatedAt = addNewOutputDTO.CreatedAt;
                UpdatedAt = addNewOutputDTO.CreatedAt;

                if (OrderID > 0)
                    return BusinessResult<bool>.SuccessResult("Order Record  Created Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Order Record Creation Fail"
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


        public  async Task<BusinessResult<bool>> Pay(PaymentInfoDTO paymentInfoDTO)
        {
            try
            {
                if(await _IsOrderIDExist(paymentInfoDTO.OrderID))
                    return BusinessResult<bool>.FailureResult("Order Not Exist."
                        , BusinessResult<bool>.enError.BadRequest);

                if (await OrderDataAccess.IsOrderCancelled(paymentInfoDTO.OrderID))
                    return BusinessResult<bool>.FailureResult("Cancelled Orders Cannot Be Updated."
                        , BusinessResult<bool>.enError.BadRequest);

                if (await OrderDataAccess.IsOrderPaid(paymentInfoDTO.OrderID))
                    return BusinessResult<bool>.FailureResult("Paid Orders Cannot Be Updated."
                        , BusinessResult<bool>.enError.BadRequest);


                if(!await OrderDataAccess.IsAmountSufficient(paymentInfoDTO.OrderID, paymentInfoDTO.amount))
                    return BusinessResult<bool>.FailureResult("Amount is not sufficient for Order."
                        , BusinessResult<bool>.enError.BadRequest);

                // payment Actions called Here 

                return await AfterPaymentUpdates();


            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Payment.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Payment", ex);
            }
        }


        private  async Task<BusinessResult<bool>> AfterPaymentUpdates()
        {
                try
                {
                    int rowsAffected = await OrderDataAccess.AfterPaymentUpdates(OrderID, PaymentMethod);

                    if (rowsAffected > 0)
                        return BusinessResult<bool>.SuccessResult("After Payment Updates Occured Successfully.");

                    else
                        return BusinessResult<bool>.FailureResult("After Payment Updates Failed."
                            , BusinessResult<bool>.enError.ServerError);

                }
                catch (SqlHelper.DataAccessException ex)
                {
                    throw new ApplicationException("A database error occurred during Updates After Payment Payment.", ex);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("A unexpected error occurred during Updates After Payment", ex);
                }
            
        }

        public static async Task<BusinessResult<OrderDTO>> Find(int id)
        {
            try
            {
                OrderDTO orderDTO = await OrderDataAccess.GetOrderByID(id);

                if (orderDTO.OrderID != 0)
                    return BusinessResult<OrderDTO>.SuccessResult("Order  Found successfully",
                        orderDTO);

                else
                    return BusinessResult<OrderDTO>.FailureResult($"Order with ID {id} Not Found"
                        ,BusinessResult<OrderDTO>.enError.NotFound);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Order By ID.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Order By ID.", ex);
            }

        }


        public static async Task<BusinessResult<PaginatedDTO<OrderDTO>>> GetAllOrders(int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<OrderDTO> PaginatedOrders =
                    await OrderDataAccess.GetAllOrders(pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<OrderDTO>>
                    .SuccessResult("Order Records Returned Successfully.", PaginatedOrders);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Orders.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Orders.", ex);
            }

        }


        public static async Task<BusinessResult<PaginatedDTO<OrderDTO>>> GetOrdersByCustomerID(int customerID, int pageNumber, int pageSize)
        {
            try
            {
                if (!await Customer.IsCustomerIDExist(customerID))
                    return BusinessResult<PaginatedDTO<OrderDTO>>.FailureResult("Customer with id Not Exist."
                        , BusinessResult<PaginatedDTO<OrderDTO>>.enError.BadRequest);

                PaginatedDTO<OrderDTO> PaginatedOrders =
                    await OrderDataAccess.GetOrdersByCustomerID(customerID, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<OrderDTO>>
                    .SuccessResult("Order Records Returned Successfully.", PaginatedOrders);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Orders.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Orders.", ex);
            }
        }

        public static async Task<BusinessResult<PaginatedDTO<OrderDTO>>> GetOrdersByTableID(int tableID, int pageNumber, int pageSize)
        {
            try
            {
                if (!await Table.IsTableIDExist(tableID))
                    return BusinessResult<PaginatedDTO<OrderDTO>>.FailureResult("Table is not exist"
                        ,BusinessResult<PaginatedDTO<OrderDTO>>.enError.BadRequest);

                PaginatedDTO<OrderDTO> PaginatedOrders =
                    await OrderDataAccess.GetOrdersByTableID(tableID, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<OrderDTO>>
                    .SuccessResult("Order Records Returned Successfully.", PaginatedOrders);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Orders.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Orders.", ex);
            }
        }


        public static async Task<BusinessResult<PaginatedDTO<OrderDTO>>> GetOrdersByStatus(enOrderStatus status, int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<OrderDTO> PaginatedOrders =
                    await OrderDataAccess.GetOrdersByStatus(status, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<OrderDTO>>
                    .SuccessResult("Order Records Returned Successfully.", PaginatedOrders);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Orders.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Orders.", ex);
            }
        }



        public static async Task<BusinessResult<PaginatedDTO<OrderDTO>>> GetOrdersWithinPeriod(DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<OrderDTO> PaginatedOrders =
                    await OrderDataAccess.GetOrdersWithinPeriod(startDate, endDate, pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<OrderDTO>>
                    .SuccessResult("Order Records Returned Successfully.", PaginatedOrders);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Orders.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Orders.", ex);
            }
        }

        public static async Task<BusinessResult<PaginatedDTO<OrderDTO>>> GetActiveOrders(int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<OrderDTO> PaginatedOrders =
                    await OrderDataAccess.GetActiveOrders( pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<OrderDTO>>
                    .SuccessResult("Order Records Returned Successfully.", PaginatedOrders);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Orders.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Orders.", ex);
            }
        }


        public static async Task<BusinessResult<PaginatedDTO<OrderDailySalesDTO>>> GetDailyOrderSalesWithinPeriod( DateOnly startDate, DateOnly endDate,int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<OrderDailySalesDTO> PaginatedOrders =
                    await OrderDataAccess.GetDailyOrderSalesWithinPeriod(pageNumber, pageSize,startDate,endDate);

                return BusinessResult<PaginatedDTO<OrderDailySalesDTO>>
                    .SuccessResult("Order Records Returned Successfully.", PaginatedOrders);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Order Sales.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Order Sales.", ex);
            }
        }


        public static async Task<BusinessResult<PaginatedDTO<OrderWeeklySalesDTO>>> GetWeeklyOrderSalesWithinPeriod(DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<OrderWeeklySalesDTO> PaginatedOrders =
                    await OrderDataAccess.GetWeeklyOrderSalesWithinPeriod(pageNumber, pageSize, startDate, endDate);

                return BusinessResult<PaginatedDTO<OrderWeeklySalesDTO>>
                    .SuccessResult("Order Records Returned Successfully.", PaginatedOrders);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Order Sales.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Order Sales.", ex);
            }
        }


        public static async Task<BusinessResult<PaginatedDTO<OrderMonthlySalesDTO>>> GetMonthlyOrderSalesWithinPeriod(DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<OrderMonthlySalesDTO> PaginatedOrders =
                    await OrderDataAccess.GetMonthlyOrderSalesWithinPeriod(pageNumber, pageSize, startDate, endDate);

                return BusinessResult<PaginatedDTO<OrderMonthlySalesDTO>>
                    .SuccessResult("Order Records Returned Successfully.", PaginatedOrders);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Order Sales.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Order Sales.", ex);
            }
        }


        public static async Task<BusinessResult<PaginatedDTO<OrderYearlySalesDTO>>> GetYearlyOrderSalesWithinPeriod(DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<OrderYearlySalesDTO> PaginatedOrders =
                    await OrderDataAccess.GetYearlyOrderSalesWithinPeriod(pageNumber, pageSize, startDate, endDate);

                return BusinessResult<PaginatedDTO<OrderYearlySalesDTO>>
                    .SuccessResult("Order Records Returned Successfully.", PaginatedOrders);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Order Sales.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Order Sales.", ex);
            }
        }


        public static async Task<BusinessResult<PaginatedDTO<StaffOrderSalesDTO>>> GetStaffMemberOrderSalesWithinPeriod(DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<StaffOrderSalesDTO> PaginatedOrders =
                    await OrderDataAccess.GetStaffMemberOrderSalesWithinPeriod(pageNumber, pageSize, startDate, endDate);

                return BusinessResult<PaginatedDTO<StaffOrderSalesDTO>>
                    .SuccessResult("Order Records Returned Successfully.", PaginatedOrders);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Order Sales.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Order Sales.", ex);
            }
        }


        public static async Task<BusinessResult<PaginatedDTO<OrderItemSalesDTO>>> GetItemSalesWithinPeriod(DateOnly startDate, DateOnly endDate,
            int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<OrderItemSalesDTO> PaginatedOrders =
                    await OrderDataAccess.GetItemSalesWithinPeriod(pageNumber, pageSize, startDate, endDate);

                return BusinessResult<PaginatedDTO<OrderItemSalesDTO>>
                    .SuccessResult("Order Records Returned Successfully.", PaginatedOrders);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Order Sales.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Order Sales.", ex);
            }
        }



        public static async Task<BusinessResult<OrderDetailsOuputDTO>> GetOrderDetailsByID(int id)
        {
            try
            {
                OrderDetailsOuputDTO orderDTO = await OrderDataAccess.GetOrderDetailsByID(id);

                if (orderDTO.OrderID != 0)
                    return BusinessResult<OrderDetailsOuputDTO>.SuccessResult("Order  Found successfully",
                        orderDTO);

                else
                    return BusinessResult<OrderDetailsOuputDTO>.FailureResult($"Order with ID {id} Not Found"
                        ,BusinessResult<OrderDetailsOuputDTO>.enError.NotFound);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Order By ID.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Order By ID.", ex);
            }

        }


        private static async Task<BusinessResult<bool>> UpdateStatus( int orderID,enOrderStatus status)
        {
            try
            {
                

                int rowsAffected = await OrderDataAccess.UpdateStatus(orderID, status);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Order Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Order Record Fail."
                        ,BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Order Status.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Order Status .", ex);
            }
        }


        public static async Task<BusinessResult<bool>> UpdatePaymentStatus(int ID, bool paymentStatus)
        {
            try
            {
                if (!await _IsOrderIDExist(ID))
                    return BusinessResult<bool>.FailureResult("Order is not exist",
                        BusinessResult<bool>.enError.BadRequest);

                int rowsAffected = await OrderDataAccess.UpdateOrderPaymentStatus(ID, paymentStatus);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Order Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Order Record Fail."
                        ,BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Order .", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Order  .", ex);
            }
        }

        public static async Task<BusinessResult<bool>> Cancel(int orderID)
        {
            try
            {
                if (!await _IsOrderIDExist(orderID))
                    return BusinessResult<bool>.FailureResult("Order is not exist",
                        BusinessResult<bool>.enError.BadRequest);

                if (await OrderDataAccess.IsOrderServed(orderID))
                    return BusinessResult<bool>.FailureResult("Served Orders Cannot Be Updated.",
                        BusinessResult<bool>.enError.BadRequest);

                if (await OrderDataAccess.IsOrderPaid(orderID))
                    return BusinessResult<bool>.FailureResult("Paid Orders Cannot Be Updated."
                        , BusinessResult<bool>.enError.BadRequest);

                BusinessResult<bool> result =  await UpdateStatus(orderID,enOrderStatus.Cancelled);

               BusinessResult<OrderDTO> order = await Find(orderID);

                if(result.Success)
                {
                    int? tableID = order.Data.TableID;

                    if (tableID != null)
                    {
                        if (!(await UpdateTableAfterCancel(tableID)).Success)
                            return await CancelFailureOutput();
                    }


                    if ((await RollBackItemsToInventory(orderID)).Success)
                        return BusinessResult<bool>.SuccessResult("Cancel Operation Succeeded.");
                    else
                        return await CancelFailureOutput();

                }

                else
                    return await CancelFailureOutput();

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Order .", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Order  .", ex);
            }
        }


        private static async Task<BusinessResult<bool>> UpdateTableAfterCancel(int? tableID)
        {
            var result = await Table.UpdateTableStatus(tableID, enTableStatus.Available);

            return result;
        }


        private static async Task<BusinessResult<bool>> RollBackItemsToInventory(int orderID)
        {
            
            var result = await OrderItem.InventoryRollBack(orderID);

            return result;
        }

        private static  async Task<BusinessResult<bool>> CancelFailureOutput() =>
              BusinessResult<bool>.FailureResult("Cancel Order Record Fail."
                  ,BusinessResult<bool>.enError.ServerError);

       


        public static async Task<BusinessResult<bool>> UpdatePaymentAmount(int ID, decimal paymentAmount)
        {
            try
            {
                if (!await _IsOrderIDExist(ID))
                    return BusinessResult<bool>.FailureResult("Order is not exist"
                        , BusinessResult<bool>.enError.BadRequest);

                int rowsAffected = await OrderDataAccess.UpdateOrderPaymentAmount(ID, paymentAmount);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Order Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Order Record Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Order .", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Order  .", ex);
            }
        }


        public static async Task<BusinessResult<bool>> UpdatePaymentNotes(int ID, string paymentNotes)
        {
            try
            {
                if (!await _IsOrderIDExist(ID))
                    return BusinessResult<bool>.FailureResult("Order is not exist"
                        , BusinessResult<bool>.enError.BadRequest);

                int rowsAffected = await OrderDataAccess.UpdateOrderNotes(ID, paymentNotes);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Order Record Updated Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Update Order Record Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Order .", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Order  .", ex);
            }
        }


        private static async Task<bool> IsActive(int orderID) =>
                !await OrderDataAccess.IsOrderServed(orderID) &&
                !await OrderDataAccess.IsOrderPaid(orderID) &&
                !await OrderDataAccess.IsOrderCancelled(orderID);


        public static async Task<BusinessResult<bool>> Delete(int ID)
        {
            try
            {

                if (!await IsActive(ID))
                    return BusinessResult<bool>.FailureResult("Cannot Delete Active Orders."
                        , BusinessResult<bool>.enError.BadRequest);

                int rowsAffected =
                    await OrderDataAccess.DeleteOrder(ID);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Order Deleted Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Delete Order Ingrediant Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Deleting Order.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Deleting Order.", ex);
            }

        }

       

        public async Task<BusinessResult<bool>> Save()
        {
            try
            {
				if (StaffID != null && !await StaffMemberDataAccess.IsStaffMemberExitst(StaffID.Value) )
					return BusinessResult<bool>.FailureResult
						("The staff Member is not exist  ."
						, BusinessResult<bool>.enError.BadRequest);

				if (StaffID != null && !await StaffMemberDataAccess.IsActive(StaffID.Value))
					return BusinessResult<bool>.FailureResult
						("The staff Member is not Active ."
						, BusinessResult<bool>.enError.BadRequest);

				foreach (var item in Items)
                {
                    if (!await MenueItemDataAccess.IsMenuItemExist(item.MenuItemID))
                        return BusinessResult<bool>.FailureResult
                            ("There is An Item in the order List that Is Not Available Or Not Exist."
                            ,BusinessResult<bool>.enError.BadRequest);
                }
               

                switch (Mode)
                {
                    case enMode.AddNew:

                        var result = await _AddNewOrder();

                        if (result.Success)
                            Mode = enMode.Update;

                        return result;

                    case enMode.Update:

                        return BusinessResult<bool>.SuccessResult("Order Updated Successfully.");

                }

                return BusinessResult<bool>.FailureResult("Add/Update Order Operation Failed."
                    , BusinessResult<bool>.enError.ServerError);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Add/Update Order.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Add/Update Order.", ex);
            }

        }



        private static async Task<bool> _IsOrderIDExist(int id) =>
          await OrderDataAccess.IsOrderExist(id);

    }
}
