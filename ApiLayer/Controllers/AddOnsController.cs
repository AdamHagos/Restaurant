using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RestaurantBusiness;
using RestaurantData;
using RestaurantDTOs;

namespace RestaurantApi.Controllers
{
    [Route("api/AddOns")]
    [ApiController]
    public class AddOnsController : ControllerBase
    {
        [HttpPost("AddNewAddOn", Name = "AddNewAddOn")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsAddOnDTO> AddNewAddOn(clsAddOnDTO AddOnDTO)
        {
            try
            {
                clsAddOn AddOn = new clsAddOn();

                AddOn.AddOnName = AddOnDTO.AddOnName;
                AddOn.Price = AddOnDTO.Price;

                if (AddOn.Save())
                {
                    AddOnDTO.AddOnID = AddOn.AddOnID;
                    return CreatedAtRoute("GetAddOnByID", new { AddOnID = AddOnDTO.AddOnID }, AddOnDTO);
                }
                else
                {
                    return BadRequest("The AddOn Could Not Be Added");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }

        }
        [HttpPut("UpdateAddOn", Name = "UpdateAddOn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsAddOnDTO> UpdateAddOn(clsAddOnDTO AddOnDTO)
        {
            try
            {
                clsAddOn AddOn = clsAddOn.Find(AddOnDTO.AddOnID);

                if (AddOn == null)
                {
                    return NotFound("Could Not Find The AddOn");
                }

                AddOn.AddOnName = AddOnDTO.AddOnName;
                AddOn.Price = AddOnDTO.Price;

                if (AddOn.Save())
                {
                    return Ok(AddOn.AddOnDTO);
                }
                else
                {
                    return BadRequest($"The {AddOnDTO.AddOnName} Could Not Be Updated");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpDelete("DeleteAddOnByID/{AddOnID}", Name = "DeleteAddOn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult DeleteAddOn(int AddOnID)
        {
            try
            {
                clsAddOn AddOn = clsAddOn.Find(AddOnID);

                if (AddOn == null)
                {
                    return NotFound("Could Not Find The AddOn");
                }

                if (clsAddOn.DeleteAddOn(AddOnID))
                {
                    return Ok("AddOn Deleted Successfully");
                }
                else
                {
                    return BadRequest("AddOn Could Not Be Deleted");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetAddOnByID/{AddOnID}", Name = "GetAddOnByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsAddOnDTO> GetAddOnByID(int AddOnID)
        {
            try
            {
                clsAddOn AddOn = clsAddOn.Find(AddOnID);

                if (AddOn == null)
                {
                    return NotFound("Could Not Find The AddOn");
                }
                else
                {
                    return Ok(AddOn.AddOnDTO);
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetAllAddOns", Name = "GetAllAddOns")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsAddOnDTO>> GetAllAddOns()
        {
            try
            {
                List<clsAddOnDTO> AddOnsList = clsAddOn.GetAllAddOns();

                if (AddOnsList.Count == 0)
                {
                    return NotFound("There Is No AddOns To Show");
                }
                return Ok(AddOnsList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetProductAddOns/{ProductID}", Name = "GetProductAddOns")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsAddOnDTO>> GetProductAddOns(int ProductID)
        {
            try
            {
                List<clsAddOnDTO> ProductAddOnsList = clsAddOn.GetProductAddOns(ProductID);

                if (ProductAddOnsList.Count == 0)
                {
                    return NotFound("The Product Does Not Have Any AddOns");
                }
                return Ok(ProductAddOnsList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetAvailableAddOnsForProduct/{ProductID}", Name = "GetAvailableAddOnsForProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsAddOnDTO>> GetAvailableAddOnsForProduct(int ProductID)
        {
            try
            {
                List<clsAddOnDTO> AvailableAddOnsList = clsAddOn.GetAvailableAddOnsForProduct(ProductID);

                if (AvailableAddOnsList.Count == 0)
                {
                    return NotFound("The Product Does Not Have Any Available AddOns");
                }
                return Ok(AvailableAddOnsList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetAddOnsByIDs", Name = "GetAddOnsByIDs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsAddOnDTO>> GetAddOnsByIDs([FromQuery] List<int> AddOnIDs)
        {
            try
            {
                List<clsAddOnDTO> AddOnsList = clsAddOn.GetAddOnsByIDs(AddOnIDs);

                if (AddOnsList.Count == 0)
                {
                    return NotFound("There Is No AddOns To Show");
                }
                return Ok(AddOnsList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
    }
}
