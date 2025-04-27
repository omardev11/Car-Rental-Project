using CarRentalBusinessLayer;
using CarRentalDataLayer.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CarRentalBusinessLayer.BusinessLayerInterfaces;

namespace Car_Rental_Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _UserBusiness;
        
        public UserController(IUserBusiness UserBusiness)
        {
            _UserBusiness = UserBusiness;
        }

        [HttpPost("{userId}", Name = "AddUser")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<int> AddNewUser(DTO.UserDTO NewUser,int userId)
        {
            if (NewUser == null || userId <= 0)
            {
                return BadRequest("The Object Of The User Can Not Be Null And The Manager UserId Can not be Invalid");
            }
            if (!_UserBusiness.IsThisUserCanManageUsers(userId))
            {
                return StatusCode(403, "You are not authorized to perform this action.");
            }

            _UserBusiness.Initialize(NewUser);
            if (_UserBusiness.Save())
            {
                return Ok(_UserBusiness._UserInfo.UserId);

            }
            else
            {
                return StatusCode(500, new { messege = "Error Adding New User" });
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------

        [HttpPut("Update/User/{updatingUserId}/{ManaginUserId}", Name = "UpdateUser")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DTO.UserDTO> UpdateUser(
             int ManaginUserId,
             int updatingUserId,
             [FromQuery] string? UserName,
             [FromQuery] bool? IsActive,
             [FromQuery] bool? CanManageUser,
             [FromQuery] string? FirstName,
             [FromQuery] string? LastName,
             [FromQuery] string? Address,
             [FromQuery] DateTime? Birthdate
            )
        {
            if (updatingUserId <= 0 || ManaginUserId <= 0)
            {
                return BadRequest("The ManagingUserid And UpdatingUserid can not be under 0 or equal 0");
            }
            if (!_UserBusiness.IsThisUserCanManageUsers(ManaginUserId))
            {
                return StatusCode(403, "You are not authorized to perform this action.");
            }
            DTO.UserDTO UpdatedUser = new DTO.UserDTO();
            UpdatedUser.UserId = updatingUserId;
            UpdatedUser.UserName = UserName;
            UpdatedUser.IsActive = IsActive;
            UpdatedUser.Birthdate = Birthdate;
            UpdatedUser.CanManageUsers = CanManageUser;
            UpdatedUser.FirstName = FirstName;
            UpdatedUser.LastName = LastName;
            UpdatedUser.Adress = Address;
        

            _UserBusiness.Initialize(UpdatedUser, UserBusiness.enMode.UpdateMode);

            if (_UserBusiness.Save())
            {
                return Ok("User Updated Succesfully");

            }
            else
            {
                return StatusCode(500, new { messege = "Error Updating User" });
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------


        [HttpGet("Login/User", Name = "LoginWhithUserNameAndPassaword")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<int> LoginWhithUserNameAndPassaword(string UserName, string Password)
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                return BadRequest("The Content Can Not Be Empty");
            }

            DTO.UserDTO User = _UserBusiness.GetUserInfoByUserNameAndPassword(UserName, Password);

            try
            {
                if (User != null)
                {
                    return Ok(User.UserId);
                }
                else
                {
                    return NotFound(new { message = "User not found" });
                }
            }
            catch (Exception)
            {
                // Log exception details (for internal debugging)
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }

        }


        //----------------------------------------------------------------------------------------------------------------------------

        [HttpGet("UserInfo/{ManagingUserId}", Name = "GetUserInfoById")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<DTO.UserDTO>> GetAllUsersBy(
             int ManagingUserId,
             [FromQuery]  int? updatingUserId,
             [FromQuery] string? UserName,
             [FromQuery] bool? IsActive,
             [FromQuery] bool? CanManageUser,
             [FromQuery] string? FirstName,
             [FromQuery] string? LastName,
             [FromQuery] string? Address,
             [FromQuery] DateTime? Birthdate,
             [FromQuery] int? Age
            )
        {
            if (ManagingUserId <= 0)
            {
                return BadRequest("The UserId Can not be under 1");
            }
            if (!_UserBusiness.IsThisUserCanManageUsers(ManagingUserId))
            {
                return StatusCode(403, "You are not authorized to perform this action.");
            }
            DTO.UserDTO UserInfo = new DTO.UserDTO();
            UserInfo.UserId = updatingUserId;
            UserInfo.UserName = UserName;
            UserInfo.IsActive = IsActive;
            UserInfo.Birthdate = Birthdate;
            UserInfo.CanManageUsers = CanManageUser;
            UserInfo.FirstName = FirstName;
            UserInfo.LastName = LastName;
            UserInfo.Adress = Address;
            UserInfo.Age = Age;

            var User = _UserBusiness.GetAllUsersBy(UserInfo);

            try
            {
                if (User != null && User.Count > 0)
                {
                    return Ok(User);
                }
                else
                {
                    return NotFound(new { message = "User not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred" + ex.Message });
            }

        }


        //----------------------------------------------------------------------------------------------------------------------------

    }
}
