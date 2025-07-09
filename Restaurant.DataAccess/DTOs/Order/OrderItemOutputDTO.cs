using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs.Order
{
    public class OrderItemOutputDTO
    {
        public int OrderItemID { get; set; }
        public string ItemName { get; set; }
        public decimal PriceAtOrder { get; set; }
        public int Quantity { get; set; }


        public OrderItemOutputDTO()
        {
            OrderItemID = 0;
            ItemName = string.Empty;
            PriceAtOrder = 0;
            Quantity = 0;
        }

        public OrderItemOutputDTO(int orderItemID, string itemName
            , decimal priceAtOrder, int quantity)
        {
            OrderItemID = orderItemID;
            ItemName = itemName;
            PriceAtOrder = priceAtOrder;
            Quantity = quantity;
        }
    }
}
