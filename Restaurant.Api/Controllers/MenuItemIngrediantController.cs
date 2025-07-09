using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Business;
using Restaurant.DataAccess.DTOs;

namespace Restaurant.Api.Controllers
{
    [Route("api/MenuItemIngrediantController")]
    [ApiController]
    public class MenuItemIngrediantController : ControllerBase
    {
        [HttpPost(Name = "AddNewMenueItemIngrediant")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddNewMenueItemIngrediant(MenueItemIngrediantDTO newMenueItemIngrediantDTO)
        {
            if (newMenueItemIngrediantDTO == null)
                return BadRequest("Menue Item Ingrediant Cannot Be null");

            MenuItemIngrediant menueItemIngrediant = new MenuItemIngrediant(newMenueItemIngrediantDTO);

            var result = await menueItemIngrediant.Save();

            if (!result.Success)
            {
                return this.HandleFailureResult(result);

            }

            newMenueItemIngrediantDTO.IngrediantID = menueItemIngrediant.IngrediantID;

            return CreatedAtAction(nameof(GetMenueItemIngrediantByMenueItemIngrediantID)
                , new { id = newMenueItemIngrediantDTO.IngrediantID }, newMenueItemIngrediantDTO);
        }


       

        [HttpGet("GetMenueItemIngrediantByMenueItemIngrediantID/{id}", Name = "GetMenueItemIngrediantByMenueItemIngrediantID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetMenueItemIngrediantByMenueItemIngrediantID(int id)
        {

            if (id < 0) return BadRequest($"Not Accepted ID : {id}");

            BusinessResult<MenuItemIngrediant> findResult = await MenuItemIngrediant.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.menueItemIngrediantDTO);
        }


        [HttpGet("GetMenueItemIngrediantByInventoryID/{inventoryID}"
            , Name = "GetMenueItemIngrediantByInventoryID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetMenuItemIngrediantByInventoryID(int inventoryID)
        {
            if (inventoryID <= 0)
                return BadRequest("Not Accepted Data");

            BusinessResult<MenueItemIngrediantReportDTO> findResult
                = await MenuItemIngrediant.GetMenuItemIngrediantByInventoryID(inventoryID);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data);
        }


        [HttpGet("GetMenuItemIngrediantsByMenuItemID/{menuItemID}"
            , Name = "GetMenuItemIngrediantsByMenuItemID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetMenuItemIngrediantsByCategory(int menuItemID)
        {
            if (menuItemID <= 0)
                return BadRequest("Not Accepted Data");

            BusinessResult<List<MenueItemIngrediantDTO>> findResult
                = await MenuItemIngrediant.GetMenuItemIngrediantsByMenuItemID(menuItemID);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.Count == 0 ?
                $"There is not Menue Item Ingrediant with the  Item ID {menuItemID}."
                : findResult.Data);
        }

    

        [HttpPut("UpdateMenueItemIngrediant/{id}", Name = "UpdateMenueItemIngrediant")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, MenueItemIngrediantDTO menueItemIngrediantDTO)
        {
           if (id <= 0)
                return BadRequest("Not Accepted ID");

            BusinessResult<MenuItemIngrediant> findResult = await MenuItemIngrediant.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);


            findResult.Data.QuantityUsed = menueItemIngrediantDTO.QuantityUsed;
            findResult.Data.Unit = menueItemIngrediantDTO.Unit;

            BusinessResult<bool> saveResult = await findResult.Data.Save();

            if (!saveResult.Success)
                return this.HandleFailureResult(saveResult);

            return Ok(saveResult.Message);

        }

  

        [HttpDelete("DeleteMenueItemIngrediant/{id}", Name = "DeleteMenueItemIngrediant")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Not Accepted Id");

            BusinessResult<MenuItemIngrediant> findResult = await MenuItemIngrediant.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);


            BusinessResult<bool> deleteResult = await findResult.Data.Delete();

            if (!deleteResult.Success)
                return this.HandleFailureResult(deleteResult);

            return Ok(deleteResult.Message);

        }

    }
}
