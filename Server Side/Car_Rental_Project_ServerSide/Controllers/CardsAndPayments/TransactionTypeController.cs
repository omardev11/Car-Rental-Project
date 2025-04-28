using CarRentalBusinessLayer;
using CarRentalBusinessLayer.CardsAndPayments;
using CarRentalDataLayer.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Car_Rental_Project_ServerSide.Controllers.CardsAndPayments
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionTypeController : ControllerBase
    {
        private readonly BusinessLayerInterfaces.ITransactionTypeBusiness _transactionTypeBusiness;

        public TransactionTypeController(BusinessLayerInterfaces.ITransactionTypeBusiness transactionTypeBusiness)
        {
            _transactionTypeBusiness = transactionTypeBusiness;
        }

        [HttpPost("TransactionType", Name = "AddTransactionType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddNewTransactionType(DTO.TransactionTypeDTO newTransactionType)
        {
            if (newTransactionType == null)
            {
                return BadRequest("The object of TransactionType cannot be null.");
            }

            _transactionTypeBusiness.Initialize(newTransactionType);
            if (_transactionTypeBusiness.Save())
            {
                return Ok("Transaction type added successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error adding new TransactionType." });
            }
        }

        [HttpPut("TransactionType/Update", Name = "UpdateTransactionType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateTransactionType(DTO.TransactionTypeDTO updatedTransactionType)
        {
            if (updatedTransactionType == null)
            {
                return BadRequest("The object of TransactionType cannot be null.");
            }

            _transactionTypeBusiness.Initialize(updatedTransactionType, TransactionTypeBusiness.enMode.UpdateMode);
            if (_transactionTypeBusiness.Save())
            {
                return Ok("Transaction type updated successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error updating TransactionType." });
            }
        }

        [HttpDelete("TransactionType/Delete/{transactionTypeId}", Name = "DeleteTransactionType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteTransactionType(int transactionTypeId)
        {
            if (transactionTypeId < 1)
            {
                return BadRequest("The TransactionTypeId is not valid.");
            }

            // Assuming your business layer has a dedicated delete method:
            if (_transactionTypeBusiness.DeleteTransactionType(transactionTypeId))
            {
                return Ok("Transaction type deleted successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error deleting TransactionType." });
            }
        }

        [HttpGet("TransactionType/By", Name = "GetTransactionTypeInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DTO.TransactionTypeDTO> GetTransactionTypeInfo(int? transactionTypeId = null, string transactionType = "")
        {
            if (transactionTypeId < 1 && string.IsNullOrEmpty(transactionType))
            {
                return BadRequest("The TransactionTypeId and TransactionType are not valid.");
            }

            DTO.TransactionTypeDTO result = _transactionTypeBusiness.GetTransactionTypeBy(transactionTypeId, transactionType);

            try
            {
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "Transaction type not found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("TransactionType/All", Name = "AllTransactionTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<DTO.TransactionTypeDTO>> GetAllTransactionTypes()
        {
            List<DTO.TransactionTypeDTO> result = _transactionTypeBusiness.GetAllTransactionTypes();

            try
            {
                if (result != null && result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "No transaction types found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
