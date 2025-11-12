using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantBusiness;
using RestaurantData;
using RestaurantDTOs;

namespace RestaurantApi.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost("AddNewUser", Name = "AddNewUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsUserDTO> AddNewUser(clsUserDTO UserDTO)
        {
            try
            {
                clsUser User = new clsUser();

                User.UserName = UserDTO.UserName;
                User.DeviceToken = UserDTO.DeviceToken;
                User.Email = UserDTO.Email;
                User.PasswordHash = UserDTO.PasswordHash;
                User.Phone = UserDTO.Phone;

                if (User.Save())
                {
                    UserDTO.UserID = User.UserID;
                    return CreatedAtRoute("GetUserByID", new { UserID = UserDTO.UserID }, UserDTO);
                }
                else
                {
                    return BadRequest("Failed Creating An Account");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpPut("UpdateUser", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsUserDTO> UpdateUser(clsUserDTO UserDTO)
        {
            try
            {
                clsUser User = clsUser.Find(UserDTO.UserID);

                if (User == null)
                {
                    return NotFound("Could Not Find The User");
                }

                User.UserName = UserDTO.UserName;
                User.DeviceToken = UserDTO.DeviceToken;
                User.Email = UserDTO.Email;
                User.PasswordHash = UserDTO.PasswordHash;
                User.Phone = UserDTO.Phone;

                if (User.Save())
                {
                    return Ok(User.UserDTO);
                }
                else
                {
                    return BadRequest($"The User Info Could Not Be Updated");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetUserByID/{UserID}", Name = "GetUserByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsUserDTO> GetUserByID(int UserID)
        {
            try
            {
                clsUser User = clsUser.Find(UserID);

                if (User == null)
                {
                    return NotFound("Could Not Find The User");
                }
                else
                {
                    return Ok(User.UserDTO);
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetUserByUserName/{UserName}", Name = "GetUserByUserName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsUserDTO> GetUserByUserName(string UserName)
        {
            try
            {
                clsUser User = clsUser.Find(UserName);

                if (User == null)
                {
                    return NotFound("Could Not Find The User");
                }
                else
                {
                    return Ok(User.UserDTO);
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetUserByUserNameAndPassword/{UserName}/{PasswordHash}", Name = "GetUserByUserNameAndPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsUserDTO> GetUserByUserNameAndPassword(string UserName, string PasswordHash)
        {
            try
            {
                clsUser User = clsUser.Find(UserName, PasswordHash);

                if (User == null)
                {
                    return NotFound("Could Not Find The User");
                }
                else
                {
                    return Ok(User.UserDTO);
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetUserCoinsTransactionsHistory/{UserID}", Name = "GetUserCoinsTransactionsHistory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsCoinTransactionDTO>> GetUserCoinsTransactionsHistory(int UserID)
        {
            try
            {
                List<clsCoinTransactionDTO> UserCoinsTransactionsHistoryList = clsUser.GetUserCoinsTransactionsHistory(UserID);

                if (UserCoinsTransactionsHistoryList.Count == 0)
                {
                    return NotFound("There Is No Transactions To Show");
                }
                return Ok(UserCoinsTransactionsHistoryList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetAllUserOrders/{UserID}", Name = "GetAllUserOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsOrderDTO>> GetAllUserOrders(int UserID)
        {
            try
            {
                List<clsOrderDTO> UserOrdersList = clsUser.GetAllUserOrders(UserID);

                if (UserOrdersList.Count == 0)
                {
                    return NotFound("There Is No Orders To Show");
                }
                return Ok(UserOrdersList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetDriverDeliveryOrders/{DriverID}", Name = "GetDriverDeliveryOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsOrderDTO>> GetDriverDeliveryOrders(int DriverID)
        {
            try
            {
                clsUser User = clsUser.Find(DriverID);

                if (User == null)
                {
                    return NotFound("Could Not Find The User");
                }

                if (User.GetUserRole() != (int)clsUserRole.enUserRole.Driver)
                {
                    return BadRequest($"Only Drivers Can Access This Page");
                }

                List<clsOrderDTO> DriverOrdersList = clsUser.GetDriverDeliveryOrders(DriverID);

                if (DriverOrdersList.Count == 0)
                {
                    return NotFound("There Is No Orders To Show");
                }
                return Ok(DriverOrdersList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
    }
}
