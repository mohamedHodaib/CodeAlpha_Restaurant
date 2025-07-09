using Microsoft.Data.SqlClient;
using Restaurant.DataAccess.DTOs;
using Restaurant.DataAccess.DTOs.Order;
using Restaurant.DataAccess.DTOs.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess
{
    public class OrderDataAccess
    {
        #region Public Methods


        /// <summary>
        /// Converts a list of OrderItemInputDto objects into a DataTable
        /// suitable for passing as a Table-Valued Parameter.
        /// </summary>
        /// <param name="items">List of order item DTOs.</param>
        /// <returns>A DataTable matching the dbo.OrderItemType schema.</returns>
        private static DataTable ConvertOrderItemsToDataTable(List<OrderItemInputDTO> items)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("menue_item_id", typeof(int));
            dt.Columns.Add("quantity", typeof(int));
            dt.Columns.Add("notes", typeof(string));

            foreach (var item in items)
            {
                DataRow row = dt.NewRow();
                row["menue_item_id"] = item.MenuItemID;
                row["quantity"] = item.Quantity;
                row["notes"] = item.Notes ?? (object)DBNull.Value; // Handle nullable string
                dt.Rows.Add(row);
            }
            return dt;
        }

        public static async Task<AddNewOutputDTO> AddNewOrder(OrderDetailsInputDTO orderDTO)
        {

            try
            {

               DataTable orderItemsDataTable =  ConvertOrderItemsToDataTable(orderDTO.Items);

                var parameters = new[]
                {
                    new SqlParameter("@order_id", SqlDbType.Int)
                    { Direction = ParameterDirection.Output },
                    new SqlParameter("@CurrentTime", SqlDbType.DateTime)
                    { Direction = ParameterDirection.Output },
                    new SqlParameter("@customer_id", SqlDbType.Int)
                    { Value = orderDTO.CustomerID == null ? DBNull.Value : orderDTO.CustomerID },
                     new SqlParameter("@table_id", SqlDbType.Int)
                    { Value =  orderDTO.TableID == null ? DBNull.Value : orderDTO.TableID },
                    new SqlParameter("@staff_id", SqlDbType.Int) 
                    { Value = orderDTO.StaffID == null ? DBNull.Value : orderDTO.StaffID },
                    new SqlParameter("@notes", SqlDbType.NVarChar)
                    { Value = orderDTO.Notes == null ? DBNull.Value : orderDTO.Notes },
                    new SqlParameter("@order_items",SqlDbType.Structured)
                    {
                        TypeName = "dbo.OrderItemInputType",
                        Value = orderItemsDataTable
                    }
                };



                var outputValues = await SqlHelper.ExecuteWithOutputParametersAsync("SP_AddNewOrder", parameters);

                AddNewOutputDTO addNewOrderOutputDTO = new AddNewOutputDTO();

                if (outputValues.TryGetValue("order_id", out object ID) && ID != null)
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



        public static async Task<PaginatedDTO<OrderDTO>> GetAllOrders(int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalOrdersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<OrderDTO> paginatedOrdersDTO = new PaginatedDTO<OrderDTO>();

                paginatedOrdersDTO.PageSize = pageSize;
                paginatedOrdersDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedAllOrders", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedOrdersDTO.Items.Add(MapReaderToOrderDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalOrdersParam);

                paginatedOrdersDTO.TotalItems =
                       TotalOrdersParam.Value == DBNull.Value ? 0 : (int)TotalOrdersParam.Value;

                return paginatedOrdersDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<OrderDTO>> GetOrdersByCustomerID(int customerID, int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter customerIDParam = new SqlParameter("@customer_id", SqlDbType.Int) { Value = customerID };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalOrdersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<OrderDTO> paginatedOrdersDTO = new PaginatedDTO<OrderDTO>();

                paginatedOrdersDTO.PageSize = pageSize;
                paginatedOrdersDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedOrdersByCustomerID", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedOrdersDTO.Items.Add(MapReaderToOrderDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalOrdersParam, customerIDParam);

                paginatedOrdersDTO.TotalItems =
                       TotalOrdersParam.Value == DBNull.Value ? 0 : (int)TotalOrdersParam.Value;

                return paginatedOrdersDTO;
            }
            catch
            {
                throw;
            }
        }

        public static async Task<PaginatedDTO<OrderDTO>> GetOrdersByTableID(int tableID, int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter tableIDParam = new SqlParameter("@table_id", SqlDbType.Int) { Value = tableID };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalOrdersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<OrderDTO> paginatedOrdersDTO = new PaginatedDTO<OrderDTO>();

                paginatedOrdersDTO.PageSize = pageSize;
                paginatedOrdersDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedOrdersByTableID", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedOrdersDTO.Items.Add(MapReaderToOrderDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalOrdersParam, tableIDParam);

                paginatedOrdersDTO.TotalItems =
                       TotalOrdersParam.Value == DBNull.Value ? 0 : (int)TotalOrdersParam.Value;

                return paginatedOrdersDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<OrderDTO>> GetOrdersByStaffID(int staffID, int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter staffIDParam = new SqlParameter("@staff_id", SqlDbType.Int) { Value = staffID };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalOrdersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<OrderDTO> paginatedOrdersDTO = new PaginatedDTO<OrderDTO>();

                paginatedOrdersDTO.PageSize = pageSize;
                paginatedOrdersDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedOrdersByStaffID", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedOrdersDTO.Items.Add(MapReaderToOrderDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalOrdersParam, staffIDParam);

                paginatedOrdersDTO.TotalItems =
                       TotalOrdersParam.Value == DBNull.Value ? 0 : (int)TotalOrdersParam.Value;

                return paginatedOrdersDTO;
            }
            catch
            {
                throw;
            }
        }




        public static async Task<PaginatedDTO<OrderDTO>> GetOrdersByStatus(enOrderStatus status, int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter statusParam = new SqlParameter("@status", SqlDbType.TinyInt) { Value = status };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalOrdersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<OrderDTO> paginatedOrdersDTO = new PaginatedDTO<OrderDTO>();

                paginatedOrdersDTO.PageSize = pageSize;
                paginatedOrdersDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedOrdersByStatus", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedOrdersDTO.Items.Add(MapReaderToOrderDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalOrdersParam, statusParam);

                paginatedOrdersDTO.TotalItems =
                       TotalOrdersParam.Value == DBNull.Value ? 0 : (int)TotalOrdersParam.Value;

                return paginatedOrdersDTO;
            }
            catch
            {
                throw;
            }
        }



        public static async Task<PaginatedDTO<OrderDTO>> GetOrdersByPaymentStatus(byte paymentStatus, int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter paymentStatusParam = new SqlParameter("@payment_status", SqlDbType.TinyInt) { Value = paymentStatus };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalOrdersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<OrderDTO> paginatedOrdersDTO = new PaginatedDTO<OrderDTO>();

                paginatedOrdersDTO.PageSize = pageSize;
                paginatedOrdersDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedOrdersByPaymentStatus", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedOrdersDTO.Items.Add(MapReaderToOrderDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalOrdersParam, paymentStatusParam);

                paginatedOrdersDTO.TotalItems =
                       TotalOrdersParam.Value == DBNull.Value ? 0 : (int)TotalOrdersParam.Value;

                return paginatedOrdersDTO;
            }
            catch
            {
                throw;
            }
        }



        public static async Task<PaginatedDTO<OrderDTO>> GetOrdersWithinPeriod
            (DateOnly startDate, DateOnly endDate , int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter startDateParam = new SqlParameter("@StartDate", SqlDbType.Date) { Value = startDate };
                SqlParameter endDateParam = new SqlParameter("@EndDate", SqlDbType.Date) { Value = endDate };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalOrdersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<OrderDTO> paginatedOrdersDTO = new PaginatedDTO<OrderDTO>();

                paginatedOrdersDTO.PageSize = pageSize;
                paginatedOrdersDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedOrdersWithinPeriod", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedOrdersDTO.Items.Add(MapReaderToOrderDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam,
                TotalOrdersParam, startDateParam, endDateParam);

                paginatedOrdersDTO.TotalItems =
                       TotalOrdersParam.Value == DBNull.Value ? 0 : (int)TotalOrdersParam.Value;

                return paginatedOrdersDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<OrderDTO>> GetActiveOrders( int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalOrdersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<OrderDTO> paginatedOrdersDTO = new PaginatedDTO<OrderDTO>();

                paginatedOrdersDTO.PageSize = pageSize;
                paginatedOrdersDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync("SP_GetPaginatedPendingOrders", reader =>
                {
                    while (reader.Read())
                    {
                        paginatedOrdersDTO.Items.Add(MapReaderToOrderDTO(reader));
                    }
                }, pageNumberParam, pageSizeParam, TotalOrdersParam);

                paginatedOrdersDTO.TotalItems =
                       TotalOrdersParam.Value == DBNull.Value ? 0 : (int)TotalOrdersParam.Value;

                return paginatedOrdersDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<T>> GetSalesWithinPeriod<T>(string storedProcedure,int pageNumber, int pageSize
            ,DateOnly startDate, DateOnly endDate,Func<IDataReader,T> CallBack)
        {
            try
            {
                SqlParameter startDateParam = new SqlParameter("@StartDate", SqlDbType.Date) { Value = startDate };
                SqlParameter endDateParam = new SqlParameter("@EndDate", SqlDbType.Date) { Value = endDate };
                SqlParameter pageNumberParam = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
                SqlParameter pageSizeParam = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
                SqlParameter TotalOrdersParam = new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };


                PaginatedDTO<T> paginatedSalesDTO = new PaginatedDTO<T>();

                paginatedSalesDTO.PageSize = pageSize;
                paginatedSalesDTO.CurrentPage = pageNumber;

                await SqlHelper.ExecuteReaderAsync(storedProcedure, reader =>
                {
                    while (reader.Read())
                    {
                        paginatedSalesDTO.Items.Add(CallBack(reader));
                    }
                },startDateParam,endDateParam ,pageNumberParam, pageSizeParam, TotalOrdersParam);

                paginatedSalesDTO.TotalItems =
                       TotalOrdersParam.Value == DBNull.Value ? 0 : (int)TotalOrdersParam.Value;

                return paginatedSalesDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<OrderDailySalesDTO>> GetDailyOrderSalesWithinPeriod(int pageNumber, int pageSize, DateOnly startDate, DateOnly endDate)
        {
            try
            {
                return await GetSalesWithinPeriod<OrderDailySalesDTO>
                    ("SP_GetDailyOrderSales",pageNumber, pageSize, startDate, endDate, row =>
                {
                   return new OrderDailySalesDTO 
                   {
                        Year = row.GetInt32(row.GetOrdinal("SalesYear"))
                        ,Month = row.GetInt32(row.GetOrdinal("SalesMonth"))
                        ,Week = row.GetInt32(row.GetOrdinal("SalesWeek"))
                        ,Day = row.GetInt32(row.GetOrdinal("SalesDay"))
                        ,StartDate = DateOnly.FromDateTime(row.GetDateTime(row.GetOrdinal("SalesStartDate")))
                        ,SalesAmount = row.GetDecimal(row.GetOrdinal("DailySales"))
                    };
                });
                    
                        
            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<OrderWeeklySalesDTO>> GetWeeklyOrderSalesWithinPeriod(int pageNumber, int pageSize, DateOnly startDate, DateOnly endDate)
        {
            try
            {
                return await GetSalesWithinPeriod<OrderWeeklySalesDTO>("SP_GetWeeklyOrderSales", pageNumber, pageSize, startDate, endDate, row =>
                {
                    return new OrderDailySalesDTO
                    {
                        Year = row.GetInt32(row.GetOrdinal("SalesYear"))
                        ,
                        Month = row.GetInt32(row.GetOrdinal("SalesMonth"))
                        ,
                        Week = row.GetInt32(row.GetOrdinal("SalesWeek"))
                        ,
                        StartDate = DateOnly.FromDateTime(row.GetDateTime(row.GetOrdinal("SalesStartDate")))
                        ,
                        SalesAmount = row.GetDecimal(row.GetOrdinal("WeeklySales"))
                    };
                });


            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<OrderMonthlySalesDTO>> GetMonthlyOrderSalesWithinPeriod(int pageNumber, int pageSize, DateOnly startDate, DateOnly endDate)
        {
            try
            {
                return await GetSalesWithinPeriod<OrderMonthlySalesDTO>("SP_GetMonthlyOrderSales", pageNumber, pageSize, startDate, endDate, row =>
                {
                    return new OrderDailySalesDTO
                    {
                        Year = row.GetInt32(row.GetOrdinal("SalesYear"))
                        ,
                        Month = row.GetInt32(row.GetOrdinal("SalesMonth"))
                        ,
                        StartDate = DateOnly.FromDateTime(row.GetDateTime(row.GetOrdinal("SalesStartDate")))
                        ,
                        SalesAmount = row.GetDecimal(row.GetOrdinal("MonthlySales"))
                    };
                });


            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<OrderYearlySalesDTO>> GetYearlyOrderSalesWithinPeriod(int pageNumber, int pageSize, DateOnly startDate, DateOnly endDate)
        {
            try
            {
                return await GetSalesWithinPeriod<OrderYearlySalesDTO>("SP_GetYearlyOrderSales"
                    , pageNumber, pageSize, startDate, endDate, row =>
                {
                     return new OrderYearlySalesDTO
                     {
                         Year = row.GetInt32(row.GetOrdinal("SalesYear"))
                         ,
                         StartDate = DateOnly.FromDateTime(row.GetDateTime(row.GetOrdinal("SalesStartDate")))
                         ,
                         SalesAmount = row.GetDecimal(row.GetOrdinal("YearlySales"))
                     };
                });


            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<StaffOrderSalesDTO>> GetStaffMemberOrderSalesWithinPeriod(int pageNumber, int pageSize, DateOnly startDate, DateOnly endDate)
        {
            try
            {
                return await GetSalesWithinPeriod<StaffOrderSalesDTO>("SP_GetStaffMemberOrderSalesWithinPeriod"
                    , pageNumber, pageSize, startDate, endDate, row =>
                {
                    return new StaffOrderSalesDTO
                    {
                        ID = row.GetInt32(row.GetOrdinal("staff_id"))
                       ,
                        Name = row.GetString(row.GetOrdinal("Name"))
                       ,
                        TotalSalesAmount = row.GetDecimal(row.GetOrdinal("TotalSalesAmount"))
                    };
                     
                });


            }
            catch
            {
                throw;
            }
        }


        public static async Task<PaginatedDTO<OrderItemSalesDTO>> GetItemSalesWithinPeriod(int pageNumber, int pageSize
            , DateOnly startDate, DateOnly endDate)
        {
            try
            {
                return await GetSalesWithinPeriod<OrderItemSalesDTO>("SP_GetMenueItemSalesWithinPeriod", 
                    pageNumber, pageSize, startDate, endDate, row =>
                {
                    return  new OrderItemSalesDTO
                    {
                       ID = row.GetInt32(row.GetOrdinal("item_id")),
                       Name = row.GetString(row.GetOrdinal("name")),
                       Category = row.GetString(row.GetOrdinal("category")),
                       TotalQuantitySold = row.GetInt32(row.GetOrdinal("TotalQuantitySold")),
                       TotalSalesAmount = row.GetDecimal(row.GetOrdinal("TotalSalesAmount"))
                    };
                });

            }
            catch
            {
                throw;
            }
        }



        public static async Task<OrderDTO> GetOrderByID(int id)
        {
            try
            {
                var param = new SqlParameter("@order_id", SqlDbType.Int) { Value = id };

                OrderDTO orderDTO = new OrderDTO();
                await SqlHelper.ExecuteReaderAsync("SP_GetOrderByID", reader =>
                {
                    if (reader.Read())
                    {
                        orderDTO = MapReaderToOrderDTO(reader, id);
                    }
                }, param);

                return orderDTO;
            }
            catch
            {
                throw;
            }
        }


        public static async Task<OrderDetailsOuputDTO> GetOrderDetailsByID(int id)
        {
            try
            {
                var param = new SqlParameter("@order_id", SqlDbType.Int) { Value = id };

                OrderDetailsOuputDTO orderDetailsOutputDTO = new OrderDetailsOuputDTO();

                OrderDTO orderDTO = new OrderDTO();

                await SqlHelper.ExecuteReaderAsync("SP_GetOrderByID", reader =>
                {
                    if (reader.Read())
                    {
                        orderDTO = MapReaderToOrderDTO(reader, id);
                    }
                }, param);




                orderDetailsOutputDTO.OrderID = orderDTO.OrderID;
                orderDetailsOutputDTO.TableID = orderDTO.TableID;
                orderDetailsOutputDTO.StaffID = orderDTO.StaffID;
                orderDetailsOutputDTO.CustomerID = orderDTO.CustomerID;
                orderDetailsOutputDTO.Status = orderDTO.Status;
                orderDetailsOutputDTO.PaymentStatus = orderDTO.PaymentStatus;
                orderDetailsOutputDTO.PaymentMethod = orderDTO.PaymentMethod;
                orderDetailsOutputDTO.Notes = orderDTO.Notes;

                orderDetailsOutputDTO.Items =
                    await OrderItemDataAccess.GetOrderItemsDetailsByOrderID(id);

                return orderDetailsOutputDTO;
            }
            catch
            {
                throw;
            }
        }

        public static async Task<int> UpdateStatus(int id, enOrderStatus newStatus)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@order_id", SqlDbType.Int) { Value = id };
                SqlParameter statusParam = new SqlParameter("@status", SqlDbType.TinyInt)
                { Value = (byte)newStatus };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateOrderStatus", idParam, statusParam);
            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> UpdateOrderPaymentStatus(int id, bool newPaymentStatus)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@order_id", SqlDbType.Int) { Value = id };
                SqlParameter paymentStatusParam = new SqlParameter("@payment_status", SqlDbType.Bit)
                { Value = newPaymentStatus };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateOrderPaymentStatus", idParam, paymentStatusParam);
            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> AfterPaymentUpdates(int id,enPaymentMethod? paymentMethod)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@order_id", SqlDbType.Int) { Value = id };
                SqlParameter newPaymentMethodParam = new SqlParameter("@payment_method", SqlDbType.Int)
                { Value = (byte)paymentMethod };

                return await SqlHelper.ExecuteNonQueryAsync("SP_AfterPaymentUpdates", idParam, newPaymentMethodParam);
            }
            catch
            {
                throw;
            }
        }


        public static async Task<int> UpdateOrderPaymentAmount(int id, decimal newTotalAmount)
        {

            try
            {

                SqlParameter idParam = new SqlParameter("@order_id", SqlDbType.Int) { Value = id };
                SqlParameter newTotalAmountParma = new SqlParameter("@total_amount", SqlDbType.Decimal)
                { Value = newTotalAmount };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateOrderTotalAmount", idParam, newTotalAmountParma);
            }
            catch
            {
                throw;
            }
        }

        public static async Task<int> UpdateOrderNotes(int id, string newOrderNotes)
        {

            try
            {
                SqlParameter idParam = new SqlParameter("@order_id", SqlDbType.Int) { Value = id };
                SqlParameter newOrderNotesParam = new SqlParameter("@notes", SqlDbType.NVarChar)
                { Value = newOrderNotes };

                return await SqlHelper.ExecuteNonQueryAsync("SP_UpdateOrderNotes", idParam, newOrderNotesParam);
            }
            catch
            {
                throw;
            }
        }


        public static async Task<bool> IsOrderServed(int id)
        {

            try
            {

                SqlParameter orderItemIDParam = new SqlParameter("@order_id", SqlDbType.Int) { Value = id };
                SqlParameter isExistParam = new SqlParameter("@IsExist", SqlDbType.Bit) { Direction = ParameterDirection.Output };


                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsOrderServed"
                    , orderItemIDParam, isExistParam);

            }
            catch
            {
                throw;
            }
        }

        public static async Task<bool> IsOrderPaid(int id)
        {

            try
            {

                SqlParameter orderItemIDParam = new SqlParameter("@order_id", SqlDbType.Int) { Value = id };
                SqlParameter isExistParam = new SqlParameter("@IsExist", SqlDbType.Bit) { Direction = ParameterDirection.Output };


                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsOrderPaid"
                    , orderItemIDParam, isExistParam);

            }
            catch
            {
                throw;
            }
        }


        public static async Task<bool> IsOrderCancelled(int id)
        {

            try
            {

                SqlParameter orderItemIDParam = new SqlParameter("@order_id", SqlDbType.Int) { Value = id };
                SqlParameter isExistParam = new SqlParameter("@IsExist", SqlDbType.Bit) { Direction = ParameterDirection.Output };


                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsOrderCancelled"
                    , orderItemIDParam, isExistParam);

            }
            catch
            {
                throw;
            }
        }

        public static async Task<int> DeleteOrder(int id)
        {

            try
            {

                var idParam = new SqlParameter("@order_id", SqlDbType.Int) { Value = id };

                return await SqlHelper.ExecuteNonQueryAsync("SP_DeleteOrder", idParam);

            }
            catch
            {
                throw;
            }
        }

        public static async Task<bool> IsAmountSufficient(int ID,decimal amount)
        {

            try
            {

                SqlParameter orderIDParam = new SqlParameter("@order_id", SqlDbType.Int) { Value = ID };
                SqlParameter amountParam = new SqlParameter("@amount", SqlDbType.Decimal) { Value = amount };
                SqlParameter isExistParam = new SqlParameter("@IsExist", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>
                    ("SP_IsAmountSufficient", orderIDParam,amountParam, isExistParam);
            }
            catch
            {
                throw;
            }
        }

        public static async Task<bool> IsOrderExist(int ID)
        {

            try
            {

                SqlParameter orderIDParam = new SqlParameter("@order_id", SqlDbType.Int) { Value = ID};
                SqlParameter isExistParam = new SqlParameter("@IsExist", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                return await SqlHelper.ExecuteWithOutputParameterAsync<bool>("SP_IsOrderExist",orderIDParam,  isExistParam);
            }
            catch
            {
                throw;
            }
        }



        #endregion

        #region Private Helper Methods



        private static OrderDTO MapReaderToOrderDTO(IDataReader reader, int? id = null)
        {
            //get ordinal of Optional columns
            int customerIDOrdinal = reader.GetOrdinal("customer_id");
            int tableIDOrdinal = reader.GetOrdinal("table_id");
            int staffIDOrdinal = reader.GetOrdinal("staff_id");
            int paymentMethodOrdinal = reader.GetOrdinal("payment_method");
            int notesOrdinal = reader.GetOrdinal("notes");

            return new OrderDTO(
                id ?? reader.GetInt32(reader.GetOrdinal("order_id")),
                reader.IsDBNull(customerIDOrdinal) ? null : reader.GetInt32(customerIDOrdinal),
                reader.IsDBNull(tableIDOrdinal) ? null : reader.GetInt32(tableIDOrdinal),
                reader.IsDBNull(staffIDOrdinal) ? null : reader.GetInt32(staffIDOrdinal),
                reader.GetDecimal(reader.GetOrdinal("total_amount")),
                (enOrderStatus) reader.GetByte(reader.GetOrdinal("status")),
                reader.GetBoolean(reader.GetOrdinal("payment_status")),
                reader.IsDBNull(paymentMethodOrdinal) ? null : (enPaymentMethod)reader.GetByte(paymentMethodOrdinal),
                reader.IsDBNull(notesOrdinal) ? null : reader.GetString(notesOrdinal)
                );
        }

        #endregion
    }
}
