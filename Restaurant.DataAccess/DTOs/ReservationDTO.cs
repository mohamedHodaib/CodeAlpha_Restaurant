using Restaurant.DataAccess.DTOs.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess.DTOs
{
     public enum enReservationStatus:byte { Pending=1,Confirmed,Seated,Completed,Cancelled,NoShow}
    public class ReservationDTO
    {
        public int ReservationID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Customer Id Must be Positive")]
        public int? CustomerID { get; set; }

        [Required(ErrorMessage = "Table Id  Is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Table Id Must be Positive")]
        public int TableID { get; set; }

        [Required(ErrorMessage = "Reservation Time  Is Required")]
        [DateTime(ErrorMessage = "Reservation Time Is Not Valid")]
        public DateTime ReservationTime { get; set; }

        [Required(ErrorMessage = "Number Of Guests Is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of Guests Must be Positive")]
        public int NumberOfGuests   { get; set; }
        public enReservationStatus? Status { get; set; }

        [StringLength(4000,ErrorMessage = "Special Requests must not exceed 4000")]
        public string? SpecialRequests { get; set; }


        public ReservationDTO()
        {
            ReservationID = 0;
            CustomerID = 0;
            TableID = 0;
            ReservationTime = DateTime.MinValue;
            Status = enReservationStatus.Pending;
            SpecialRequests = null;
            NumberOfGuests = 0;
        }


        public ReservationDTO(int reservationID, int? customerID, int tableID, DateTime reservationTime,
            int numberOfGuests, enReservationStatus? status, string? specialRequests)
        {
            ReservationID = reservationID;
            CustomerID = customerID;
            TableID = tableID;
            ReservationTime = reservationTime;
            NumberOfGuests = numberOfGuests;
            Status = status;
            SpecialRequests = specialRequests;
        }




    }
}
