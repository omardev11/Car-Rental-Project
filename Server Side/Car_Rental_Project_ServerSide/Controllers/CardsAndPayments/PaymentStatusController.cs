using CarRentalBusinessLayer.CardsAndPayments;
using CarRentalDataLayer.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CarRentalBusinessLayer.BusinessLayerInterfaces;

namespace Car_Rental_Project_ServerSide.Controllers.CardsAndPayments
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentStatusController : ControllerBase
    {
        private readonly IPaymentStatusBusiness _paymentStatusBusiness;

        public PaymentStatusController(IPaymentStatusBusiness paymentStatusBusiness)
        {
            _paymentStatusBusiness = paymentStatusBusiness;
        }

        [HttpPost("PaymentStatus", Name = "AddPaymentStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddNewPaymentStatus(DTO.PaymentStatusDTO newPaymentStatus)
        {
            if (newPaymentStatus == null)
            {
                return BadRequest("The object of PaymentStatus cannot be null.");
            }

            _paymentStatusBusiness.Initialize(newPaymentStatus);
            if (_paymentStatusBusiness.Save())
            {
                return Ok("Payment status added successfully.");
            }
            return StatusCode(500, new { message = "Error adding new PaymentStatus." });
        }

        [HttpPut("PaymentStatus/Update", Name = "UpdatePaymentStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdatePaymentStatus(DTO.PaymentStatusDTO updatedPaymentStatus)
        {
            if (updatedPaymentStatus == null)
            {
                return BadRequest("The object of PaymentStatus cannot be null.");
            }

            _paymentStatusBusiness.Initialize(updatedPaymentStatus, PaymentStatusBusiness.enMode.UpdateMode);
            if (_paymentStatusBusiness.Save())
            {
                return Ok("Payment status updated successfully.");
            }
            return StatusCode(500, new { message = "Error updating PaymentStatus." });
        }

        [HttpDelete("PaymentStatus/Delete/{paymentStatusId}", Name = "DeletePaymentStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeletePaymentStatus(int paymentStatusId)
        {
            if (paymentStatusId < 1)
            {
                return BadRequest("The PaymentStatusId is not valid.");
            }


            if (_paymentStatusBusiness.DeletePaymentStatus(paymentStatusId))
            {
                return Ok("Payment status deleted successfully.");
            }
            return StatusCode(500, new { message = "Error deleting PaymentStatus." });
        }

        [HttpGet("PaymentStatus/By", Name = "GetPaymentStatusInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DTO.PaymentStatusDTO> GetPaymentStatusInfo(int? paymentStatusId = null, string statusName = "")
        {
            if (paymentStatusId < 1 && string.IsNullOrEmpty(statusName))
            {
                return BadRequest("Invalid PaymentStatusId or StatusName.");
            }

            DTO.PaymentStatusDTO result = _paymentStatusBusiness.GetPaymentStatusInfoBy(paymentStatusId, statusName);

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound(new { message = "Payment status not found." });
        }

        [HttpGet("PaymentStatus/All", Name = "AllPaymentStatuses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<DTO.PaymentStatusDTO>> GetAllPaymentStatuses()
        {
            var result = _paymentStatusBusiness.GetAllPaymentStatuses();

            if (result != null && result.Count > 0)
            {
                return Ok(result);
            }
            return NotFound(new { message = "No payment statuses found." });
        }
    }
}
