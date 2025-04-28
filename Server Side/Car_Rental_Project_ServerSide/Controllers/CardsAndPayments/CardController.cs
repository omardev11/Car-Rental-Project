using CarRentalBusinessLayer.CardsAndPayments;
using CarRentalBusinessLayer.Vehicle;
using CarRentalDataLayer.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarRentalBusinessLayer;

namespace Car_Rental_Project_ServerSide.Controllers.CardsAndPayments
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly BusinessLayerInterfaces.ICardBusiness _CardBusiness;

        public CardController(BusinessLayerInterfaces.ICardBusiness cardbusiness)
        {
            _CardBusiness = cardbusiness;
        }

        [HttpPost("Card", Name = "AddCard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddNewCard(DTO.CardDTO newCard)
        {
            if (newCard == null)
            {
                return BadRequest("The object of Card can't be null.");
            }

            _CardBusiness.Initialize(newCard);
            if (_CardBusiness.Save())
            {
                return Ok("Card added successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error adding new Card." });
            }
        }

        [HttpPut("Card/Update", Name = "UpdateCard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateCard(DTO.CardDTO updatedCard)
        {
            if (updatedCard == null)
            {
                return BadRequest("The object of Card can't be null.");
            }

            _CardBusiness.Initialize(updatedCard, CardBusiness.enMode.UpdateMode);
            if (_CardBusiness.Save())
            {
                return Ok("Card updated successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error updating Card." });
            }
        }

        [HttpDelete("Card/Delete/{CardId}", Name = "DeleteCard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteCard(int CardId)
        {
            if (CardId < 1)
            {
                return BadRequest("The CardId is not valid.");
            }


            if (_CardBusiness.DeleteCardInfoById(CardId))
            {
                return Ok("Card deleted successfully.");
            }
            else
            {
                return StatusCode(500, new { message = "Error deleting Card." });
            }
        }

        [HttpGet("Card/ByCardId", Name = "GetCardInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DTO.CardDTO> GetCardInfoById(int CardId)
        {
            if (CardId <= 0)
            {
                return BadRequest("The CardId id not valid.");
            }

            DTO.CardDTO result = _CardBusiness.GetCardInfoById(CardId);

            try
            {
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "Card not found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("Cards/ByCustomerId", Name = "GetAllCardsForAcustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<DTO.CardDTO>> GetAllCardsForAcustomer(int customerId)
        {
            List<DTO.CardDTO> result = _CardBusiness.GetAllCardsForAcustomer(customerId);

            try
            {
                if (result != null && result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { message = "No Cards found." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}

