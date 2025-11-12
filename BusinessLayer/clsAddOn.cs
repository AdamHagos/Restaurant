using RestaurantBusiness;
using RestaurantData;
using RestaurantDTOs;
using System;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace RestaurantBusiness
{
    public class clsAddOn
    {
        public int AddOnID { get; private set; }
        public string AddOnName { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public clsAddOnDTO AddOnDTO
        {
            get { return (new clsAddOnDTO(this.AddOnID,this.AddOnName,this.Price,this.IsActive)); }
        }
        private clsAddOn(int AddOnid, string addOnName, decimal price, bool active)
        {
            this.AddOnID = AddOnid;
            this.AddOnName = addOnName;
            this.Price = price;
            this.IsActive = active;
        }
        public clsAddOn()
        {
            this.AddOnID = -1;
            this.AddOnName = "";
            this.Price = 0;
            this.IsActive = true;
        }
        public static clsAddOn Find(int ID)
        {
            clsAddOnDTO AddOnDTO = clsAddOnsData.GetAddOnByID(ID);

            if (AddOnDTO != null)
            {
                return new clsAddOn(ID,AddOnDTO.AddOnName,AddOnDTO.Price,AddOnDTO.IsActive);
            }
            else
                return null;
        }
        public static bool DeleteAddOn(int ID)
        {
            return clsAddOnsData.DeleteAddOn(ID);
        }
        private bool _AddNewAddOn()
        {
            this.AddOnID = clsAddOnsData.AddAddOn(this.AddOnDTO);

            return (this.AddOnID != -1);
        }
        private bool _UpdateAddOn()
        {
            return clsAddOnsData.UpdateAddOn(this.AddOnDTO);
        }
        public bool Save()
        {
            if (this.AddOnID == -1)
            {
                return _AddNewAddOn();
            }
            else
            {
                return _UpdateAddOn();
            }
        }
        public static List<clsAddOnDTO> GetAllAddOns()
        {
            return clsAddOnsData.GetAllAddOns();
        }
        public static List<clsAddOnDTO> GetProductAddOns(int ProductID)
        {
            return clsAddOnsData.GetProductAddOns(ProductID);
        }
        public static List<clsAddOnDTO> GetAvailableAddOnsForProduct(int ProductID)
        {
            return clsAddOnsData.GetAvailableAddOnsForProduct(ProductID);
        }
        public static List<clsAddOnDTO> GetAddOnsByIDs(List<int> AddOnIDs)
        {
            return clsAddOnsData.GetAddOnsByIDs(AddOnIDs);
        }
    }
}

