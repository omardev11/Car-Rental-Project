using CarRentalBusinessLayer.Vehicle;
using CarRentalDataLayer.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CarRentalBusinessLayer.BusinessLayerInterfaces;

namespace Car_Rental_Project_ServerSide.Controllers.Vehicle
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleStatusController : ControllerBase
    {
        private readonly IVehicleStatusBusiness _vehicleStatusBusiness;
        public VehicleStatusController(IVehicleStatusBusiness vehicleStatusBusiness)
        {
            _vehicleStatusBusiness = vehicleStatusBusiness;
        }

        [HttpPost("VehicleStatus", Name = "AddVehicleStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddNewVehicleStatus(DTO.VehicleStatusDTO newVehicleStatus)
        {
            if (newVehicleStatus == null)
            {
                return BadRequest("The object of VehicleStatus cannot be null.");
            }

            _vehicleStatusBusiness.Initialize(newVehicleStatus);
            if (_vehicleStatusBusiness.Save())
            {
                return Ok("Vehicle status added successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error adding new VehicleStatus." });
            }
        }

        [HttpPut("VehicleStatus/Update", Name = "UpdateVehicleStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateVehicleStatus(DTO.VehicleStatusDTO updatedVehicleStatus)
        {
            if (updatedVehicleStatus == null)
            {
                return BadRequest("The object of VehicleStatus cannot be null.");
            }

            _vehicleStatusBusiness.Initialize(updatedVehicleStatus, VehicleStatusBusiness.enMode.UpdateMode);
            if (_vehicleStatusBusiness.Save())
            {
                return Ok("Vehicle status updated successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error updating VehicleStatus." });
            }
        }

        [HttpDelete("VehicleStatus/Delete/{vehicleStatusId}", Name = "DeleteVehicleStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteVehicleStatus(int vehicleStatusId)
        {
            if (vehicleStatusId < 1)
            {
                return BadRequest("The VehicleStatusId is not valid.");
            }

            _vehicleStatusBusiness.Initialize(new DTO.VehicleStatusDTO(), VehicleStatusBusiness.enMode.DeleteMode);
            _vehicleStatusBusiness.VehicleStatusInfo.VehicleStatusId = vehicleStatusId;
            if (_vehicleStatusBusiness.Save())
            {
                return Ok("Vehicle status deleted successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error deleting VehicleStatus." });
            }
        }

        [HttpGet("Vehicle/StatusBy", Name = "GetVehicleStatusInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DTO.VehicleStatusDTO> GetVehicleStatusInfo(int? Vehiclestausid = null ,string StatusName = "")
        {
            if (Vehiclestausid < 1 && string.IsNullOrEmpty(StatusName))
            {
                return BadRequest("The VehicleStatusId and StatusName is not valid.");
            }

            DTO.VehicleStatusDTO result = _vehicleStatusBusiness.GetVehicleStatusInfoBy(Vehiclestausid, StatusName);

            try
            {
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "Vehicle status not found." });
                }
            }
            catch (Exception)
            {
                // Log exception details (for internal debugging)
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("Status/All", Name = "AllVehicleStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DTO.VehicleStatusDTO> GetAllVehicleStatus()
        {
           
            List<DTO.VehicleStatusDTO> result = _vehicleStatusBusiness.GetAllVehicleStatus();

            try
            {
                if (result != null && result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "Vehicle status not found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
