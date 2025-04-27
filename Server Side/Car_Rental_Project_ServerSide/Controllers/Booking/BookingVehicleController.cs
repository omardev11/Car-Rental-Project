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
    public class BookingVehicleController : ControllerBase
    {
        private readonly IBookingVehicleBusiness _BookingVehicleBusiness;
        
        public BookingVehicleController(IBookingVehicleBusiness bookingVehiclebusiness)
        {
            _BookingVehicleBusiness = bookingVehiclebusiness;
        }

        [HttpPost("BookingVehicle", Name = "AddBookingVehicle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddNewBookingVehicle(DTO.AddingBookingInfoDTO newBookingVehicle)
        {
            if (newBookingVehicle.UserViewBookingInfo == null || newBookingVehicle.BookingInfoFromCustomer == null)
            {
                return BadRequest("The object of BookingVehicle can't be null.");
            }

            _BookingVehicleBusiness.Initialize(newBookingVehicle);
            if (_BookingVehicleBusiness.Save())
            {
                return Ok("BookingVehicle added successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error adding new BookingVehicle." });
            }
        }


        [HttpPut("BookingVehicle/Update/FromCustomer/{bookingId}", Name = "UpdateBookingVehicleFromCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateBookingVehicleFromCustomer(
            int bookingId,
             [FromQuery] DateTime? RentalStartDate,
             [FromQuery] DateTime? RentalEndDate,
             [FromQuery] string? PickupLocation,
             [FromQuery] string? DropOffLocation
            )
        {
            if (bookingId <= 0)
            {
                return BadRequest("The Booking Id Must Be Valid Number");
            }

            var updateBookingVehicleInfo = new DTO.AddingBookingInfoDTO();
            updateBookingVehicleInfo.BookingInfoFromCustomer = new DTO.BookingInfoFromCustomer(RentalStartDate, RentalEndDate, PickupLocation, DropOffLocation);  
            updateBookingVehicleInfo.UserViewBookingInfo.BookingId = bookingId;

            _BookingVehicleBusiness.Initialize(updateBookingVehicleInfo, BookingVehicleBusiness.enMode.UpdateFromCustomerMode);

            if (_BookingVehicleBusiness.Save())
            {
                return Ok("BookingVehicle updated successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error updating BookingVehicle." });
            }
        }


        [HttpPut("BookingVehicle/Update/FromUser", Name = "UpdateBookingVehicleFromUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateBookingVehicleFromUser(DTO.UserViewBookingInfo updatedBookingVehicle)
        {
            if (updatedBookingVehicle == null)
            {
                return BadRequest("The object of BookingVehicle can't be null.");
            }

            var updateBookingVehicleInfo = new DTO.AddingBookingInfoDTO();
            updateBookingVehicleInfo.UserViewBookingInfo = updatedBookingVehicle;

            _BookingVehicleBusiness.Initialize(updateBookingVehicleInfo, BookingVehicleBusiness.enMode.UpdateFromUserMode);

            if (_BookingVehicleBusiness.Save())
            {
                return Ok("BookingVehicle updated successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error updating BookingVehicle." });
            }
        }


        [HttpDelete("BookingVehicle/Cancel", Name = "CancelBooking")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult CancelBooking(int bookingId)
        {
            if (bookingId <= 0)
            {
                return BadRequest("The BookingVehicleId Must Be Valid.");
            }

            var result = _BookingVehicleBusiness.CancelBooking(bookingId);
          

            if (result)
            {
                return Ok("BookingVehicle Canceled successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error Canceling BookingVehicle." });
            }
        }

 

        [HttpGet("BookingVehicle/ByBookingId", Name = "GetBookingVehicleByBookingId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DTO.CustomerViewBookingInfo> GetBookingVehicleByBookingId(int bookingId)
        {
            if  (bookingId <= 0)
            {
                return BadRequest("The BookingVehicleId Is not valid.");
            }

            var result = _BookingVehicleBusiness.GetBookingVehicleByBookingId(bookingId);

            try
            {
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "BookingVehicle not found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }


        [HttpGet("AllBookingVehicle/ByCustomerId", Name = "GetAllBookingVehiclesForACustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult< List<DTO.MiniInfoOfBookingDTO.SendingInfo>> GetAllBookingVehiclesForACustomer(int customerId)
        {
            if (customerId <= 0)
            {
                return BadRequest("The CustomerId Is not valid.");
            }

            List<DTO.MiniInfoOfBookingDTO.SendingInfo> result = _BookingVehicleBusiness.GetAllBookingVehiclesForACustomer(customerId);

            try
            {
                if (result != null && result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "BookingVehicle not found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("AllBookingVehicleWaitingApproval", Name = "GetAllBookingWaitingApproval")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<DTO.MiniInfoOfBookingDTO.SendingInfo>> GetAllBookingWaitingApproval()
        {
         
            List<DTO.MiniInfoOfBookingDTO> result = _BookingVehicleBusiness.GetAllBookingWaitingApproval();

            try
            {
                if (result != null && result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "BookingVehicle not found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
