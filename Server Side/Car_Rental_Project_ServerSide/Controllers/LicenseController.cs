using CarRentalBusinessLayer;
using CarRentalDataLayer.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Car_Rental_Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenseController : ControllerBase
    {
        private readonly BusinessLayerInterfaces.ILicenseBusiness _LicenseBusiness;

        public LicenseController(BusinessLayerInterfaces.ILicenseBusiness LicenseBusiness)
        {
            _LicenseBusiness = LicenseBusiness;
        }



        [HttpPost("License", Name = "AddLicense")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddNewLicenseForCustomer(DTO.LicenseDTO NewLicense)
        {
            if (NewLicense == null)
            {
                return BadRequest("The Object Of The License Can Not Be Null");
            }

            _LicenseBusiness.Initialize(NewLicense);
            if (_LicenseBusiness.Save())
            {
                return Ok();

            }
            else
            {
                return StatusCode(500, new { messege = "Error Adding New License" });
            }
        }


        //----------------------------------------------------------------------------------------------------------------------------


        [HttpPut("License/Update", Name = "UpdateLicense")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateLicenseInfo(DTO.LicenseDTO UpdatedLicenseInfo)
        {
            if (UpdatedLicenseInfo == null)
            {
                return BadRequest("The Object Of The License Can Not Be Null");
            }
            _LicenseBusiness.Initialize(UpdatedLicenseInfo, LicenseBusiness.enMode.UpdateMode);

            if (_LicenseBusiness.Save())
            {
                return Ok();

            }
            else
            {
                return StatusCode(500, new { messege = "Error Updating License" });
            }
        }


        //----------------------------------------------------------------------------------------------------------------------------


        [HttpDelete("License/Delete{LicenseId}", Name = "DeleteLicense")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteLicense(int LicenseId)
        {
            if (LicenseId < 1)
            {
                return BadRequest("The LicenseId is not valid");
            }
            _LicenseBusiness.Initialize(new DTO.LicenseDTO(), LicenseBusiness.enMode.DeleteMode);
            _LicenseBusiness.LicenseInfo.LicenseId = LicenseId;
            if (_LicenseBusiness.Save())
            {
                return Ok();

            }
            else
            {
                return StatusCode(500, new { messege = "Error Deleting License" });
            }
        }


        //----------------------------------------------------------------------------------------------------------------------------


        [HttpGet("Customer/{CustomerId}/Licenses", Name = "GetCustomerLicenses")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<DTO.LicenseDTO>> GetAllLicenseForCustomer(int CustomerId)
        {
            if (CustomerId < 0)
            {
                return BadRequest("The CustomerId is not valid");
            }

            List<DTO.LicenseDTO> AllLicenses = _LicenseBusiness.GetLicenseByCustomerId(CustomerId);

            try
            {
                if (AllLicenses.Count > 0)
                {
                    return Ok(AllLicenses);
                }
                else
                {
                    return NotFound(new { message = "not found Any License" });
                }
            }
            catch (Exception)
            {
                // Log exception details (for internal debugging)
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }

        }
    
}
}
