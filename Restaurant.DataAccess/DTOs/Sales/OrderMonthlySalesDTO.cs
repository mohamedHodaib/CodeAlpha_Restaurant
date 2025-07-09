using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs.Sales
{
    public class OrderMonthlySalesDTO : OrderYearlySalesDTO
    {
        public int Month {  get; set; }
    }
}
