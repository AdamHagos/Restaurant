using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantBusiness;
using RestaurantData;
using RestaurantDTOs;
using static RestaurantBusiness.clsUserRole;

namespace RestaurantApi.Controllers
{
    [Route("api/Orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [HttpPost("AddNewAddOrder", Name = "AddNewOrder")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsOrderDTO> AddNewOrder(clsProcessOrderDTO ProcessOrderDTO)
        {
            try
            {
                int? UserRole = clsUser.GetUserRole(ProcessOrderDTO.OrderDTO.UserID);

                if (UserRole != null)
                {
                    return BadRequest($"Employees Cant Order");
                }

                List<clsCheckoutCartItemsDTO> UpdatedCheckoutCartItemsDTO = clsOrder.SyncCartItemsWithDatabase(ProcessOrderDTO.CheckoutCartItemsDTO);
                if (!clsOrder.IsCartUpToDate(ProcessOrderDTO.CheckoutCartItemsDTO, UpdatedCheckoutCartItemsDTO))
                {
                    return Ok(UpdatedCheckoutCartItemsDTO);
                }
                
                decimal OrderTotalAmount = clsOrder.CalculateCartTotalAmount(ProcessOrderDTO.CheckoutCartItemsDTO);
                
                if ((ProcessOrderDTO.PaymentInfoDTO.PaymentMethod.Count == 1) && (ProcessOrderDTO.PaymentInfoDTO.PaymentMethod[0] == (byte)clsOrder.enPaymentMethod.Coins))
                {
                    int CoinsValue = clsUser.GetCoinsValue((int)ProcessOrderDTO.PaymentInfoDTO.Amount[0]);
                    if (OrderTotalAmount != CoinsValue)
                    {
                        return BadRequest("The Coins Arent Enough. Add Cash Or Debit Option With Coin");
                    }
                }

                clsOrder Order = new clsOrder();

                Order.ServiceType = ProcessOrderDTO.OrderDTO.ServiceType;
                Order.LocationID = ProcessOrderDTO.OrderDTO.LocationID;
                Order.UserID = ProcessOrderDTO.OrderDTO.UserID;
                Order.TotalAmount = OrderTotalAmount;

                if (Order.Save(ProcessOrderDTO.PaymentInfoDTO, ProcessOrderDTO.CheckoutCartItemsDTO))
                {
                    ProcessOrderDTO.OrderDTO.OrderID = Order.OrderID;
                    ProcessOrderDTO.OrderDTO.TotalAmount = Order.TotalAmount;
                    return CreatedAtRoute("GetOrderByID", new { OrderID = ProcessOrderDTO.OrderDTO.OrderID }, ProcessOrderDTO.OrderDTO);
                }
                else
                {
                    return BadRequest("The Order Could Not Be Placed");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpPut("UpdateOrderDriver/{OrderID}/{NewDriverID}", Name = "UpdateOrderDriver")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsOrderDTO> UpdateOrderDriver(int OrderID, int NewDriverID)
        {
            try
            {
                clsOrder Order = clsOrder.Find(OrderID);
                if (Order.UpdateOrderDriver(NewDriverID))
                {
                    return Ok(Order.OrderDTO);
                }
                else
                {
                    return BadRequest($"The Order Could Not Be Updated");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpPut("UpdateOrderStatuses/{OrderID}", Name = "UpdateOrderStatuses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsOrderDTO> UpdateOrderStatuses(int OrderID, int UserID)
        {
            try
            {
                clsOrder Order = clsOrder.Find(OrderID);

                int? UserRole = clsUser.GetUserRole(UserID);

                if (UserRole == null)
                {
                    return BadRequest($"You Have No Authorization To Update The Status Of The Order");
                }

                byte? OrderCurrentStatus = Order.GetOrderCurrentStatus();

                if (UserRole == (int)clsUserRole.enUserRole.Driver)
                {
                    if ((Order.ServiceType == (byte)clsOrder.enServiceType.Delivery) && !clsOrder.IsOrderAssignedToDriver(OrderID, UserID))
                    {
                        return BadRequest($"You are not assigned to the order in order to update it");
                    }
                    if (Order.ServiceType != (byte)clsOrder.enServiceType.Delivery)
                    {
                        return BadRequest($"You cant update the order since it isnt delivery");
                    }
                    if (OrderCurrentStatus < (byte)clsOrder.enOrderStatus.ReadyToDeliver)
                    {
                        return BadRequest($"it is the staff duty to update it");
                    }
                }

                if (UserRole != (int)clsUserRole.enUserRole.Driver)
                {
                    if (Order.ServiceType == (byte)clsOrder.enServiceType.Delivery && OrderCurrentStatus >= (byte)clsOrder.enOrderStatus.ReadyToDeliver)
                    {
                        return BadRequest($"it is the driver duty to update it");
                    }
                }

                if (Order.UpdateOrderStatus())
                {
                    return Ok(Order.OrderDTO);
                }
                else
                {
                    return BadRequest($"The Status Of the Order Could Not Be Updated");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetOrderByID/{OrderID}", Name = "GetOrderByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsOrderDTO> GetOrderByID(int OrderID)
        {
            try
            {
                clsOrder Order = clsOrder.Find(OrderID);

                if (Order == null)
                {
                    return NotFound("Could Not Find The Order");
                }
                else
                {
                    return Ok(Order.OrderDTO);
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }

        }
        [HttpGet("GetOrderStatuses/{OrderID}", Name = "GetOrderStatuses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsOrderStatusDTO>> GetOrderStatuses(int OrderID)
        {
            try
            {
                List<clsOrderStatusDTO> OrderStatusesList = clsOrder.GetOrderStatuses(OrderID);

                if (OrderStatusesList.Count == 0)
                {
                    return NotFound("There Is No Orders To Show");
                }
                return Ok(OrderStatusesList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetAllOrders", Name = "GetAllOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsOrderDTO>> GetAllOrders()
        {
            try
            {
                List<clsOrderDTO> OrdersList = clsOrder.GetAllOrders();

                if (OrdersList.Count == 0)
                {
                    return NotFound("There Is No Orders To Show");
                }
                return Ok(OrdersList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetAllOrdersByOrderStatus/{OrderStatus}", Name = "GetAllOrdersByOrderStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsOrderStatus>> GetAllOrdersByOrderStatus(int OrderStatus)
        {
            try
            {
                List<clsOrderDTO> OrdersList = clsOrder.GetAllOrders(OrderStatus);

                if (OrdersList.Count == 0)
                {
                    return NotFound("There Is No Orders To Show");
                }
                return Ok(OrdersList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetCartItemsByOrderID/{OrderID}", Name = "GetCartItemsByOrderID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsCartDTO>> GetCartItemsByOrderID(int OrderID)
        {
            try
            {
                List<clsCartDTO> CartsList = clsOrder.GetCartItemsByOrderID(OrderID);

                if (CartsList.Count == 0)
                {
                    return NotFound("The Order Does Not Have Any Items");
                }
                return Ok(CartsList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpPut("MarkOrderAsCompleted/{OrderID}", Name = "MarkOrderAsCompleted")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsAddOnDTO>> MarkOrderAsCompleted(int OrderID, int UserID)
        {
            try
            {
                int? UserRole = clsUser.GetUserRole(UserID);

                if (UserRole == null)
                {
                    return BadRequest($"You Have No Authorization To Update The Status Of The Order");
                }

                clsOrder Order = clsOrder.Find(OrderID);
                byte? OrderCurrentStatus = Order.GetOrderCurrentStatus();

                if (OrderCurrentStatus != (byte)clsOrder.enOrderStatus.ReadyToPick || OrderCurrentStatus != (byte)clsOrder.enOrderStatus.Arrived)
                {
                    return BadRequest($"The Order Cant Be Marked As Completed At This Current Status");
                }

                if (UserRole == (int)clsUserRole.enUserRole.Driver && OrderCurrentStatus != (byte)clsOrder.enOrderStatus.Arrived)
                {
                    return BadRequest($"The Driver Cant Update The Status Of The Order");
                }
                if (UserRole != (int)clsUserRole.enUserRole.Driver && OrderCurrentStatus == (byte)clsOrder.enOrderStatus.Arrived)
                {
                    return BadRequest($"Only The Driver can Mark The Order As Completed");
                }

                if (Order.MarkOrderAsCompleted())
                {
                    return Ok(Order.OrderDTO);
                }
                else
                {
                    return BadRequest("The Order Could Not Be Marked As Completed");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("SyncCartItemsWithDatabase", Name = "SyncCartItemsWithDatabase")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsCheckoutCartItemsDTO>> SyncCartItemsWithDatabase(List<clsCheckoutCartItemsDTO> CheckoutCartItems)
        {
            try
            {
                List<clsCheckoutCartItemsDTO> UpdatedCheckoutCartItemsDTO = clsOrder.SyncCartItemsWithDatabase(CheckoutCartItems);

                if (UpdatedCheckoutCartItemsDTO.Count == 0)
                {
                    return NotFound("The Updated Checkout Cart Does Not Have Any Items");
                }
                return Ok(UpdatedCheckoutCartItemsDTO);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
    }
}
