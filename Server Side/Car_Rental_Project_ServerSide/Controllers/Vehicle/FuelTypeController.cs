using CarRentalBusinessLayer;
using CarRentalBusinessLayer.Vehicle;
using CarRentalDataLayer.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Car_Rental_Project_ServerSide.Controllers.Vehicle
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelTypeController : ControllerBase
    {
        private readonly BusinessLayerInterfaces.IFuelTypeBusiness _fuelTypeBusiness;

        public FuelTypeController(BusinessLayerInterfaces.IFuelTypeBusiness fuelTypeBusiness)
        {
            _fuelTypeBusiness = fuelTypeBusiness;
        }

        [HttpPost("FuelType", Name = "AddFuelType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddNewFuelType(DTO.FuelTypeDTO newFuelType)
        {
            if (newFuelType == null)
            {
                return BadRequest("The object of FuelType cannot be null.");
            }

            _fuelTypeBusiness.Initialize(newFuelType);
            if (_fuelTypeBusiness.Save())
            {
                return Ok("Fuel type added successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error adding new FuelType." });
            }
        }

        [HttpPut("FuelType/Update", Name = "UpdateFuelType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateFuelType(DTO.FuelTypeDTO updatedFuelType)
        {
            if (updatedFuelType == null)
            {
                return BadRequest("The object of FuelType cannot be null.");
            }

            _fuelTypeBusiness.Initialize(updatedFuelType, FuelTypeBusiness.enMode.UpdateMode);
            if (_fuelTypeBusiness.Save())
            {
                return Ok("Fuel type updated successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error updating FuelType." });
            }
        }

        [HttpDelete("FuelType/Delete/{fuelTypeId}", Name = "DeleteFuelType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteFuelType(int fuelTypeId)
        {
            if (fuelTypeId < 1)
            {
                return BadRequest("The FuelTypeId is not valid.");
            }

            _fuelTypeBusiness.Initialize(new DTO.FuelTypeDTO(), FuelTypeBusiness.enMode.DeleteMode);
            _fuelTypeBusiness.FuelTypeInfo.FuelTypeId = fuelTypeId;
            if (_fuelTypeBusiness.Save())
            {
                return Ok("Fuel type deleted successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error deleting FuelType." });
            }
        }

        [HttpGet("FuelType/By", Name = "GetFuelTypeInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DTO.FuelTypeDTO> GetFuelTypeInfo(int? fuelTypeId = null, string fuelTypeName = "")
        {
            if (fuelTypeId < 1 && string.IsNullOrEmpty(fuelTypeName))
            {
                return BadRequest("The FuelTypeId and FuelTypeName are not valid.");
            }

            DTO.FuelTypeDTO result = _fuelTypeBusiness.GetFuelTypeInfoBy(fuelTypeId, fuelTypeName);

            try
            {
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "Fuel type not found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("FuelTypes/All", Name = "AllFuelTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<DTO.FuelTypeDTO>> GetAllFuelTypes()
        {
            List<DTO.FuelTypeDTO> result = _fuelTypeBusiness.GetAllFuelTypeInfo();

            try
            {
                if (result != null && result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "No fuel types found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
