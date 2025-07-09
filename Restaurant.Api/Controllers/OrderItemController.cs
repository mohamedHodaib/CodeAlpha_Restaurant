using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Business;
using Restaurant.DataAccess.DTOs;
using Restaurant.DataAccess.DTOs.Order;

namespace Restaurant.Api.Controllers
{
    [Route("api/OrderItemController")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        [HttpPost("AddNewOrderItemRecord/{orderID}", Name = "AddNewOrderItemRecord")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddNewOrderItemRecord(int orderID, OrderItemInputDTO newOrderItemRecordDTO)
        {
            if (orderID <= 0 || newOrderItemRecordDTO == null)
                return BadRequest("not Accepted data.");

            var result = await OrderItem.AddNewItemToOrder(orderID, newOrderItemRecordDTO);

            if (!result.Success)
            {
                return this.HandleFailureResult(result);
            }

            return Ok("Order Item Created Successfully");
        }


        [HttpPut("UpdateOrderItemQuantity/{orderID}/{orderItemID}/{newQuantity}", Name = "UpdateOrderItemQuantity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOrderItemQuantity(int orderID, int orderItemID, int newQuantity)
        {
            if (orderID <= 0 || orderItemID <= 0 || newQuantity <= 0)
                return BadRequest("Not Accepted Data");

            BusinessResult<bool> UpdateResult = await OrderItem.UpdateOrderItemQuantity(orderID, orderItemID, newQuantity);

            if (!UpdateResult.Success)
                return this.HandleFailureResult(UpdateResult);

            return Ok(UpdateResult.Message);

        }


        [HttpDelete("DeleteOrderItemRecord/{orderID}/{orderItemID}", Name = "DeleteOrderItemRecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int orderID, int orderItemID)
        {
            if (orderID <= 0 || orderItemID <= 0)
                return BadRequest("Not Accepted Id");

            BusinessResult<bool> deleteResult = await OrderItem.DeleteOrderItem(orderID, orderItemID);

            if (!deleteResult.Success)
                return this.HandleFailureResult(deleteResult);

            return Ok(deleteResult.Message);

        }
    }
}
