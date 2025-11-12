using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantBusiness;
using RestaurantData;
using RestaurantDTOs;

namespace RestaurantApi.Controllers
{
    [Route("api/Shifts")]
    [ApiController]
    public class ShiftsController : ControllerBase
    {
        [HttpPost("AddNewShift", Name = "AddNewShift")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsShiftDTO> AddNewShift(clsShiftDTO ShiftDTO)
        {
            try
            {
                clsShift Shift = new clsShift();

                Shift.ShiftDay = ShiftDTO.ShiftDay;
                Shift.ShiftStart = ShiftDTO.ShiftStart;
                Shift.ShiftEnd = ShiftDTO.ShiftEnd;

                if (Shift.Save())
                {
                    ShiftDTO.ShiftID = Shift.ShiftID;
                    return CreatedAtRoute("GetShiftByID", new { ShiftID = ShiftDTO.ShiftID }, ShiftDTO);
                }
                else
                {
                    return BadRequest("The Shift Could Not Be Added");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpPut("UpdateShift", Name = "UpdateShift")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsShiftDTO> UpdateShift(clsShiftDTO ShiftDTO)
        {
            try
            {
                clsShift Shift = clsShift.Find(ShiftDTO.ShiftID);

                if (Shift == null)
                {
                    return NotFound("Could Not Find The Shift");
                }

                Shift.ShiftDay = ShiftDTO.ShiftDay;
                Shift.ShiftStart = ShiftDTO.ShiftStart;
                Shift.ShiftEnd = ShiftDTO.ShiftEnd;

                if (Shift.Save())
                {
                    return Ok(Shift.ShiftDTO);
                }
                else
                {
                    return BadRequest("The Shift Could Not Be Updated");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpDelete("DeleteShiftByID/{ShiftID}", Name = "DeleteShift")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult DeleteShift(int ShiftID)
        {
            try
            {
                clsShift Shift = clsShift.Find(ShiftID);

                if (Shift == null)
                {
                    return NotFound("Could Not Find The Shift");
                }

                if (clsShift.DeleteShift(ShiftID))
                {
                    return Ok("Shift Deleted Successfully");
                }
                else
                {
                    return BadRequest("Shift Could Not Be Deleted");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetShiftByID/{ShiftID}", Name = "GetShiftByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsShiftDTO> GetShiftByID(int ShiftID)
        {
            try
            {
                clsShift Shift = clsShift.Find(ShiftID);

                if (Shift == null)
                {
                    return NotFound("Could Not Find The Shift");
                }
                else
                {
                    return Ok(Shift.ShiftDTO);
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetAllShifts", Name = "GetAllShifts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsShiftDTO>> GetAllShifts()
        {
            try
            {
                List<clsShiftDTO> AllShiftsList = clsShift.GetAllShifts();

                if (AllShiftsList.Count == 0)
                {
                    return NotFound("There Is No Shifts To Show");
                }
                return Ok(AllShiftsList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("IsRestaurantOpenNow", Name = "IsRestaurantOpenNow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult IsRestaurantOpenNow()
        {
            string OpeningHour = "";
            try
            {
                if (clsShift.IsRestaurantOpenNow(ref OpeningHour))
                {
                    return Ok();
                }
                else
                {
                    return NotFound(OpeningHour);
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetDaysWithoutSchedule", Name = "GetDaysWithoutSchedule")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<int>> GetDaysWithoutSchedule()
        {
            try
            {
                return clsShift.GetDaysWithoutSchedule();
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
    }
}
