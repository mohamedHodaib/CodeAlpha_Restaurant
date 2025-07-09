using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Restaurant.DataAccess.DTOs.InventoryDTO;

namespace Restaurant.DataAccess.DTOs
{
     public enum enIngrediantUnit { kg, g }
    public class MenueItemIngrediantDTO
    {

        public int IngrediantID { get; set; }
        public int ItemID { get; set; }
        public int InventoryID { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999999.99.")]
        public decimal QuantityUsed { get; set; }

        [Required(ErrorMessage = "Unit Is Required")]
        [EnumDataType(typeof(enIngrediantUnit), ErrorMessage = "Unit must be kg or g")]
        public string Unit { get; set; }


        public MenueItemIngrediantDTO() 
        {
            IngrediantID = 0;
            ItemID = 0;
            InventoryID = 0;
            QuantityUsed = 0;
            Unit = "";
        }


        public MenueItemIngrediantDTO(int ingrediantID, int itemID, int inventoryID
            , decimal quantityUsed, string unit)
        {
            IngrediantID = ingrediantID;
            ItemID = itemID;
            InventoryID = inventoryID;
            QuantityUsed = quantityUsed;
            Unit = unit;
        }
    }
}
