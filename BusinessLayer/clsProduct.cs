using RestaurantData;
using System;
using System.Data;
using System.Diagnostics;
using RestaurantDTOs;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace RestaurantBusiness
{
    public class clsProduct
    {
        public int ProductID { get; private set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; }
        public string ImageUrl { get; set; }
        public int Calories { get; set; }
        public bool IsActive { get; set; }
        public clsProductDTO ProductDTO
        {
            get { return (new clsProductDTO(this.ProductID, this.ProductName, this.ProductDescription, this.Price, this.CategoryID, this.ImageUrl, this.Calories, this.IsActive)); }
        }
        private clsProduct(int productID, string productName, string productDescription, decimal price, int categoryID, string imageUrl, int calories, bool active)
        {
            this.ProductID = productID;
            this.ProductName = productName;
            this.ProductDescription = productDescription;
            this.Price = price;
            this.CategoryID = categoryID;
            this.ImageUrl = imageUrl;
            this.Calories = calories;
            this.IsActive = active;
        }
        public clsProduct()
        {
            this.ProductID = -1;
            this.ProductName = "";
            this.ProductDescription = "";
            this.Price = 0;
            this.CategoryID = -1;
            this.ImageUrl = "";
            this.Calories = 0;
            this.IsActive = true;
        }
        public static clsProduct Find(int ID)
        {
            clsProductDTO ProductDTO = clsProductsData.GetProductByID(ID);

            if (ProductDTO != null)
            {
                return new clsProduct(ID, ProductDTO.ProductName, ProductDTO.ProductDescription, ProductDTO.Price, ProductDTO.CategoryID, ProductDTO.ImageUrl, ProductDTO.Calories, ProductDTO.IsActive);
            }
            else
                return null;
        }
        public static bool DeleteProduct(int ID)
        {
            return clsProductsData.DeleteProduct(ID);
        }
        private bool _AddNewProduct()
        {
            this.ProductID = clsProductsData.AddProduct(this.ProductDTO);

            return (this.ProductID != -1);
        }
        private bool _UpdateProduct()
        {
            return clsProductsData.UpdateProduct(this.ProductDTO);
        }
        public bool Save()
        {
            if (this.ProductID == -1)
            {
                return _AddNewProduct();
            }
            else
            {
                return _UpdateProduct();
            }
        }
        public static List<clsProductDTO> GetAllProducts()
        {
            return clsProductsData.GetAllProducts();
        }
        public static List<clsProductDTO> GetAllProductsByIDs(List<int> ProductIDs)
        {
            return clsProductsData.GetAllProductsByIDs(ProductIDs);
        }
        public static List<clsProductDTO> GetAllProductsWithAddOn(int AddOnID)
        {
            return clsProductsData.GetAllProductsWithAddOn(AddOnID);
        }
        public bool AddProductAddOns(List<int> AddOnIDs)
        {
            return clsProductAddOnsData.AddProductAddOns(this.ProductID, AddOnIDs);
        }
        public bool DeleteProductAddOns(List<int> AddOnIDs)
        {
            return clsProductAddOnsData.DeleteProductAddOns(this.ProductID, AddOnIDs);
        }
    }
}
//public class clsProductDTO
//{
//    public int ID { get; set; }
//    public string Name { get; set; }
//    public string Description { get; set; }
//    public decimal Price { get; set; }
//    public int CategoryID { get; set; }
//    public string ImageUrl { get; set; }
//    public int Calories { get; set; }
//    public bool Active { get; set; }
//    public clsProductDTO(int iD, string name, string description, decimal price, int categoryID, string imageUrl, int calories, bool active)
//    {
//        ID = iD;
//        Name = name;
//        Description = description;
//        Price = price;
//        CategoryID = categoryID;
//        ImageUrl = imageUrl;
//        Calories = calories;
//        Active = active;
//    }
//}