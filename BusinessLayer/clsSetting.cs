using System;
using System.Data;
using RestaurantData;
using RestaurantDTOs;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace RestaurantBusiness
{
    public class clsSetting
    {
        public byte CoinsToEarn {  get; set; }
        public int AmountToSpend { get; set; }
        public decimal DeliveryFeePerKilo {  get; set; }
        public int CoinWorth { get; set; }
        public string RestaurantAddress { get; set; }
        public decimal RestaurantLatitude { get; set; }
        public decimal RestaurantLongitude { get; set; }
        public string Currency { get; set; }
        public clsSettingDTO SettingDTO
        {
            get { return (new clsSettingDTO(this.CoinsToEarn, this.AmountToSpend, this.DeliveryFeePerKilo, CoinWorth, this.RestaurantAddress, this.RestaurantLatitude, this.RestaurantLongitude, this.Currency)); }
        }
        private clsSetting(byte coinsToEarn, int amountToSpend, decimal deliveryFeePerKilo, string restaurantAddress, decimal restaurantLatitude, decimal restaurantLongitude, string currency)
        {
            CoinsToEarn = coinsToEarn;
            AmountToSpend = amountToSpend;
            DeliveryFeePerKilo = deliveryFeePerKilo;
            RestaurantAddress = restaurantAddress;
            RestaurantLatitude = restaurantLatitude;
            RestaurantLongitude = restaurantLongitude;
            Currency = currency;
        }
        public static clsSetting GetSettingsInfo()
        {
            clsSettingDTO SettingDTO = clsSettingsData.Get();

            return new clsSetting(SettingDTO.CoinsToEarn, SettingDTO.AmountToSpend, SettingDTO.DeliveryFeePerKilo, SettingDTO.RestaurantAddress, SettingDTO.RestaurantLatitude,SettingDTO.RestaurantLongitude, SettingDTO.Currency);
        }
        public bool Save()
        {
            return clsSettingsData.Update(this.SettingDTO);
        }
    }
}
//public class clsSettingDTO
//{
//    public byte ID { get; private set; }
//    public byte CoinsToEarn { get; set; }
//    public int AmountToSpend { get; set; }
//    public decimal DeliveryFeePerKilo { get; set; }
//    public string RestaurantAddress { get; set; }
//    public string Currency { get; set; }
//    public clsSettingDTO(byte iD, byte coinsToEarn, int amountToSpend, decimal deliveryFeePerKilo, string restaurantAddress, string currency)
//    {
//        ID = iD;
//        CoinsToEarn = coinsToEarn;
//        AmountToSpend = amountToSpend;
//        DeliveryFeePerKilo = deliveryFeePerKilo;
//        RestaurantAddress = restaurantAddress;
//        Currency = currency;
//    }
//}