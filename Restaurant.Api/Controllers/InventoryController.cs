using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Business;
using Restaurant.DataAccess.DTOs;

namespace Restaurant.Api.Controllers
{
    [Route("api/InventoryController")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        [HttpPost(Name = "AddNewInventoryRecord")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddNewInventoryRecord(InventoryDTO newInventoryRecordDTO)
        {
            if (newInventoryRecordDTO == null)
                return BadRequest("Inventory Record Cannot Be null");

            Inventory inventoryItem = new Inventory(newInventoryRecordDTO);

            var result = await inventoryItem.Save();

            if (!result.Success)
            {
                return this.HandleFailureResult(result);

            }

            newInventoryRecordDTO.ID = inventoryItem.ID;

            return CreatedAtAction(nameof(GetInventoryRecordByInventoryID)
                , new { id = newInventoryRecordDTO.ID }, newInventoryRecordDTO);
        }


        [HttpGet("GetAllInventories/{pageNumber}/{pageSize}"
           , Name = "GetAllInventories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAllInventories(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<InventoryDTO>> findResult
                = await Inventory.GetAllInventories(pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                "There is no Inventory Records."
                : findResult.Data);
        }



        [HttpGet("GetInventoryRecordByInventoryID/{id}", Name = "GetInventoryRecordByInventoryID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetInventoryRecordByInventoryID(int id)
        {

            if (id < 0) return BadRequest($"Not Accepted ID : {id}");

            BusinessResult<Inventory> findResult = await Inventory.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.inventoryDTO);
        }


       


        [HttpGet("GetInventoryItemsBySupplier/{supplier}/{pageNumber}/{pageSize}"
            , Name = "GetInventoryItemsBySupplier")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetInventoryItemsBySupplier(string supplier,int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1 || 
                string.IsNullOrEmpty(supplier) || supplier.Length > 255)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<InventoryDTO>> findResult
                = await Inventory.GetInventoryItemsBySupplier(supplier,pageNumber,pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is not Inventory Record with {supplier} Supplier."
                : findResult.Data);
        }


        [HttpGet("GetInventoryItemsByUnit/{unit}/{pageNumber}/{pageSize}"
            , Name = "GetInventoryItemsByUnit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetInventoryItemsByUnit(string unit, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1
                || string.IsNullOrEmpty(unit) || unit.Length > 50)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<InventoryDTO>> findResult
                = await Inventory.GetInventoryItemsByUnit(unit, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is not Inventory Record with {unit} Unit."
                : findResult.Data);
        }


        [HttpGet("SearchInventoryItemsByName/{name}/{pageNumber}/{pageSize}"
            , Name = "SearchInventoryItemsByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> SearchInventoryItemsByName(string name, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1
                || string.IsNullOrEmpty(name)
                || name.Length > 255 )
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<InventoryDTO>> findResult
                = await Inventory.SearchInventoryItemsByName(name, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is not Inventory Record with that contains  {name}."
                : findResult.Data);
        }


        [HttpGet("GetLowStockInventoryItems/{pageNumber}/{pageSize}"
           , Name = "GetLowStockInventoryItems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetLowStockInventoryItems(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<InventoryDTO>> findResult
                = await Inventory.GetLowStockItems(pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                "There is no Inventory Items that thier quantity is less than thier low level stock ."
                : findResult.Data);
        }


        [HttpPut("UpdateInventoryRecord/{id}", Name = "UpdateInventoryRecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, InventoryDTO inventoryDTO)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");

            BusinessResult<Inventory> findResult = await Inventory.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);


            findResult.Data.Quantity = inventoryDTO.Quantity;
            findResult.Data.Supplier = inventoryDTO.Supplier;
            findResult.Data.MinStockLevel = inventoryDTO.MinStockLevel;
            findResult.Data.Unit = inventoryDTO.Unit;

            BusinessResult<bool> saveResult = await findResult.Data.Save();

            if (!saveResult.Success)
                return this.HandleFailureResult(saveResult);

            return Ok(saveResult.Message);

        }

        [HttpPut("UpdateInventoryItemQuantity/{id}/{quantity}", Name = "UpdateInventoryItemQuantity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateInventoryItemQuantity(int id, decimal quantity)
        {
            if (id <= 0  || quantity < 0.01m || quantity > 999999.99m)
                return BadRequest("Not Accepted ID");

            BusinessResult<Inventory> findResult = await Inventory.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);


            findResult.Data.Quantity =  quantity;

            BusinessResult<bool> saveResult = await findResult.Data.Save();

            if (!saveResult.Success)
                return this.HandleFailureResult(saveResult);

            return Ok(saveResult.Message);

        }

        [HttpPut("UpdateInventoryItemUnit/{id}/{unit}", Name = "UpdateInventoryItemUnit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateInventoryItemUnit(int id, string unit)
        {
            if (id <= 0 || unit != "kg" && unit != "g")
                return BadRequest("Not Accepted ID");

            BusinessResult<Inventory> findResult = await Inventory.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);


            findResult.Data.Unit = unit;

            BusinessResult<bool> saveResult = await findResult.Data.Save();

            if (!saveResult.Success)
                return this.HandleFailureResult(saveResult);

            return Ok(saveResult.Message);

        }

        [HttpPut("UpdateInventoryItemSupplier/{id}/{supplier}", Name = "UpdateInventoryItemSupplier")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateInventoryItemSupplier(int id, string supplier)
        {
            if (id <= 0 ||
                (supplier != null
                && ( string.IsNullOrEmpty(supplier) ||supplier.Length > 255)))
                return BadRequest("Not Accepted ID");

            BusinessResult<Inventory> findResult = await Inventory.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);


            findResult.Data.Supplier = supplier;

            BusinessResult<bool> saveResult = await findResult.Data.Save();

            if (!saveResult.Success)
                return this.HandleFailureResult(saveResult);

            return Ok(saveResult.Message);

        }


        [HttpPut("UpdateInventoryItemMinLevelStock/{id}/{minLevelStock}", Name = "UpdateInventoryItemMinLevelStock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateInventoryItemMinLevelStock(int id, decimal minLevelStock)
        {
            if (id <= 0 || minLevelStock < 0.01m || minLevelStock > 999999.99m)
                return BadRequest("Not Accepted ID");

            BusinessResult<Inventory> findResult = await Inventory.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);


            findResult.Data.MinStockLevel = minLevelStock;

            BusinessResult<bool> saveResult = await findResult.Data.Save();

            if (!saveResult.Success)
                return this.HandleFailureResult(saveResult);

            return Ok(saveResult.Message);

        }


        [HttpDelete("DeleteInventoryRecord/{id}", Name = "DeleteInventoryRecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Not Accepted Id");

            BusinessResult<Inventory> findResult = await Inventory.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);


            BusinessResult<bool> deleteResult = await findResult.Data.Delete();

            if (!deleteResult.Success)
                return this.HandleFailureResult(deleteResult);

            return Ok(deleteResult.Message);

        }

    }
}
