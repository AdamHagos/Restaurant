using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using RestaurantData;
using RestaurantDTOs;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace RestaurantBusiness
{
    public class clsCategory
    {
        public int CategoryID { get; private set; }
        public string CategoryName { get; set; }
        public string CategoryImageUrl { get; set; }
        public bool IsActive { get; set; }
        public clsCategoryDTO CategoryDTO
        {
            get { return (new clsCategoryDTO(this.CategoryID,this.CategoryImageUrl,this.CategoryName, this.IsActive)); }
        }
        private clsCategory(int categoryID, string categoryName, string categoryImageUrl, bool isActive)
        {
            this.CategoryID = categoryID;
            this.CategoryName = categoryName;
            this.CategoryImageUrl = categoryImageUrl;
            this.IsActive = isActive;
        }
        public clsCategory()
        {
            this.CategoryID = -1;
            this.CategoryName = "";
            this.CategoryImageUrl = "";
            this.IsActive = true;
        }
        public static clsCategory Find(int ID)
        {
            clsCategoryDTO CategoryDTO = clsCategoriesData.GetCategoryByID(ID);

            if (CategoryDTO != null)
            {
                return new clsCategory(ID, CategoryDTO.CategoryName, CategoryDTO.CategoryImageUrl,CategoryDTO.IsActive);
            }
            else
                return null;
        }
        public static bool DeleteCategory(int ID)
        {
            return clsCategoriesData.DeleteCategory(ID);
        }
        private bool _AddNewCategory()
        {
            this.CategoryID = clsCategoriesData.AddCategory(this.CategoryDTO);

            return (this.CategoryID != -1);
        }
        private bool _UpdateCategory()
        {
            return clsCategoriesData.UpdateCategory(this.CategoryDTO);
        }
        public bool Save()
        {
            if (this.CategoryID == -1)
            {
                return _AddNewCategory();
            }
            else
            {
                return _UpdateCategory();
            }
        }
        public static List<clsCategoryDTO> GetAllCategories()
        {
            return clsCategoriesData.GetAllCategories();
        }
        public List<clsProductDTO> GetAllProducts()
        {
            return clsProductsData.GetAllProducts(this.CategoryID);
        }
    }
}