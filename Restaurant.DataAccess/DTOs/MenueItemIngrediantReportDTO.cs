using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs
{
    public class MenueItemIngrediantReportDTO
    {
        public int IngrediantID { get; set; }
        public string ItemName { get; set; }
        public string IngrediantName { get; set; }
        public decimal QuantityUsed { get; set; }
        public string Unit { get; set; }


        public MenueItemIngrediantReportDTO()
        {
            IngrediantID = 0;
            ItemName = string.Empty;
            IngrediantName = string.Empty;
            QuantityUsed = 0;
            Unit = "";
        }


        public MenueItemIngrediantReportDTO(int ingrediantID, string itemName, string ingrediantName
            , decimal quantityUsed, string unit)
        {
            IngrediantID = ingrediantID;
            ItemName = itemName;
            IngrediantName = ingrediantName;
            QuantityUsed = quantityUsed;
            Unit = unit;
        }
    }
}
