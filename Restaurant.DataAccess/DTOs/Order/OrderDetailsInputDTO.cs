using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs.Order
{
    public class OrderDetailsInputDTO : OrderDTO
    {

        public List<OrderItemInputDTO> Items { get; set; }

        public OrderDetailsInputDTO()
        {
            Items = new List<OrderItemInputDTO>();
        }

        public OrderDetailsInputDTO(int orderID, int? customerID, int? tableID, int? staffID,
            decimal totalAmount, enOrderStatus status, bool paymentStatus, enPaymentMethod? paymentMethod
            , string? notes, List<OrderItemInputDTO> items)
            : base(orderID, customerID, tableID, staffID,
             totalAmount, status, paymentStatus, paymentMethod, notes)
        {
            Items = items;
        }
    }
}
