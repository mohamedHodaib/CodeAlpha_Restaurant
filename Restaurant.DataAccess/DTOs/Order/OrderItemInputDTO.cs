using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs.Order
{
    public class OrderItemInputDTO
    {
        public int MenuItemID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity Must be Positive")]
        public int Quantity { get; set; }

        [StringLength(4000, ErrorMessage = "Notes must not exceed 4000")]
        public string? Notes { get; set; } // Can be null
    }
}
