using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs
{
    public class MenueItemDTO
    {
        public int ID { get; set; }

        [Required(ErrorMessage ="Item Name Is Required")]
        [StringLength(255,MinimumLength = 2
            ,ErrorMessage ="Item Name Must Not be Empty and cannot exceed 255 ")]
        public string Name { get; set; }

        [StringLength(4000, ErrorMessage = "Item Name cannot exceed 255.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999999.99.")]
        public decimal Price { get; set; }


        [Required(ErrorMessage = "Category is required.")]
        [StringLength(100, MinimumLength = 4
            , ErrorMessage = "Item Category Must Not be Empty and cannot exceed 100.")]
        public string Category { get; set; }


        public bool IsAvailable { get; set; }


        public MenueItemDTO()
        {
            ID = 0;
            Name = string.Empty;
            Description = null;
            Price = 0;
            Category = string.Empty;
            IsAvailable = true;
        }

        public MenueItemDTO(int id, string name, string? description,
            decimal price, string category,bool isAvailable)
        {
            ID = id;
            Name = name;
            Description = description;
            Price = price;
            Category = category;
            IsAvailable = isAvailable;
        }
    }
}