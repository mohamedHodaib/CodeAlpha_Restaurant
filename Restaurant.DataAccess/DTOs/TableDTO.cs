using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs
{
    public enum enTableStatus:byte { Available = 1, Reserved, Occupied, Cleaning, Maintenance };
    public class TableDTO
    {


        public int ID { get; set; }

        [Required(ErrorMessage = "Table Number Is Required")]
        [StringLength(255, MinimumLength = 4
            , ErrorMessage = "Table Number Characters must between 4 to 50 characters")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Capacity Is Required")]
        [Range(2, 10, ErrorMessage = "Capacity must be between 2 to ten Characters.")]
        public int Capacity { get; set; }
        public enTableStatus CurrentStatus { get; set; }

        [Required(ErrorMessage = "Table Number Is Required")]
        [StringLength(100, MinimumLength = 4
            , ErrorMessage = "Table Location Characters must between 5 to 100 characters")]
        public string Location { get; set; }

        public TableDTO() 
        {
            ID = 0;
            Number = string.Empty;
            Capacity = 0;
            CurrentStatus = enTableStatus.Available;
            Location = string.Empty;
        }


        public TableDTO(int iD, string number, int capacity, enTableStatus currentStatus, string location)
        {
            ID = iD;
            Number = number;
            Capacity = capacity;
            CurrentStatus = currentStatus;
            Location = location;
        }
    }
}
