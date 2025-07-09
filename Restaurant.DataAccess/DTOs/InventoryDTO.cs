using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs
{
     public enum enInventoryUnit { kg ,g}
    public class InventoryDTO
    {
        public int ID {  get; set; }

        [Required(ErrorMessage = "Item Name Is Required")]
        [StringLength(255,MinimumLength = 2
            ,ErrorMessage = "Item Name Characters must between 2 to 255 characters")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Quantity Is Required")]
        [Range(0.01, 999999.99, ErrorMessage = "Quantity must be from Min Stock Level 999999.99.")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Unit Is Required")]
        [EnumDataType(typeof(enIngrediantUnit),ErrorMessage = "Unit must be kg or g")]
        public string Unit {  get; set; }

        [Required(ErrorMessage = "Min Level Stock Is Required")]
        [Range(0.01, 999999.99, ErrorMessage = "Quantity must be between 0.01 and 999999.99.")]
        public decimal MinStockLevel { get; set; }

        [StringLength(255, ErrorMessage = "Supplier Characters must between 2 to 255 characters")]
        public string? Supplier { get; set; }

        public DateOnly ? LastRestockDate { get; set; }


        public InventoryDTO() 
        { 
            ID = 0;
            ItemName = string.Empty;
            Quantity = 0;
            Unit = string.Empty;
            MinStockLevel = 0;
            Supplier = null;
            LastRestockDate = null;
        }

        public InventoryDTO(int iD, string itemName, decimal quantity, string unit,
            decimal minStockLevel, string? supplier, DateOnly? lastRestockDate)
        {
            ID = iD;
            ItemName = itemName;
            Quantity = quantity;
            Unit = unit;
            MinStockLevel = minStockLevel;
            Supplier = supplier;
            LastRestockDate = lastRestockDate;
        }

    }
}
