using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs.Sales
{
    public class OrderItemSalesDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string Category { get; set; }
        public int TotalQuantitySold { get; set; }
        public decimal TotalSalesAmount { get; set; }
    }
}
