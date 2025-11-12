using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantBusiness;
using RestaurantData;
using RestaurantDTOs;

namespace RestaurantApi.Controllers
{
    [Route("api/Locations")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        [HttpPost("AddNewLocation", Name = "AddNewLocation")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsLocationDTO> AddNewLocation(clsLocationDTO LocationDTO)
        {
            try
            {
                clsLocation Location = new clsLocation();

                Location.LocationName = LocationDTO.LocationName;
                Location.LocationAddress = LocationDTO.LocationAddress;
                Location.Latitude = LocationDTO.Latitude;
                Location.Longitude = LocationDTO.Longitude;
                Location.UserID = LocationDTO.UserID;

                if (Location.Save())
                {
                    LocationDTO.LocationID = Location.LocationID;
                    return CreatedAtRoute("GetLocationByID", new { LocationID = LocationDTO.LocationID }, LocationDTO);
                }
                else
                {
                    return BadRequest("The Location Could Not Be Added");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpPut("UpdateLocation", Name = "UpdateLocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsLocationDTO> UpdateLocation(clsLocationDTO LocationDTO)
        {
            try
            {
                clsLocation Location = clsLocation.Find(LocationDTO.LocationID);

                if (Location == null)
                {
                    return NotFound("Could Not Find The Location");
                }

                Location.LocationName = LocationDTO.LocationName;
                Location.LocationAddress = LocationDTO.LocationAddress;
                Location.Latitude = LocationDTO.Latitude;
                Location.Longitude = LocationDTO.Longitude;

                if (Location.Save())
                {
                    return Ok(Location.LocationDTO);
                }
                else
                {
                    return BadRequest("The Location Could Not Be Updated");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpDelete("DeleteLocationByID/{LocationID}", Name = "DeleteLocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult DeleteLocation(int LocationID)
        {
            try
            {
                clsLocation Location = clsLocation.Find(LocationID);

                if (Location == null)
                {
                    return NotFound("Could Not Find The Location");
                }

                if (clsLocation.DeleteLocation(LocationID))
                {
                    return Ok("Location Deleted Successfully");
                }
                else
                {
                    return BadRequest("Location Could Not Be Deleted");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetLocationByID/{LocationID}", Name = "GetLocationByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsLocationDTO> GetLocationByID(int LocationID)
        {
            try
            {
                clsLocation Location = clsLocation.Find(LocationID);

                if (Location == null)
                {
                    return NotFound("Could Not Find The Location");
                }
                else
                {
                    return Ok(Location.LocationDTO);
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetAllUserLocations/{UserID}", Name = "GetAllUserLocations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsLocationDTO>> GetAllUserLocations(int UserID)
        {
            try
            {
                List<clsLocationDTO> AllUserLocationsList = clsLocation.GetAllLocations(UserID);

                if (AllUserLocationsList.Count == 0)
                {
                    return NotFound("There Is No Locations To Show");
                }
                return Ok(AllUserLocationsList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
    }
}
