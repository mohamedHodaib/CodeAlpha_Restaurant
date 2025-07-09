using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs.Order
{
    public class OrderDetailsOuputDTO : OrderDTO
    {

        public List<OrderItemOutputDTO> Items { get; set; }

        public OrderDetailsOuputDTO() 
        {
            Items = new List<OrderItemOutputDTO>();
        }

        public OrderDetailsOuputDTO(int orderID, int? customerID, int? tableID, int? staffID,
            decimal totalAmount, enOrderStatus status, bool paymentStatus, enPaymentMethod? paymentMethod
            , string? notes,List<OrderItemOutputDTO> items)
            :base ( orderID, customerID, tableID,  staffID,
               totalAmount,  status,  paymentStatus, paymentMethod,notes )
        {
            Items = items;
        }
    }
}
