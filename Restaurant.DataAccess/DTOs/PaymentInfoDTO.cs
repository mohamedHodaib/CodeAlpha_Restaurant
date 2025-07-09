using Restaurant.DataAccess.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs
{
    public class PaymentInfoDTO
    {
        public int OrderID { get; set; }
        public enPaymentMethod PaymentMethod { get; set; }
        public decimal amount { get; set; }
    }
}
