using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs.Sales
{
    public class OrderYearlySalesDTO
    {
        public int Year { get; set; }
        public DateOnly StartDate { get; set; }
        public decimal SalesAmount { get; set; }
    }
}
