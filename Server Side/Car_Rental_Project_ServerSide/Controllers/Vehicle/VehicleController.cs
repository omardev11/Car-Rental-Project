using CarRentalBusinessLayer;
using CarRentalBusinessLayer.Vehicle;
using CarRentalDataLayer.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices.ObjectiveC;

namespace Car_Rental_Project_ServerSide.Controllers.Vehicle
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly BusinessLayerInterfaces.IVehicleBusiness _VehicleBusiness;

        public VehicleController(BusinessLayerInterfaces.IVehicleBusiness vehicleBusiness)
        {
            _VehicleBusiness = vehicleBusiness;
        }

        [HttpPost("Vehicle", Name = "AddVehicle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddNewVehicle(DTO.UserViewVehicleDTO newVehicle)
        {
            if (newVehicle == null)
            {
                return BadRequest("The object of Vehicle cannot be null.");
            }

            _VehicleBusiness.Initialize(newVehicle);
            if (_VehicleBusiness.Save())
            {
                return Ok("Vehicle added successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error adding new Vehicle." });
            }
        }

        [HttpPut("Vehicle/Update{VehicleId}", Name = "UpdateVehicle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateVehicle(
             int VehicleId,
             [FromQuery] string? VehicleName,
             [FromQuery] string? Make,
             [FromQuery] string? Model,
             [FromQuery] int? Year,
             [FromQuery] int? StatusId,
             [FromQuery] string? LicensePlate,
             [FromQuery] int? LocationId,
             [FromQuery] int? FuelTypeId,
             [FromQuery] int? VehicleCategoryId
            )
        {
            if (VehicleId < 0)
            {
                return BadRequest("The VehicleId Can no be Under 0.");
            }

            DTO.VehicleIdiesDTO vehicleIdiesDTO = new DTO.VehicleIdiesDTO();
            DTO.VehicleNamesDTO vehicleNamesDTO = new DTO.VehicleNamesDTO();

            DTO.UserViewVehicleDTO updatedVehicle;

            vehicleIdiesDTO.VehicleId = VehicleId;
            vehicleIdiesDTO.VehicleStatusId = StatusId;
            vehicleIdiesDTO.VehicleCategoryId = VehicleCategoryId;
            vehicleIdiesDTO.LocationId = LocationId;
            vehicleIdiesDTO.FuelTypeId = FuelTypeId;

            vehicleNamesDTO.VehicleName = VehicleName;
            vehicleNamesDTO.Year = Year;
            vehicleNamesDTO.Make = Make;
            vehicleNamesDTO.Model = Model;
            vehicleNamesDTO.LicensePlate = LicensePlate;

            updatedVehicle = new DTO.UserViewVehicleDTO(vehicleNamesDTO, vehicleIdiesDTO);

            _VehicleBusiness.Initialize(updatedVehicle, VehicleBusiness.enMode.UpdateMode);
            if (_VehicleBusiness.Save())
            {
                return Ok("Vehicle updated successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error updating Vehicle." });
            }
        }

        [HttpDelete("Vehicle/Delete/{VehicleId}", Name = "DeleteVehicle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteVehicle(int VehicleId)
        {
            if (VehicleId < 1)
            {
                return BadRequest("The VehicleId is not valid.");
            }

            bool IsDeleted = _VehicleBusiness.DeleteVehicle(VehicleId);

            if (IsDeleted)
            {
                return Ok("Vehicle deleted successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error deleting Vehicle." });
            }
        }


        [HttpGet("VehicleForUser/By", Name = "GetVehicleInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<DTO.UserViewVehicleDTO>> GetVehicleInfo(
             [FromQuery] int? VehicleId,
             [FromQuery] string? VehicleName,
             [FromQuery] string? Make,
             [FromQuery] string? Model,
             [FromQuery] int? Year,
             [FromQuery] int? StatusId,
             [FromQuery] string? LicensePlate,
             [FromQuery] int? LocationId,
             [FromQuery] int? FuelTypeId,
             [FromQuery] int? VehicleCategoryId,
             [FromQuery] decimal? StartPrice,
             [FromQuery] decimal? EndPrice
            )
        {
            if (EndPrice.HasValue && decimal.IsNegative(EndPrice.Value))
            {
                return BadRequest("The Prices Can not be Under 0.");
            }
            else
            {
                if (StartPrice.HasValue)
                {
                    if (decimal.IsNegative(StartPrice.Value))
                        return BadRequest("The Prices Can not be Under 0.");
                }
                else
                    StartPrice = 0;
            }

            DTO.VehicleIdiesDTO vehicleIdiesDTO = new DTO.VehicleIdiesDTO();
            DTO.VehicleNamesDTO vehicleNamesDTO = new DTO.VehicleNamesDTO();

            DTO.UserViewVehicleDTO UvehicleInfo;

            vehicleIdiesDTO.VehicleId = VehicleId;
            if (VehicleId != null) 
            {
                UvehicleInfo = new DTO.UserViewVehicleDTO(vehicleNamesDTO,vehicleIdiesDTO);
            }
            else
            {
                vehicleIdiesDTO.VehicleStatusId = StatusId;
                vehicleIdiesDTO.VehicleCategoryId = VehicleCategoryId;
                vehicleIdiesDTO.LocationId = LocationId;
                vehicleIdiesDTO.FuelTypeId = FuelTypeId;

                vehicleNamesDTO.VehicleName = VehicleName;
                vehicleNamesDTO.Year = Year;
                vehicleNamesDTO.Make = Make;
                vehicleNamesDTO.Model = Model;
                vehicleNamesDTO.LicensePlate = LicensePlate;

                UvehicleInfo = new DTO.UserViewVehicleDTO(vehicleNamesDTO, vehicleIdiesDTO);
            }



            List<DTO.UserViewVehicleDTO> result = _VehicleBusiness.GetVehicleBy(UvehicleInfo,StartPrice,EndPrice);

            try
            {
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "Vehicle not found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }


        [HttpGet("VehicleForCustomer/By", Name = "GetVehicleInfoForCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<DTO.CustomerViewVehicleDTO>> GetVehicleByForCustomerView(
             [FromQuery] int? VehicleId,
             [FromQuery] string? VehicleName,
             [FromQuery] string? Make,
             [FromQuery] string? Model,
             [FromQuery] int? Year,
             [FromQuery] int? StatusId,
             [FromQuery] string? LicensePlate,
             [FromQuery] int? LocationId,
             [FromQuery] int? FuelTypeId,
             [FromQuery] int? VehicleCategoryId,
              [FromQuery] decimal? StartPrice,
             [FromQuery] decimal? EndPrice
            )
        {
            if (EndPrice.HasValue && decimal.IsNegative(EndPrice.Value))
            {
                return BadRequest("The Prices Can not be Under 0.");
            }
            else
            {
                if (StartPrice.HasValue)
                {
                    if (decimal.IsNegative(StartPrice.Value))
                        return BadRequest("The Prices Can not be Under 0.");
                }
                else
                    StartPrice = 0;
            }
            DTO.VehicleIdiesDTO vehicleIdiesDTO = new DTO.VehicleIdiesDTO();
            DTO.VehicleNamesDTO vehicleNamesDTO = new DTO.VehicleNamesDTO();

            DTO.UserViewVehicleDTO UvehicleInfo;

            vehicleIdiesDTO.VehicleId = VehicleId;
            if (VehicleId != null)
            {
                UvehicleInfo = new DTO.UserViewVehicleDTO(vehicleNamesDTO, vehicleIdiesDTO);
            }
            else
            {
                vehicleIdiesDTO.VehicleStatusId = StatusId;
                vehicleIdiesDTO.VehicleCategoryId = VehicleCategoryId;
                vehicleIdiesDTO.LocationId = LocationId;
                vehicleIdiesDTO.FuelTypeId = FuelTypeId;

                vehicleNamesDTO.VehicleName = VehicleName;
                vehicleNamesDTO.Year = Year;
                vehicleNamesDTO.Make = Make;
                vehicleNamesDTO.Model = Model;
                vehicleNamesDTO.LicensePlate = LicensePlate;

                UvehicleInfo = new DTO.UserViewVehicleDTO(vehicleNamesDTO, vehicleIdiesDTO);
            }

            List<DTO.CustomerViewVehicleDTO> result = _VehicleBusiness.GetVehicleByForCustomerView(UvehicleInfo, StartPrice, EndPrice);

            try
            {
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "Vehicle not found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }


        //[HttpGet("VehiclePrice/ForCustomer/By", Name = "GetVehicleByPriceBetweenForCustomerView")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public ActionResult<List<DTO.CustomerViewVehicleDTO>> GetVehicleByPriceBetweenForCustomerView(decimal StartPrice, decimal EndPrice)
        //{
        //    if (decimal.IsNegative(StartPrice) || decimal.IsNegative(EndPrice))
        //    {
        //        return BadRequest("The Prices Can not be Under 0.");
        //    }

        //    List<DTO.CustomerViewVehicleDTO> result = _VehicleBusiness.GetVehicleByPriceBetweenForCustomerView(StartPrice,EndPrice);

        //    try
        //    {
        //        if (result != null)
        //        {
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            return NotFound(new { message = "Vehicle not found." });
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, new { message = "An unexpected error occurred." });
        //    }
        //}
    }
}
