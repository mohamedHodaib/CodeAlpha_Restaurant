using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs
{
    public class AddNewOutputDTO
    {
        public int  ID {  get; set; }

        public DateTime CreatedAt { get; set; }


        public AddNewOutputDTO() 
        {
            ID = 0;
            CreatedAt = DateTime.MinValue;
        }

        public AddNewOutputDTO(int iD, DateTime createdAt, DateTime updatedAt)
        {
            ID = iD;
            CreatedAt = createdAt;
        }


    }
}
