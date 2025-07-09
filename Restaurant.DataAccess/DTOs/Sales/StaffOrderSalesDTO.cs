using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs.Sales
{
    public class StaffOrderSalesDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal TotalSalesAmount { get; set; }
    }
}
