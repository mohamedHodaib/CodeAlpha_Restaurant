using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Business;
using Restaurant.DataAccess.DTOs;
using Restaurant.DataAccess.DTOs.Order;
using Restaurant.DataAccess.DTOs.Sales;

namespace Restaurant.Api.Controllers
{
    [Route("api/OrderController")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpPost(Name = "AddNewOrderRecord")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddNewOrderRecord(OrderDetailsInputDTO newOrderRecordDTO)
        {
            if (newOrderRecordDTO == null)
                return BadRequest("Order Record Cannot Be null");

            Order order = new Order(newOrderRecordDTO);

            var result = await order.Save();

            if (!result.Success)
            {
                return this.HandleFailureResult(result);

            }

            newOrderRecordDTO.OrderID = order.OrderID;

            return CreatedAtAction(nameof(GetOrderRecordByOrderID)
                , new { id = newOrderRecordDTO.OrderID }, newOrderRecordDTO);
        }


        [HttpGet("GetAllOrders/{pageNumber}/{pageSize}"
           , Name = "GetAllOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAllOrders(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<OrderDTO>> findResult
                = await Order.GetAllOrders(pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                "There is no Order Records."
                : findResult.Data);
        }



        [HttpGet("GetOrderRecordByOrderID/{id}", Name = "GetOrderRecordByOrderID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetOrderRecordByOrderID(int id)
        {

            if (id < 0) return BadRequest($"Not Accepted ID : {id}");

            BusinessResult<OrderDTO> findResult = await Order.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data);
        }





        [HttpGet("GetActiveOrders/{pageNumber}/{pageSize}"
            , Name = "GetActiveOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetActiveOrders(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<OrderDTO>> findResult
                = await Order.GetActiveOrders(pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no Active Orders."
                : findResult.Data);
        }


        [HttpGet("GetOrderDetailsByID/{id}"
            , Name = "GetOrderDetailsByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetOrderDetailsByID(int id)
        {
            if (id <= 0)
                return BadRequest("Not Accepted Data");

            BusinessResult<OrderDetailsOuputDTO> findResult
                = await Order.GetOrderDetailsByID(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data);
        }


        [HttpGet("GetOrdersByCustomerID/{customerID}/{pageNumber}/{pageSize}"
           , Name = "GetOrdersByCustomerID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetOrdersByCustomerID(int customerID, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<OrderDTO>> findResult
                = await Order.GetOrdersByCustomerID(customerID, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no Active Orders."
                : findResult.Data);
        }




        [HttpGet("GetOrdersByStatus/{status}/{pageNumber}/{pageSize}"
        , Name = "GetOrdersByStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetOrdersByStatus(enOrderStatus status, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<OrderDTO>> findResult
                = await Order.GetOrdersByStatus(status, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no Orders With status {status}."
                : findResult.Data);
        }

        [HttpGet("GetOrdersWithinPeriod/{startDate}/{endDate}/{pageNumber}/{pageSize}"
         , Name = "GetOrdersWithinPeriod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetOrdersWithinPeriod(DateOnly startDate, DateOnly endDate,
             int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<OrderDTO>> findResult
                = await Order.GetOrdersWithinPeriod(startDate
                , endDate, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no Orders from {startDate} to {endDate}."
                : findResult.Data);
        }


        [HttpGet("GetDailyOrderSalesWithinPeriod/{startDate}/{endDate}/{pageNumber}/{pageSize}"
           , Name = "GetDailyOrderSalesWithinPeriod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetDailyOrderSalesWithinPeriod(DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<OrderDailySalesDTO>> findResult
                = await Order.GetDailyOrderSalesWithinPeriod(startDate, endDate, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no Order Sales from {startDate} to {endDate}."
                : findResult.Data);
        }


        [HttpGet("GetMonthlyOrderSalesWithinPeriod/{startDate}/{endDate}/{pageNumber}/{pageSize}"
           , Name = "GetMonthlyOrderSalesWithinPeriod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetMonthlyOrderSalesWithinPeriod(DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<OrderMonthlySalesDTO>> findResult
                = await Order.GetMonthlyOrderSalesWithinPeriod(startDate, endDate, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no Order Sales from {startDate} to {endDate}."
                : findResult.Data);
        }

        [HttpGet("GetWeeklyOrderSalesWithinPeriod/{startDate}/{endDate}/{pageNumber}/{pageSize}"
           , Name = "GetWeeklyOrderSalesWithinPeriod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetWeeklyOrderSalesWithinPeriod(DateOnly startDate, DateOnly endDate
            , int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<OrderWeeklySalesDTO>> findResult
                = await Order.GetWeeklyOrderSalesWithinPeriod(startDate, endDate, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no Order Sales from {startDate} to {endDate}."
                : findResult.Data);
        }

        [HttpGet("GetYearlyOrderSalesWithinPeriod/{startDate}/{endDate}/{pageNumber}/{pageSize}"
           , Name = "GetYearlyOrderSalesWithinPeriod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetYearlyOrderSalesWithinPeriod(DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<OrderYearlySalesDTO>> findResult
                = await Order.GetYearlyOrderSalesWithinPeriod(startDate, endDate, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no Order Sales from {startDate} to {endDate}."
                : findResult.Data);
        }

        [HttpGet("GetItemSalesWithinPeriod/{startDate}/{endDate}/{pageNumber}/{pageSize}"
           , Name = "GetItemSalesWithinPeriod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetItemSalesWithinPeriod(DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<OrderItemSalesDTO>> findResult
                = await Order.GetItemSalesWithinPeriod(startDate, endDate, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no Item Sales from {startDate} to {endDate}."
                : findResult.Data);
        }


        [HttpGet("GetStaffMemberOrderSalesWithinPeriod/{startDate}/{endDate}/{pageNumber}/{pageSize}"
           , Name = "GetStaffMemberOrderSalesWithinPeriod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStaffMemberOrderSalesWithinPeriod(DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<StaffOrderSalesDTO>> findResult
                = await Order.GetStaffMemberOrderSalesWithinPeriod(startDate, endDate, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no Staff Member Sales from {startDate} to {endDate}."
                : findResult.Data);
        }



        [HttpPut("Cancel/{id}", Name = "Cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Cancel(int id)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");

            BusinessResult<bool> UpdateResult = await Order.Cancel(id);

            if (!UpdateResult.Success)
                return this.HandleFailureResult(UpdateResult);

            return Ok(UpdateResult.Message);

        }

        [HttpPut("UpdatePaymentStatus/{id}/{status}", Name = "UpdatePaymentStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePaymentStatus(int id, bool status)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");

            BusinessResult<bool> UpdateResult = await Order.UpdatePaymentStatus(id, status);

            if (!UpdateResult.Success)
                return this.HandleFailureResult(UpdateResult);

            return Ok(UpdateResult.Message);

        }


        [HttpPut("UpdatePaymentAmount/{id}/{amount}", Name = "UpdatePaymentAmount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePaymentStatus(int id, decimal amount)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");

            BusinessResult<bool> UpdateResult = await Order.UpdatePaymentAmount(id, amount);

            if (!UpdateResult.Success)
                return this.HandleFailureResult(UpdateResult);

            return Ok(UpdateResult.Message);

        }


        [HttpPut("UpdatePaymentNotes/{id}/{notes}", Name = "UpdatePaymentNotes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePaymentNotes(int id, string notes)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");

            BusinessResult<bool> UpdateResult = await Order.UpdatePaymentNotes(id, notes);

            if (!UpdateResult.Success)
                return this.HandleFailureResult(UpdateResult);

            return Ok(UpdateResult.Message);

        }



        [HttpDelete("DeleteOrderRecord/{id}", Name = "DeleteOrderRecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Not Accepted Id");

            BusinessResult<bool> deleteResult = await Order.Delete(id);

            if (!deleteResult.Success)
                return this.HandleFailureResult(deleteResult);

            return Ok(deleteResult.Message);

        }

    }
}
