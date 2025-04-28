using CarRentalBusinessLayer;
using CarRentalBusinessLayer.Vehicle;
using CarRentalDataLayer.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Car_Rental_Project_ServerSide.Controllers.Vehicle
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleReturnLogController : ControllerBase
    {
        private readonly BusinessLayerInterfaces.IVehicleReturnLogBusiness _vehicleReturnLogBusiness;

        public VehicleReturnLogController(BusinessLayerInterfaces.IVehicleReturnLogBusiness vehicleReturnLogBusiness)
        {
            _vehicleReturnLogBusiness = vehicleReturnLogBusiness;
        }

        [HttpPost("VehicleReturnLog", Name = "AddVehicleReturnLog")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]




        public ActionResult AddNewVehicleReturnLog(DTO.VehicleReturnLog newReturnLog)
        {
            if (newReturnLog == null)
            {
                return BadRequest("The object of VehicleReturnLog cannot be null.");
            }

            _vehicleReturnLogBusiness.Initialize(newReturnLog);
            if (_vehicleReturnLogBusiness.Save())
            {
                return Ok("Vehicle return log added successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error adding new VehicleReturnLog." });
            }
        }

        [HttpPut("VehicleReturnLog/Update/{ReturnId}", Name = "UpdateVehicleReturnLog")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateVehicleReturnLog(
              int ReturnId,
             [FromQuery] int? BookingId,
             [FromQuery] DateTime? ActualReturnDate,
             [FromQuery] string? FinalCheckNotes
            )
        {
            DTO.VehicleReturnLog updatedReturnLog = new DTO.VehicleReturnLog()
            {
                ReturnId = ReturnId,
                BookingId = BookingId,
                ActualReturnDate = ActualReturnDate,
                FinalCheckNotes = FinalCheckNotes
               
            };

           
            _vehicleReturnLogBusiness.Initialize(updatedReturnLog, VehicleReturnLogBusiness.enMode.UpdateMode);
            if (_vehicleReturnLogBusiness.Save())
            {
                return Ok("Vehicle return log updated successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error updating VehicleReturnLog." });
            }
        }

   



        [HttpGet("VehicleReturnLogsBy/All", Name = "AllVehicleReturnLogs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<DTO.VehicleReturnLog>> GetAllVehicleReturnLogs(
             [FromQuery] int? ReturnId,
             [FromQuery] int? BookingId,
             [FromQuery] DateTime? ActualReturnDate,
             [FromQuery] int? ActualRentalDays,
             [FromQuery] decimal? ActualTotalAmount
            )
        {
            DTO.VehicleReturnLog FiltiringReturnINfo = new DTO.VehicleReturnLog()
            {
                ReturnId = ReturnId,
                BookingId = BookingId,
                ActualTotalAmount = ActualTotalAmount,
                ActualReturnDate = ActualReturnDate,
                ActualRentalDays = ActualRentalDays,
            };
            List<DTO.VehicleReturnLog> result = _vehicleReturnLogBusiness.GetAllVehicleReturnLogsBy(FiltiringReturnINfo);

            try
            {
                if (result != null && result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "No vehicle return logs found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
