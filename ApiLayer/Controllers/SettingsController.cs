using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantDTOs;
using RestaurantBusiness;

namespace RestaurantApi.Controllers
{
    [Route("api/Settings")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        [HttpGet("GetSettingsData", Name = "GetSettingsData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsSettingDTO> GetSettingsData()
        {
            try
            {
                clsSetting SettingsData = clsSetting.GetSettingsInfo();

                return Ok(SettingsData.SettingDTO);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpPut("UpdateSettingsData", Name = "UpdateSettingsData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsSettingDTO> UpdateSettingsData(clsSettingDTO SettingDTO)
        {
            try
            {
                clsSetting SettingsData = clsSetting.GetSettingsInfo();

                SettingsData.CoinsToEarn = SettingDTO.CoinsToEarn;
                SettingsData.AmountToSpend = SettingDTO.AmountToSpend;
                SettingsData.CoinWorth = SettingDTO.CoinWorth;
                SettingsData.RestaurantAddress = SettingDTO.RestaurantAddress;
                SettingsData.RestaurantLatitude = SettingDTO.RestaurantLatitude;
                SettingsData.RestaurantLongitude = SettingDTO.RestaurantLongitude;
                SettingsData.Currency = SettingDTO.Currency;

                if (SettingsData.Save())
                {
                    return Ok(SettingsData.SettingDTO);
                }
                else
                {
                    return BadRequest("Could not save the settings data");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
    }
}
