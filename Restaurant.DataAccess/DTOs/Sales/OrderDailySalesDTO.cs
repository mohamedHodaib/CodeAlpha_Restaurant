using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs.Sales
{
    public class OrderDailySalesDTO : OrderWeeklySalesDTO
    {
        public int Day {  get; set; }
    }
}
