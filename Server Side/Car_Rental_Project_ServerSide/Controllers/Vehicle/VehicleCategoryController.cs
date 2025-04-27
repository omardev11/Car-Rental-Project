using CarRentalBusinessLayer.Vehicle;
using CarRentalDataLayer.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CarRentalBusinessLayer.BusinessLayerInterfaces;

namespace Car_Rental_Project_ServerSide.Controllers.Vehicle
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleCategoryController : ControllerBase
    {
        private readonly IVehicleCategoryBusiness _vehicleCategoryBusiness;

        public VehicleCategoryController(IVehicleCategoryBusiness vehicleCategoryBusiness)
        {
            _vehicleCategoryBusiness = vehicleCategoryBusiness;
        }

        [HttpPost("VehicleCategory", Name = "AddVehicleCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddNewVehicleCategory(DTO.VehicleCategoryDTO newVehicleCategory)
        {
            if (newVehicleCategory == null)
            {
                return BadRequest("The object of VehicleCategory cannot be null.");
            }

            _vehicleCategoryBusiness.Initialize(newVehicleCategory);
            if (_vehicleCategoryBusiness.Save())
            {
                return Ok("Vehicle category added successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error adding new VehicleCategory." });
            }
        }

        [HttpPut("VehicleCategory/Update", Name = "UpdateVehicleCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateVehicleCategory(DTO.VehicleCategoryDTO updatedVehicleCategory)
        {
            if (updatedVehicleCategory == null)
            {
                return BadRequest("The object of VehicleCategory cannot be null.");
            }

            _vehicleCategoryBusiness.Initialize(updatedVehicleCategory, VehicleCategoryBusiness.enMode.UpdateMode);
            if (_vehicleCategoryBusiness.Save())
            {
                return Ok("Vehicle category updated successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error updating VehicleCategory." });
            }
        }

        [HttpDelete("VehicleCategory/Delete/{vehicleCategoryId}", Name = "DeleteVehicleCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteVehicleCategory(int vehicleCategoryId)
        {
            if (vehicleCategoryId < 1)
            {
                return BadRequest("The VehicleCategoryId is not valid.");
            }

            _vehicleCategoryBusiness.Initialize(new DTO.VehicleCategoryDTO(), VehicleCategoryBusiness.enMode.DeleteMode);
            _vehicleCategoryBusiness.VehicleCategoryInfo.VehicleCategoryId = vehicleCategoryId;
            if (_vehicleCategoryBusiness.Save())
            {
                return Ok("Vehicle category deleted successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error deleting VehicleCategory." });
            }
        }

        [HttpGet("Vehicle/CategoryBy", Name = "GetVehicleCategoryInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DTO.VehicleCategoryDTO> GetVehicleCategoryInfo(int? vehicleCategoryId = null, string categoryName = "")
        {
            if (vehicleCategoryId < 1 && string.IsNullOrEmpty(categoryName))
            {
                return BadRequest("The VehicleCategoryId and CategoryName are not valid.");
            }

            DTO.VehicleCategoryDTO result = _vehicleCategoryBusiness.GetVehicleCategoryInfoBy(vehicleCategoryId, categoryName);

            try
            {
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "Vehicle category not found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("Category/All", Name = "AllVehicleCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<DTO.VehicleCategoryDTO>> GetAllVehicleCategories()
        {
            List<DTO.VehicleCategoryDTO> result = _vehicleCategoryBusiness.GetAllVehicleCategoryInfo();

            try
            {
                if (result != null && result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "No vehicle categories found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
