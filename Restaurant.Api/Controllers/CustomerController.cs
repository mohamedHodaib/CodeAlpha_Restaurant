using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Business;
using Restaurant.DataAccess.DTOs;
using System.Text.RegularExpressions;

namespace Restaurant.Api.Controllers
{
    [Route("api/CustomerController")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpPost(Name = "AddNewCustomerRecord")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddNewCustomerRecord(CustomerDTO newCustomerRecordDTO)
        {
            if (newCustomerRecordDTO == null)
                return BadRequest("Customer Record Cannot Be null");

            Customer menueItem = new Customer(newCustomerRecordDTO);

            var result = await menueItem.Save();

            if (!result.Success)
            {
                return this.HandleFailureResult(result);

            }

            newCustomerRecordDTO.ID = menueItem.ID;
            newCustomerRecordDTO.LoyalityPoints = menueItem.LoyalityPoints;

            return CreatedAtAction(nameof(GetCustomerRecordByCustomerID)
                , new { id = newCustomerRecordDTO.ID }, newCustomerRecordDTO);
        }


        [HttpGet("GetAllCustomers/{pageNumber}/{pageSize}"
           , Name = "GetAllCustomers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAllCustomers(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<CustomerDTO>> findResult
                = await Customer.GetAllCustomers(pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                "There is no Customer Records."
                : findResult.Data);
        }



        [HttpGet("GetCustomerRecordByCustomerID/{id}", Name = "GetCustomerRecordByCustomerID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetCustomerRecordByCustomerID(int id)
        {

            if (id < 0) return BadRequest($"Not Accepted ID : {id}");

            BusinessResult<Customer> findResult = await Customer.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.customerDTO);
        }


        [HttpGet("GetCustomerRecordByPhoneNumber/{phoneNumber}", Name = "GetCustomerRecordByPhoneNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetCustomerRecordByPhoneNumber(string phoneNumber)
        {

            if (string.IsNullOrEmpty(phoneNumber) || 
                !Regex.IsMatch(phoneNumber, @"^(\+20|01)[0125][0-9]{8}$")) 
                return BadRequest($"Not Valid Number");

            BusinessResult<Customer> findResult = await Customer.GetCustomerByPhoneNumber(phoneNumber);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.customerDTO);
        }

        [HttpGet("GetCustomerRecordByEmail/{email}", Name = "GetCustomerRecordByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetCustomerRecordByEmail(string email)
        {

            if (string.IsNullOrEmpty(email) ||
                !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return BadRequest($"Not Valid Email");

            BusinessResult<Customer> findResult = await Customer.GetCustomerByEmail(email);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.customerDTO);
        }


        [HttpGet("GetCustomerItemsByFirstName/{firstName}/{pageNumber}/{pageSize}"
            , Name = "GetCustomerItemsByFirstName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetCustomerItemsByFirstName(string firstName, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1 ||
                string.IsNullOrEmpty(firstName) || firstName.Length > 100)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<CustomerDTO>> findResult
                = await Customer.GetCustomersByFirstName(firstName, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is not Customer Record with first Name  {firstName}."
                : findResult.Data);
        }


        [HttpGet("GetCustomerItemsByLastName/{lastName}/{pageNumber}/{pageSize}"
           , Name = "GetCustomerItemsByLastName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetCustomerItemsByLastName(string lastName, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1 ||
                string.IsNullOrEmpty(lastName) || lastName.Length > 100)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<CustomerDTO>> findResult
                = await Customer.GetCustomersByLastName(lastName, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is not Customer Record with last Name  {lastName}."
                : findResult.Data);
        }

        [HttpGet("GetCustomerItemsByFirstAndLastName/{firstName}/{lastName}/{pageNumber}/{pageSize}"
    , Name = "GetCustomerItemsByFirstAndLastName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetCustomerItemsByFirstAndLastName(string firstName, string lastName
            , int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1 ||
                string.IsNullOrEmpty(firstName) || firstName.Length > 100
                || string.IsNullOrEmpty(lastName) || lastName.Length > 100)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<CustomerDTO>> findResult
                = await Customer.GetCustomersByFirstNameAndLastName(firstName,lastName, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is not Customer Record with first Name  {firstName} and LastName {lastName}."
                : findResult.Data);
        }

        [HttpGet("GetCustomerItemsByLoyalityPoints/{loyalityPoints}/{pageNumber}/{pageSize}"
            , Name = "GetCustomerItemsByLoyalityPoints")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetCustomerItemsByLoyalityPoints(int loyalityPoints
            , int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1 || loyalityPoints < 0)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<CustomerDTO>> findResult
                = await Customer.GetCustomersByLoyaltyPoints(loyalityPoints, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is not Customer Record with {loyalityPoints} Loyality Points."
                : findResult.Data);
        }


        [HttpPut("UpdateCustomerRecord/{id}", Name = "UpdateCustomerRecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, CustomerDTO customerDTO)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");

            BusinessResult<Customer> findResult = await Customer.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);


            findResult.Data.FirstName = customerDTO.FirstName;
            findResult.Data.LastName = customerDTO.LastName;
            findResult.Data.Phone = customerDTO.Phone;
            findResult.Data.Email = customerDTO.Email;

            BusinessResult<bool> saveResult = await findResult.Data.Save();

            if (!saveResult.Success)
                return this.HandleFailureResult(saveResult);

            return Ok(saveResult.Message);

        }

    

        [HttpDelete("DeleteCustomerRecord/{id}", Name = "DeleteCustomerRecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Not Accepted Id");

            BusinessResult<Customer> findResult = await Customer.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);


            BusinessResult<bool> deleteResult = await findResult.Data.Delete();

            if (!deleteResult.Success)
                return this.HandleFailureResult(deleteResult);

            return Ok(deleteResult.Message);

        }

    }
}
