using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Business;
using Restaurant.DataAccess.DTOs;

namespace Restaurant.Api.Controllers
{
    [Route("api/MenueItemController")]
    [ApiController]
    public class MenueItemController : ControllerBase
    {
        [HttpPost(Name = "AddNewMenueItem")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddNewMenueItem(MenueItemDTO newMenueItemDTO)
        {
            if (newMenueItemDTO == null)
                return BadRequest("Menu Item Cannot Be null");

            MenueItem menueItem = new MenueItem(newMenueItemDTO);

            var result = await menueItem.Save();

            if (!result.Success)
            {
                return this.HandleFailureResult(result);

            }

            newMenueItemDTO.ID = menueItem.ID;

            return CreatedAtAction(nameof(GetMenueItemByMenueItemID)
                , new { id = newMenueItemDTO.ID }, newMenueItemDTO);
        }


        [HttpGet("GetAllMenuItems/{pageNumber}/{pageSize}", Name = "GetAllMenuItems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAllMenuItems(int pageNumber,int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<MenueItemDTO>> findResult 
                = await MenueItem.GetAllMenueItems(pageNumber,pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.Items.Count == 0 ?
                 "There is not Menu Items."
                 : findResult.Data);
        }

        [HttpGet("GetMenueItemByMenueItemID/{id}", Name = "GetMenueItemByMenueItemID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetMenueItemByMenueItemID(int id)
        {

                if (id < 0) return BadRequest($"Not Accepted ID : {id}");

            BusinessResult<MenueItem> findResult = await MenueItem.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.menueItemDTO);
        }


        [HttpGet("GetMenueItemByMenueItemName/{name}",Name = "GetMenuItemByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetMenuItemByName(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Length > 255 || name.Length < 2)
                return BadRequest("Not Accepted Data");

            BusinessResult<MenueItem> findResult
                = await MenueItem.Find(name);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.menueItemDTO);
        }


        [HttpGet("GetMenuItemsByCategory/{category}/{pageNumber}/{pageSize}", Name = "GetMenuItemsByCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetMenuItemsByCategory(string category,int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1
                || string.IsNullOrEmpty(category) 
                || category.Length > 100
                || category.Length < 4)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<MenueItemDTO>> findResult
                = await MenueItem.GetMenueItemsByCategory(category,pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.Items.Count == 0 ?
                "There is not Menu Item with the Provided Category."
                : findResult.Data);
        }

        [HttpGet("GetMenuItemsByDescription/{description}/{pageNumber}/{pageSize}"
            , Name = "GetMenuItemsByDescription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetMenuItemsByDescription(string description, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1
                || string.IsNullOrEmpty(description)
                || description.Length > 4000)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<MenueItemDTO>> findResult
                = await MenueItem.GetMenueItemsByDescription(description, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.Items.Count == 0 ?
                "There is not Menu Item with the Provided Description."
                : findResult.Data);
        }


        [HttpGet("GetMenuItemsByAvailability/{availability}/{pageNumber}/{pageSize}", 
            Name = "GetMenuItemsByAvailability")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetMenuItemsByAvailability(bool availability,int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<MenueItemDTO>> findResult = availability ?
              await MenueItem.GetAvailableMenueItems(pageNumber,pageSize)
              : await MenueItem.GetUnAvailableMenueItems(pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.Items.Count == 0 ?
                "There is not Menu Item with the Provided Availability."
                : findResult.Data);
        }

        [HttpPut("UpdateMenueItem/{id}", Name = "UpdateMenueItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, MenueItemDTO menueItemDTO)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");

            BusinessResult<MenueItem> findResult = await MenueItem.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);


            findResult.Data.Name = menueItemDTO.Name;
            findResult.Data.Category = menueItemDTO.Category;
            findResult.Data.Description = menueItemDTO.Description;
            findResult.Data.Price = menueItemDTO.Price;
            findResult.Data.IsAvailable = menueItemDTO.IsAvailable;

            BusinessResult<bool> saveResult = await findResult.Data.Save();

            if (!saveResult.Success)
                return this.HandleFailureResult(saveResult);

            return Ok(saveResult.Message);

        }

        [HttpPut("UpdateMenueItemAvailability/{id}/{availability}", Name = "UpdateMenueItemAvailability")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMenueItemAvailability(int id, bool availability)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");

            BusinessResult<MenueItem> findResult = await MenueItem.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            BusinessResult<bool> UpdateResult = availability ?
                await findResult.Data.MakeItAvailable()
                : await findResult.Data.MakeItUnAvailable();

            if (!UpdateResult.Success)
                return this.HandleFailureResult(UpdateResult);

            return Ok(UpdateResult.Message);

        }

        [HttpDelete("DeleteMenueItem/{id}", Name = "DeleteMenueItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Not Accepted Id");

            BusinessResult<MenueItem> findResult = await MenueItem.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);


            BusinessResult<bool> deleteResult = await findResult.Data.Delete();

            if (!deleteResult.Success)
                return this.HandleFailureResult(deleteResult);

            return Ok(deleteResult.Message);

        }

    }
}
