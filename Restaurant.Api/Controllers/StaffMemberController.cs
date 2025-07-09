using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Business;
using Restaurant.DataAccess.DTOs;
using Restaurant.DataAccess.DTOs.StaffMember;
using System.Text.RegularExpressions;
using static Restaurant.DataAccess.DTOs.StaffMember.StaffMemberOutputDTO;

namespace Restaurant.Api.Controllers
{
    [Route("api/StaffMemberController")]
    [ApiController]
    public class StaffMemberController : ControllerBase
    {
        [HttpPost(Name = "AddNewStaffMemberRecord")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddNewStaffMemberRecord(StaffMemberInputDTO newStaffMemberRecordDTO)
        {
            if (newStaffMemberRecordDTO == null)
                return BadRequest("Staff Member Record Cannot Be null");

            StaffMember staffMember = new StaffMember(newStaffMemberRecordDTO);

            var result = await staffMember.Save();

            if (!result.Success)
            {
                return this.HandleFailureResult(result);

            }

            newStaffMemberRecordDTO.ID = staffMember.ID;

            StaffMemberOutputDTO staffMemberOutputDTO =
                 new StaffMemberOutputDTO
                 {
                     ID = staffMember.ID,
                     Role = staffMember.Role,
                     FirstName = staffMember.FirstName,
                     LastName = staffMember.LastName,
                     Email = staffMember.Email,
                     Phone = staffMember.Phone,
                     IsActive = staffMember.IsActive

                 };

            return CreatedAtAction(nameof(GetStaffMemberRecordByStaffMemberID)
                , new { id = newStaffMemberRecordDTO.ID }, staffMemberOutputDTO);
        }


        [HttpPost("Authenticate/{userName}/{password}",Name = "Authenticate")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Authenticate(string userName,string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return BadRequest("Not Accepted Data");

            BusinessResult<bool> authenticationResult = await StaffMember.Authenticate(userName, password);

            if(!authenticationResult.Success)
                return this.HandleFailureResult(authenticationResult);

            return Ok(authenticationResult.Message);
        }


        [HttpGet("GetAllStaffMembers/{pageNumber}/{pageSize}"
           , Name = "GetAllStaffMembers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAllStaffMembers(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<StaffMemberOutputDTO>> findResult
                = await StaffMember.GetAllStaffMembers(pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                "There is no Staff Member Records."
                : findResult.Data);
        }



        [HttpGet("GetStaffMemberRecordByStaffMemberID/{id}", Name = "GetStaffMemberRecordByStaffMemberID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetStaffMemberRecordByStaffMemberID(int id)
        {

            if (id < 0) return BadRequest($"Not Accepted ID : {id}");

            BusinessResult<StaffMemberOutputDTO> findResult = await StaffMember.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data);
        }


        [HttpGet("GetStaffMemberRecordByPhoneNumber/{phoneNumber}", Name = "GetStaffMemberRecordByPhoneNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetStaffMemberRecordByPhoneNumber(string phoneNumber)
        {

            if (string.IsNullOrEmpty(phoneNumber) ||
                !Regex.IsMatch(phoneNumber, @"^(\+20|01)[0125][0-9]{8}$"))
                return BadRequest($"Not Valid Number");

            BusinessResult<StaffMemberOutputDTO> findResult = 
                await StaffMember.GetStaffMemberByPhoneNumber(phoneNumber);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data);
        }

        [HttpGet("GetStaffMemberRecordByEmail/{email}", Name = "GetStaffMemberRecordByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetStaffMemberRecordByEmail(string email)
        {

            if (string.IsNullOrEmpty(email) ||
                !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return BadRequest($"Not Valid Email");

            BusinessResult<StaffMemberOutputDTO> findResult = await StaffMember.GetStaffMemberByEmail(email);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data);
        }


        [HttpGet("GetStaffMemberItemsByFirstName/{firstName}/{pageNumber}/{pageSize}"
            , Name = "GetStaffMemberItemsByFirstName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetStaffMemberItemsByFirstName(string firstName, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1 ||
                string.IsNullOrEmpty(firstName) || firstName.Length > 100)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<StaffMemberOutputDTO>> findResult
                = await StaffMember.GetStaffMemberByFirstName(firstName, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is not StaffMember Record with first Name  {firstName}."
                : findResult.Data);
        }


        [HttpGet("GetStaffMemberItemsByLastName/{lastName}/{pageNumber}/{pageSize}"
           , Name = "GetStaffMemberItemsByLastName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetStaffMemberItemsByLastName(string lastName, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1 ||
                string.IsNullOrEmpty(lastName) || lastName.Length > 100)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<StaffMemberOutputDTO>> findResult
                = await StaffMember.GetStaffMemberByLastName(lastName, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is not StaffMember Record with last Name  {lastName}."
                : findResult.Data);
        }

        [HttpGet("GetStaffMemberItemsByFirstAndLastName/{firstName}/{lastName}/{pageNumber}/{pageSize}"
        , Name = "GetStaffMemberItemsByFirstAndLastName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetStaffMemberItemsByFirstName(string firstName, string lastName
            , int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1 ||
                string.IsNullOrEmpty(firstName) || firstName.Length > 100
                || string.IsNullOrEmpty(lastName) || lastName.Length > 100)
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<StaffMemberOutputDTO>> findResult
                = await StaffMember.GetStaffMemberByFirstNameAndLastName(firstName,lastName, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is not StaffMember Record with first Name  {firstName} and LastName {lastName}."
                : findResult.Data);
        }


        [HttpGet("GetStaffMemberItemsByRole/{role}/{pageNumber}/{pageSize}"
            , Name = "GetStaffMemberItemsByRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetStaffMemberItemsByRole(enRole role
            , int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1 )
                return BadRequest("Not Accepted Data");

            BusinessResult<PaginatedDTO<StaffMemberOutputDTO>> findResult
                = await StaffMember.GetStaffMembersByRole(role, pageNumber, pageSize);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            return Ok(findResult.Data.TotalItems == 0 ?
                $"There is not Staff Member Record with {role} Role."
                : findResult.Data);
        }


        [HttpPut("UpdateStaffMemberRecord/{id}", Name = "UpdateStaffMemberRecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, StaffMemberInputDTO staffMemberDTO)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");

            BusinessResult<StaffMemberOutputDTO> findResult = await StaffMember.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);

            findResult.Data.Role = staffMemberDTO.Role;
            findResult.Data.FirstName = staffMemberDTO.FirstName;
            findResult.Data.LastName = staffMemberDTO.LastName;
            findResult.Data.Phone = staffMemberDTO.Phone;
            findResult.Data.Email = staffMemberDTO.Email;

            staffMemberDTO.ID = id;

            StaffMember staffMember = new StaffMember(staffMemberDTO,StaffMember.enMode.Update);

            BusinessResult<bool> saveResult = await staffMember.Save();

            if (!saveResult.Success)
                return this.HandleFailureResult(saveResult);

            return Ok(saveResult.Message);

        }


        [HttpPut("UpdateStaffMemberStatus/{id}/{isActive}", Name = "UpdateStaffMemberStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStatus(int id, bool isActive)
        {
            if (id <= 0)
                return BadRequest("Not Accepted ID");     

            BusinessResult<bool> UpdateResult = isActive ?
              await StaffMember.Activate(id)
              : await StaffMember.DeActivate(id);

            if (!UpdateResult.Success)
                return this.HandleFailureResult(UpdateResult);

            return Ok(UpdateResult.Message);

        }


        [HttpPut("UpdateStaffMemberPassword/{id}/{password}", Name = "UpdateStaffMemberPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStaffMemberPassword(int id, string password)
        {
            if (id <= 0 || !Regex.IsMatch(password,
                @"^(?=.*[a-zA-Z].*[a-zA-Z])(?=.*\d.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':
                ""\\|,.<>/?~`].*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>/?~`])[A-Za-z0-9!@#
                $%^&*()_+\-=\[\]{};':""\\|,.<>/?~`]{6,}$"))
                return BadRequest("Not Accepted ID");

            BusinessResult<bool> UpdateResult = await StaffMember.UpdatePassword(id,password);

            if (!UpdateResult.Success)
                return this.HandleFailureResult(UpdateResult);

            return Ok(UpdateResult.Message);

        }



        [HttpDelete("DeleteStaffMemberRecord/{id}", Name = "DeleteStaffMemberRecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Not Accepted Id");

            BusinessResult<StaffMemberOutputDTO> findResult = await StaffMember.Find(id);

            if (!findResult.Success)
                return this.HandleFailureResult(findResult);


            BusinessResult<bool> deleteResult = await StaffMember.Delete(id);

            if (!deleteResult.Success)
                return this.HandleFailureResult(deleteResult);

            return Ok(deleteResult.Message);

        }

    }
}
