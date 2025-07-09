using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Business;
using Restaurant.DataAccess.DTOs;

namespace Restaurant.Api.Controllers
{
    [Route("api/ReservationController")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        [HttpPost(Name = "AddNewReservationRecord")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddNewReservationRecord(ReservationDTO newReservationRecordDTO)
        {
            if (newReservationRecordDTO == null)
                return BadRequest("Reservation Record Cannot Be null");

            Reservation reservation = new Reservation(newReservationRecordDTO);

            var result = await reservation.Save();

            if (!result.Success)
            {
                return this.HandleFailureResult(result);
            }

            newReservationRecordDTO.ReservationID = reservation.ReservationID;

            return CreatedAtRoute("GetReservationRecordByReservationID"
                , new { id = newReservationRecordDTO.ReservationID }, newReservationRecordDTO);
        }


        [HttpGet("GetAllReservations/{pageNumber}/{pageSize}"
           , Name = "GetAllReservations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAllReservations(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<ReservationDTO>> findResult
                = await Reservation.GetAllReservations(pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                "There is no Reservation Records."
                : findResult.Data);
        }



        [HttpGet("GetReservationRecordByReservationID/{id}", Name = "GetReservationRecordByReservationID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetReservationRecordByReservationID(int id)
        {

            if (id < 0) return BadRequest($"Not Accepted ID : {id}");

            BusinessResult<Reservation> findResult = await Reservation.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.reservationDTO);
        }





        [HttpGet("GetPastReservations/{pageNumber}/{pageSize}"
            , Name = "GetPastReservations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetPastReservations(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<ReservationDTO>> findResult
                = await Reservation.GetPastReservations(pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no Reservations in the past."
                : findResult.Data);
        }


        [HttpGet("GetUpcomingReservations/{pageNumber}/{pageSize}"
           , Name = "GetUpcomingReservations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetUpcommingReservations(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<ReservationDTO>> findResult
                = await Reservation.GetUpcommingReservations(pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no Reservations in the Future."
                : findResult.Data);
        }


        [HttpGet("GetReservationsByCustomerID/{customerID}/{pageNumber}/{pageSize}"
        , Name = "GetReservationsByCustomerID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetReservationsByCustomerID(int customerID, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<ReservationDTO>> findResult
                = await Reservation.GetReservationsByCustomerID(customerID, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"Customer With ID {customerID} not have reservations."
                : findResult.Data);
        }

        [HttpGet("GetReservationsByTableID/{tableID}/{pageNumber}/{pageSize}"
        , Name = "GetReservationsByTableID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetReservationsByTableID(int tableID, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<ReservationDTO>> findResult
                = await Reservation.GetReservationsByTableID(tableID, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"Table With ID {tableID} not have reservations."
                : findResult.Data);
        }

        [HttpGet("GetReservationsByNumberOfGuests/{numberOfGuests}/{pageNumber}/{pageSize}"
        , Name = "GetReservationsByNumberOfGuests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetReservationsByNumberOfGuests(int numberOfGuests, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<ReservationDTO>> findResult
                = await Reservation.GetReservationsByNumberOfGuests(numberOfGuests, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no Reservations With the {numberOfGuests} Guests ."
                : findResult.Data);
        }


        [HttpGet("GetReservationsByStatus/{status}/{pageNumber}/{pageSize}"
        , Name = "GetReservationsByStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetReservationsByStatus(enReservationStatus status,
            int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<ReservationDTO>> findResult
                = await Reservation.GetReservationsByStatus(status, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no Reservations With status {status}."
                : findResult.Data);
        }



        [HttpGet("GetReservationsWithinPeriod/{startDate}/{endDate}/{pageNumber}/{pageSize}"
         , Name = "GetReservationsWithinPeriod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetReservationsWithinPeriod(DateOnly startDate, DateOnly endDate,
             int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<ReservationDTO>> findResult
                = await Reservation.GetReservationsWithinPeriod(startDate
                , endDate, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is no Reservations from {startDate} to {endDate}."
                : findResult.Data);
        }





        [HttpPut("UpdateReservationRecord/{id}", Name = "UpdateReservationRecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, ReservationDTO reservationDTO)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");

            BusinessResult<Reservation> findResult = await Reservation.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            findResult.Data.CustomerID = reservationDTO.CustomerID;
            findResult.Data.TableID = reservationDTO.TableID;
            findResult.Data.ReservationTime = reservationDTO.ReservationTime;
            findResult.Data.NumberOfGuests = reservationDTO.NumberOfGuests;
            findResult.Data.SpecialRequests = reservationDTO.SpecialRequests;
            findResult.Data.Status = reservationDTO.Status;


            BusinessResult<bool> saveResult = await findResult.Data.Save();

            if (!saveResult.Success)
                return this.HandleFailureResult(saveResult);

            return Ok(saveResult.Message);

        }


        [HttpPut("UpdateReservationStatus/{id}/{status}", Name = "UpdateReservationStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateReservationStatus(int id, enReservationStatus status)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");


            BusinessResult<bool> UpdateResult = await Reservation.UpdateStatus(id, status);

            if (!UpdateResult.Success)
                return this.HandleFailureResult(UpdateResult);

            return Ok(UpdateResult.Message);

        }

        [HttpPut("UpdateReservationTime/{id}/{time}", Name = "UpdateReservationTime")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateReservationTime(int id, DateTime time)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");


            BusinessResult<bool> UpdateResult = await Reservation.UpdateTime(id, time);

            if (!UpdateResult.Success)
                return this.HandleFailureResult(UpdateResult);

            return Ok(UpdateResult.Message);

        }


        [HttpPut("UpdateReservationNumberOfGuests/{id}/{numberOfGuests}", Name = "UpdateReservationNumberOfGuests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateReservationNumberOfGuests(int id, int numberOfGuests)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");


            BusinessResult<bool> UpdateResult = await Reservation.UpdateNumberOfGuests(id, numberOfGuests);

            if (!UpdateResult.Success)
                return this.HandleFailureResult(UpdateResult);

            return Ok(UpdateResult.Message);

        }


        [HttpPut("UpdateReservationTable/{id}/{tableID}", Name = "UpdateReservationTable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateReservationTable(int id, int tableID)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");


            BusinessResult<bool> UpdateResult = await Reservation.UpdateReservedTable(id, tableID);

            if (!UpdateResult.Success)
                return this.HandleFailureResult(UpdateResult);

            return Ok(UpdateResult.Message);

        }


        [HttpPut("UpdateReservationSpecialRequests/{id}/{specialRequests}"
            , Name = "UpdateReservationSpecialRequests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateReservationSpecialRequests(int id, string specialRequests)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");


            BusinessResult<bool> UpdateResult = await Reservation.UpdateSpecialRequests(id, specialRequests);

            if (!UpdateResult.Success)
                return this.HandleFailureResult(UpdateResult);

            return Ok(UpdateResult.Message);

        }



        [HttpDelete("DeleteReservationRecord/{id}", Name = "DeleteReservationRecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Not Accepted Id");

            BusinessResult<Reservation> findResult = await Reservation.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);


            BusinessResult<bool> deleteResult = await findResult.Data.Delete();

            if (!deleteResult.Success)
                return this.HandleFailureResult(deleteResult);

            return Ok(deleteResult.Message);

        }

    }
}
