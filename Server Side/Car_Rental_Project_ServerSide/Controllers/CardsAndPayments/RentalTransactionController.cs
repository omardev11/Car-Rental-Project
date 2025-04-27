using CarRentalBusinessLayer.CardsAndPayments;
using CarRentalDataLayer.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;
using static CarRentalBusinessLayer.BusinessLayerInterfaces;

namespace Car_Rental_Project_ServerSide.Controllers.CardsAndPayments
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalTransactionController : ControllerBase
    {
        private readonly IRentalTransactionBusiness _rentalTransactionBusiness;

        public RentalTransactionController(IRentalTransactionBusiness rentalTransactionBusiness)
        {
            _rentalTransactionBusiness = rentalTransactionBusiness;
        }

        [HttpPost("RentalTransaction", Name = "AddRentalTransaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddNewRentalTransaction(DTO.RentalTransactionDTO newRentalTransaction)
        {
            if (newRentalTransaction == null)
            {
                return BadRequest("The object of RentalTransaction cannot be null.");
            }

            _rentalTransactionBusiness.Initialize(newRentalTransaction);
            if (_rentalTransactionBusiness.Save())
            {
                return Ok("Rental transaction added successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error adding new RentalTransaction." });
            }
        }



        [HttpPut("RentalTransaction/Update/{TransactionId}", Name = "UpdateRentalTransaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateRentalTransaction(
              int TransactionId,
             [FromQuery] int? TransactionTypeId,
             [FromQuery] int? BookingId,
             [FromQuery] int? PaymentMehtodId,
             [FromQuery] int? PaymentStatusId,
             [FromQuery] decimal? Amount
            )
        {
            DTO.RentalTransactionDTO updatedRentalTransaction = new DTO.RentalTransactionDTO()
            {
                TransactionId = TransactionId,
                TransactionTypeId = TransactionTypeId,
                BookingId = BookingId,
                PaymentMethodId = PaymentMehtodId,
                PaymentStatusId = PaymentStatusId,
                Amount = Amount

            };
           
            _rentalTransactionBusiness.Initialize(updatedRentalTransaction, RentalTransactionBusiness.enMode.UpdateMode);
            if (_rentalTransactionBusiness.Save())
            {
                return Ok("Rental transaction updated successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error updating RentalTransaction." });
            }
        }




        [HttpGet("RentalTransaction/AllBy", Name = "GetAllRentalTransactionsBy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<DTO.RentalTransactionDTO>> GetAllRentalTransactionsBy(
             [FromQuery] int? TransactionId,
             [FromQuery] int? TransactionTypeId,
             [FromQuery] int? BookingId,
             [FromQuery] int? PaymentMehtodId,
             [FromQuery] int? PaymentStatusId,
             [FromQuery] decimal? Amount,
             [FromQuery] DateTime? TransactionDate
            )
        {
            DTO.RentalTransactionDTO FiltiringRentalTransaction = new DTO.RentalTransactionDTO()
            {
                TransactionId = TransactionId,
                TransactionTypeId = TransactionTypeId,
                BookingId = BookingId,
                PaymentMethodId = PaymentMehtodId,
                PaymentStatusId = PaymentStatusId,
                Amount = Amount,
                TransactionDate = TransactionDate

            };

            List<DTO.RentalTransactionDTO> result = _rentalTransactionBusiness.GetAllRentalTransactionsBy(FiltiringRentalTransaction);

            try
            {
                if (result != null && result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "No rental transactions found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
