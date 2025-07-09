using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Business;
using Restaurant.DataAccess.DTOs;

namespace Restaurant.Api.Controllers
{
    [Route("api/TableController")]
    [ApiController]
    public class TableController : ControllerBase
    {
        [HttpPost(Name = "AddNewTableRecord")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddNewTableRecord([FromBody] TableDTO newTableRecordDTO)
        {
            if (newTableRecordDTO == null)
                return BadRequest("Table Record Cannot Be null");

            Table table = new Table(newTableRecordDTO);

            var result = await table.Save();

            if (!result.Success)
            {
                return this.HandleFailureResult(result);

            }

            newTableRecordDTO.ID = table.ID;

            return CreatedAtAction(nameof(GetTableRecordByTableID)
                , new { id = newTableRecordDTO.ID }, newTableRecordDTO);
        }


        [HttpGet("GetAllTables/{pageNumber}/{pageSize}"
           , Name = "GetAllTables")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAllTables(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<TableDTO>> findResult
                = await Table.GetAllTables(pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                "There is no Table Records."
                : findResult.Data);
        }



        [HttpGet("GetTableRecordByTableID/{id}", Name = "GetTableRecordByTableID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetTableRecordByTableID(int id)
        {

            if (id < 0) return BadRequest($"Not Accepted ID : {id}");

            BusinessResult<Table> findResult = await Table.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.tableDTO);
        }





        [HttpGet("GetAvailableTablesWithMinCapacity/{minCapacity}/{pageNumber}/{pageSize}"
            , Name = "GetAvailableTablesWithMinCapacity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAvailableTablesWithMinCapacity(int minCapacity, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1 || minCapacity <= 0 || minCapacity > 10)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<TableDTO>> findResult
                = await Table.GetAvailableTablesWithMinCapacity(minCapacity, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no Available Tables With minCapacity {minCapacity}."
                : findResult.Data);
        }


        [HttpGet("GetTablesByCapacity/{capacity}/{pageNumber}/{pageSize}"
            , Name = "GetTablesByCapacity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetTablesByCapacity(int capacity, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1 || capacity <= 0 || capacity > 10)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<TableDTO>> findResult
                = await Table.GetTablesByCapacity(capacity, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no Tables With Capacity {capacity}."
                : findResult.Data);
        }



        [HttpGet("GetTablesByStatus/{status}/{pageNumber}/{pageSize}"
        , Name = "GetTablesByStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetTablesByStatus(enTableStatus status, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<TableDTO>> findResult
                = await Table.GetTablesByStatus(status, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no Tables With status {status}."
                : findResult.Data);
        }



        [HttpGet("GetTablesByMinCapacityAndStatus/{capacity}/{status}/{pageNumber}/{pageSize}"
        , Name = "GetTablesByMinCapacityAndStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetTablesByMinCapacityAndStatus(int capacity, enTableStatus status
            , int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1 || capacity <= 0 || capacity > 10)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<TableDTO>> findResult
                = await Table.GetTablesByMinCapacityAndStatus(capacity, status, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no  Tables With capacity {capacity} and status {status}."
                : findResult.Data);
        }


        [HttpGet("GetTablesByLocation/{location}/{pageNumber}/{pageSize}"
        , Name = "GetTablesByLocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetTablesByLocation(string location, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<TableDTO>> findResult
                = await Table.GetTablesByLocation(location, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no Tables With Location {location}."
                : findResult.Data);
        }



        [HttpPut("UpdateTableRecord/{id}", Name = "UpdateTableRecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, TableDTO tableDTO)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");

            BusinessResult<Table> findResult = await Table.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            findResult.Data.Number = tableDTO.Number;
            findResult.Data.Capacity = tableDTO.Capacity;
            findResult.Data.CurrentStatus = tableDTO.CurrentStatus;
            findResult.Data.Location = tableDTO.Location;


            BusinessResult<bool> saveResult = await findResult.Data.Save();

            if (!saveResult.Success)
                return this.HandleFailureResult(saveResult);

            return Ok(saveResult.Message);

        }

        [HttpPut("UpdateTableStatus/{id}/{status}", Name = "UpdateTableStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTableStatus(int id, enTableStatus status)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");


            BusinessResult<bool> UpdateResult = await Table.UpdateTableStatus(id, status);

            if (!UpdateResult.Success)
                return this.HandleFailureResult(UpdateResult);

            return Ok(UpdateResult.Message);

        }

        [HttpPut("UpdateTableCapacity/{id}/{capacity}", Name = "UpdateTableCapacity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTableCapacity(int id, int capacity)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");


            BusinessResult<bool> UpdateResult = await Table.UpdateTableCapacity(id, capacity);

            if (!UpdateResult.Success)
                return this.HandleFailureResult(UpdateResult);

            return Ok(UpdateResult.Message);

        }


        [HttpPut("UpdateTableLocation/{id}/{location}", Name = "UpdateTableLocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTableLocation(int id, string location)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");


            BusinessResult<bool> UpdateResult = await Table.UpdateTableLocation(id, location);

            if (!UpdateResult.Success)
                return this.HandleFailureResult(UpdateResult);

            return Ok(UpdateResult.Message);

        }



        [HttpDelete("DeleteTableRecord/{id}", Name = "DeleteTableRecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Not Accepted Id");

            BusinessResult<Table> findResult = await Table.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);


            BusinessResult<bool> deleteResult = await findResult.Data.Delete();

            if (!deleteResult.Success)
                return this.HandleFailureResult(deleteResult);

            return Ok(deleteResult.Message);

        }

    }
}
