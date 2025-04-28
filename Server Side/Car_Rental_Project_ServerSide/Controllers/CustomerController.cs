using CarRentalBusinessLayer;
using CarRentalDataLayer.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Car_Rental_Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly BusinessLayerInterfaces.ICustomerBusiness _CustomerBusiness;

        public CustomerController(BusinessLayerInterfaces.ICustomerBusiness CustomerBusiness)
        {
            _CustomerBusiness = CustomerBusiness;
        }

        [HttpPost("Customer", Name = "AddCustomer")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<int> AddNewCustomer(DTO.CustomerDTO.CustomerInfo NewCustomer)
        {
            if (NewCustomer == null)
            {
                return BadRequest("The Object Of The Customer Can Not Be Null");
            }
            if (_CustomerBusiness.CheckEmailExists(NewCustomer.Email))
            {
                return Conflict(new { error = "Email already registered." });
            }
            DTO.CustomerDTO newCustomer = new DTO.CustomerDTO();
            newCustomer.customerInfo = NewCustomer;

            _CustomerBusiness.Initialize(newCustomer);
            if (_CustomerBusiness.Save())
            {
                return Ok(_CustomerBusiness.Customer.CustomerId);

            }
            else
            {
                return StatusCode(500, new { messege = "Error Adding New Customer" });
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------

        [HttpPut("Customer/Update/{CustomerId}", Name = "UpdateCustomer")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DTO.CustomerDTO.CustomerInfo> UpdateCustomer(DTO.CustomerDTO.CustomerInfo UpdatedCustomer, int CustomerId)
        {
            if (UpdatedCustomer == null || CustomerId < 0)
            {
                return BadRequest("The Object Of The Customer Can Not Be Null or Customerid can not be under 0");
            }
            DTO.CustomerDTO newCustomer = new DTO.CustomerDTO();
            newCustomer.customerInfo = UpdatedCustomer;

            _CustomerBusiness.Initialize(newCustomer,CustomerBusiness.enMode.UpdateMode);

            if (_CustomerBusiness.Save())
            {
                return Ok(_CustomerBusiness.Customer.customerInfo);

            }
            else
            {
                return StatusCode(500, new { messege = "Error Updating Customer" });
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------

        [HttpPut("Customer/UpdatePassword/{CustomerId}", Name = "UpdateCustomerPassword")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DTO.CustomerDTO.CustomerInfo> UpdateCustomerPassword(string NewPassword, int CustomerId)
        {
            if (string.IsNullOrWhiteSpace(NewPassword) || CustomerId < 0)
            {
                return BadRequest("The New Password Can Not Be Null or Customerid can not be under 0");
            }

            if (_CustomerBusiness.UpdateCustomerPasswordByid(NewPassword, CustomerId))
            {
                return Ok();

            }
            else
            {
                return StatusCode(500, new { messege = "Error Updating Customer Password" });
            }
        }


        //----------------------------------------------------------------------------------------------------------------------------

        [HttpGet("Login/{Gmail}", Name = "LoginWhithGmailAndPassaword")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<int> LoginWhithGmailAndPassaword(string Gmail, string Password)
        {
            if (string.IsNullOrEmpty(Gmail) || string.IsNullOrEmpty(Password))
            {
                return BadRequest("The Content Can Not Be Empty");
            }

            DTO.CustomerDTO Customer = _CustomerBusiness.GetCustomerByEmailAndPassword(Gmail,Password);

            try
            {
                if (Customer != null)
                {
                    return Ok(Customer.CustomerId);
                }
                else
                {
                    return NotFound(new { message = "Customer not found" });
                }
            }
            catch (Exception ex)
            {
                // Log exception details (for internal debugging)
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }

        }


        //----------------------------------------------------------------------------------------------------------------------------

        [HttpGet("CustomerInfo/{CustomerId}", Name = "GetCustomerInfoById")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DTO.CustomerDTO.CustomerInfo> GetCustomerInfoById(int CustomerId)
        {
            if (CustomerId < 1)
            {
                return BadRequest("The CustomerId Can not be under 1");
            }

            DTO.CustomerDTO Customer = _CustomerBusiness.GetCustomerInfoById(CustomerId);

            try
            {
                if (Customer != null)
                {
                    Customer.customerInfo.PassWord = "";
                    return Ok(Customer.customerInfo);
                }
                else
                {
                    return NotFound(new { message = "Customer not found" });
                }
            }
            catch (Exception ex)
            {
                // Log exception details (for internal debugging)
                return StatusCode(500, new { message = "An unexpected error occurred" + ex.Message });
            }

        }


        //----------------------------------------------------------------------------------------------------------------------------

    }
}
