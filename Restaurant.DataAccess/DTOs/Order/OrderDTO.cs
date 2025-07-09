using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs.Order
{
    public enum enOrderStatus { Pending = 1, Preparing, Ready, Served, Paid, Cancelled };
    public enum enPaymentMethod { Cash=1,Card,MobilePay };
    public class OrderDTO
    {


        public int OrderID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Customer Id Must be Positive")]
        public int? CustomerID { get; set; }

        
        [Range(1, int.MaxValue, ErrorMessage = "Table Id Must be Positive")]
        public int? TableID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Staff Id Must be Positive")]
        public int? StaffID { get; set; }
        public decimal TotalAmount { get; set; }
        public enOrderStatus Status { get; set; }
        public bool PaymentStatus { get; set; }
        public enPaymentMethod? PaymentMethod { get; set; }

        [StringLength(4000, ErrorMessage = "Notes must not exceed 4000")]
        public string? Notes { get; set; }

        public OrderDTO()
        {
            OrderID = 0;
            CustomerID = null;
            TableID = null;
            StaffID = null;
            TotalAmount = 0;
            Status =  enOrderStatus.Pending;
            PaymentStatus = false;
            PaymentMethod = null;
            Notes = null;
        }


        public OrderDTO(int orderID, int? customerID, int? tableID, int? staffID, decimal totalAmount,
            enOrderStatus status, bool paymentStatus, enPaymentMethod? paymentMethod,string? notes)
        {
            OrderID = orderID;
            CustomerID = customerID;
            TableID = tableID;
            StaffID = staffID;
            TotalAmount = totalAmount;
            Status = status;
            PaymentStatus = paymentStatus;
            PaymentMethod = paymentMethod;
            Notes = notes;
        }

       




    }
}
