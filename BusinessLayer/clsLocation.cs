using RestaurantData;
using System;
using System.Data;
using System.Net;
using RestaurantDTOs;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace RestaurantBusiness
{
    public class clsLocation
    {
        public int LocationID { get; private set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int UserID { get; set; }
        public bool IsActive { get; set; }
        public clsLocationDTO LocationDTO
        {
            get { return (new clsLocationDTO(this.LocationID, this.LocationName, this.LocationAddress, this.Latitude, this.Longitude, this.UserID, this.IsActive)); }
        }
        private clsLocation(int LocationiD, string locationName, string locationAddress, decimal latitude, decimal longitude, int userID, bool isActive)
        {
            this.LocationID = LocationiD;
            this.LocationName = locationName;
            this.LocationAddress = locationAddress;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.UserID = userID;
            this.IsActive = isActive;
        }
        public clsLocation()
        {
            this.LocationID = -1;
            this.LocationName = "";
            this.LocationAddress = "";
            this.Latitude = 0;
            this.Longitude = 0;
            this.UserID = -1;
            this.IsActive = true;
        }
        public static clsLocation Find(int ID)
        {
            clsLocationDTO LocationDTO = clsLocationsData.GetLocationByID(ID);

            if (LocationDTO != null)
            {
                return new clsLocation(ID, LocationDTO.LocationName, LocationDTO.LocationAddress, LocationDTO.Latitude, LocationDTO.Longitude, LocationDTO.UserID, LocationDTO.IsActive);
            }
            else
                return null;
        }
        public static bool DeleteLocation(int ID)
        {
            return clsLocationsData.DeleteLocation(ID);
        }
        private bool _AddNewLocation()
        {
            this.LocationID = clsLocationsData.AddLocation(this.LocationDTO);

            return (this.LocationID != -1);
        }
        private bool _UpdateLocation()
        {
            return clsLocationsData.UpdateLocation(this.LocationDTO);
        }
        public bool Save()
        {
            if (this.LocationID == -1)
            {
                return _AddNewLocation();
            }
            else
            {
                return _UpdateLocation();
            }
        }
        public static List<clsLocationDTO> GetAllLocations(int UserID)
        {
            return clsLocationsData.GetAllLocations(UserID);
        }
    }
}
//public class clsLocationDTO
//{
//    public int ID { get; set; }
//    public string Name { get; set; }
//    public string Address { get; set; }
//    public decimal Latitude { get; set; }
//    public decimal Longitude { get; set; }
//    public int UserID { get; set; }
//    public bool Active { get; set; }
//    public clsLocationDTO(int iD, string name, string address, decimal latitude, decimal longitude, int userID, bool active)
//    {
//        ID = iD;
//        Name = name;
//        Address = address;
//        Latitude = latitude;
//        Longitude = longitude;
//        UserID = userID;
//        Active = active;
//    }
//}