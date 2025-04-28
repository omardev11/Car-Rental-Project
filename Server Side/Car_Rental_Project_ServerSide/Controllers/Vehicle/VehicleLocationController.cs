using CarRentalBusinessLayer;
using CarRentalBusinessLayer.Vehicle;
using CarRentalDataLayer.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Car_Rental_Project_ServerSide.Controllers.Vehicle
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleLocationController : ControllerBase
    {
        private readonly BusinessLayerInterfaces.ILocationBusiness _locationBusiness;

        public VehicleLocationController(BusinessLayerInterfaces.ILocationBusiness locationBusiness)
        {
            _locationBusiness = locationBusiness;
        }

        [HttpPost("Location", Name = "AddLocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddNewLocation(DTO.LocationDTO newLocation)
        {
            if (newLocation == null || newLocation.LocationInfor == null)
            {
                return BadRequest("The object of Location or LocationInfo cannot be null.");
            }

            _locationBusiness.Initialize(newLocation);
            if (_locationBusiness.Save())
            {
                return Ok("Location added successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error adding new Location." });
            }
        }

        [HttpPut("Location/Update", Name = "UpdateLocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateLocation(DTO.LocationDTO updatedLocation)
        {
            if (updatedLocation == null || updatedLocation.LocationInfor == null)
            {
                return BadRequest("The object of Location or LocationInfo cannot be null.");
            }

            _locationBusiness.Initialize(updatedLocation, LocationBusiness.enMode.UpdateMode);
            if (_locationBusiness.Save())
            {
                return Ok("Location updated successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error updating Location." });
            }
        }

        [HttpDelete("Location/Delete/{locationId}", Name = "DeleteLocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteLocation(int locationId)
        {
            if (locationId < 1)
            {
                return BadRequest("The LocationId is not valid.");
            }

            _locationBusiness.Initialize(new DTO.LocationDTO(), LocationBusiness.enMode.DeleteMode);
            _locationBusiness.LocationInfo.LocationId = locationId;
            if (_locationBusiness.Save())
            {
                return Ok("Location deleted successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error deleting Location." });
            }
        }

        [HttpGet("Location/By", Name = "GetLocationInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<DTO.LocationDTO>> GetLocationInfo(string LocationName = "", string Address = "")
        {
            if (string.IsNullOrEmpty(LocationName) && string.IsNullOrEmpty(Address))
            {
                return BadRequest("The LocationId and LocationName are not valid.");
            }

            List<DTO.LocationDTO> result = _locationBusiness.GetAllLocationsByLocationNameOrAdress(LocationName, Address);

            try
            {
                if (result != null && result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "Location not found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("Locations/All", Name = "AllLocations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<DTO.LocationDTO>> GetAllLocations()
        {
            List<DTO.LocationDTO> result = _locationBusiness.GetAllLocations();

            try
            {
                if (result != null && result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "No locations found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
