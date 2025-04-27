using CarRentalBusinessLayer.Booking;
using CarRentalBusinessLayer.Vehicle;
using CarRentalDataLayer.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CarRentalBusinessLayer.BusinessLayerInterfaces;

namespace Car_Rental_Project_ServerSide.Controllers.Booking
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingStatusController : ControllerBase
    {
        private readonly IBookingStatusBusiness _BookingStatusBusiness; 
        public BookingStatusController(IBookingStatusBusiness bookinStatusbusiness)
        {
            _BookingStatusBusiness = bookinStatusbusiness;
        }

        [HttpPost("BookingStatus", Name = "AddBookingStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddNewBookingStatus(DTO.BookingStatusDTO newBookingStatus)
        {
            if (newBookingStatus == null)
            {
                return BadRequest("The object of BookingStatus cannot be null.");
            }

            _BookingStatusBusiness.Initialize(newBookingStatus);
            if (_BookingStatusBusiness.Save())
            {
                return Ok("Booking status added successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error adding new BookingStatus." });
            }
        }

        [HttpPut("BookingStatus/Update", Name = "UpdateBookingStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateBookingStatus(DTO.BookingStatusDTO updatedBookingStatus)
        {
            if (updatedBookingStatus == null)
            {
                return BadRequest("The object of BookingStatus cannot be null.");
            }

            _BookingStatusBusiness.Initialize(updatedBookingStatus, BookingStatusBusiness.enMode.UpdateMode);
            if (_BookingStatusBusiness.Save())
            {
                return Ok("Booking status updated successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error updating BookingStatus." });
            }
        }

        [HttpDelete("BookingStatus/Delete/{BookingStatusId}", Name = "DeleteBookingStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteBookingStatus(int BookingStatusId)
        {
            if (BookingStatusId < 1)
            {
                return BadRequest("The BookingStatusId is not valid.");
            }


            bool IsDeleted = _BookingStatusBusiness.DeleteVehicleStatus(BookingStatusId);
            if (IsDeleted)
            {
                return Ok("Booking status deleted successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error deleting BookingStatus." });
            }
        }

        [HttpGet("BookingStatusBy", Name = "GetBookingStatusInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DTO.BookingStatusDTO> GetBookingStatusInfo(int? Vehiclestausid = null, string StatusName = "")
        {
            if (Vehiclestausid == null && string.IsNullOrEmpty(StatusName))
            {
                return BadRequest("The BookingStatusId and StatusName is not valid.");
            }

            DTO.BookingStatusDTO result = _BookingStatusBusiness.GetBookingStatusBy(Vehiclestausid, StatusName);

            try
            {
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "Booking status not found." });
                }
            }
            catch (Exception)
            {
                // Log exception details (for internal debugging)
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("BookingStatus/All", Name = "AllBookingStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DTO.BookingStatusDTO> GetAllBookingStatus()
        {

            List<DTO.BookingStatusDTO> result = _BookingStatusBusiness.GetAllBookingStatus();

            try
            {
                if (result != null && result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "Booking status not found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
