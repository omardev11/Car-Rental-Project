using CarRentalBusinessLayer;
using CarRentalBusinessLayer.CardsAndPayments;
using CarRentalDataLayer.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Car_Rental_Project_ServerSide.Controllers.CardsAndPayments
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly BusinessLayerInterfaces.IPaymentMethodBusiness _paymentMethodBusiness;

        public PaymentMethodController(BusinessLayerInterfaces.IPaymentMethodBusiness paymentMethodBusiness)
        {
            _paymentMethodBusiness = paymentMethodBusiness;
        }

        [HttpPost("PaymentMethod", Name = "AddPaymentMethod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddNewPaymentMethod(DTO.PaymentMethodDTO newPaymentMethod)
        {
            if (newPaymentMethod == null)
            {
                return BadRequest("The object of PaymentMethod cannot be null.");
            }

            _paymentMethodBusiness.Initialize(newPaymentMethod);
            if (_paymentMethodBusiness.Save())
            {
                return Ok("Payment method added successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error adding new PaymentMethod." });
            }
        }

        [HttpPut("PaymentMethod/Update", Name = "UpdatePaymentMethod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdatePaymentMethod(DTO.PaymentMethodDTO updatedPaymentMethod)
        {
            if (updatedPaymentMethod == null)
            {
                return BadRequest("The object of PaymentMethod cannot be null.");
            }

            _paymentMethodBusiness.Initialize(updatedPaymentMethod, PaymentMethodBusiness.enMode.UpdateMode);
            if (_paymentMethodBusiness.Save())
            {
                return Ok("Payment method updated successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error updating PaymentMethod." });
            }
        }

        [HttpDelete("PaymentMethod/Delete/{paymentMethodId}", Name = "DeletePaymentMethod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeletePaymentMethod(int paymentMethodId)
        {
            if (paymentMethodId < 1)
            {
                return BadRequest("The PaymentMethodId is not valid.");
            }

            if (_paymentMethodBusiness.DeletePaymentMethod(paymentMethodId))
            {
                return Ok("Payment method deleted successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error deleting PaymentMethod." });
            }
        }

        [HttpGet("PaymentMethod/By", Name = "GetPaymentMethodInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DTO.PaymentMethodDTO> GetPaymentMethodInfo(int? paymentMethodId = null, string paymentMethod = "")
        {
            if (paymentMethodId < 1 && string.IsNullOrEmpty(paymentMethod))
            {
                return BadRequest("The PaymentMethodId and PaymentMethod are not valid.");
            }

            DTO.PaymentMethodDTO result = _paymentMethodBusiness.GetPaymentMethodById(paymentMethodId, paymentMethod);

            try
            {
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "Payment method not found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("PaymentMethod/All", Name = "AllPaymentMethods")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<DTO.PaymentMethodDTO>> GetAllPaymentMethods()
        {
            List<DTO.PaymentMethodDTO> result = _paymentMethodBusiness.GetAllPaymentMethods();

            try
            {
                if (result != null && result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "No payment methods found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
