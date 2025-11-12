using System;
using System.Data;
using Microsoft.Data.SqlClient;
using RestaurantDTOs;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace RestaurantData
{
    public class clsProductsData
    {
        public static int AddProduct(clsProductDTO ProductDTO)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_AddNewProduct", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50)
                {
                    Value = ProductDTO.ProductName
                });
                command.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 1000)
                {
                    Value = ProductDTO.ProductDescription
                });
                command.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal)
                {
                    Value = ProductDTO.Price,
                    Precision = 10,
                    Scale = 2
                });
                command.Parameters.AddWithValue("@CategoryID", ProductDTO.CategoryID);
                command.Parameters.Add(new SqlParameter("@ImageUrl", SqlDbType.NVarChar, 2083)
                {
                    Value = ProductDTO.ImageUrl
                });
                command.Parameters.AddWithValue("@Calories", ProductDTO.Calories);
                command.Parameters.AddWithValue("@IsActive", ProductDTO.IsActive);

                var ReturnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                ReturnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return (int)ReturnParam.Value;
            }
        }
        public static bool UpdateProduct(clsProductDTO ProductDTO)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_UpdateProduct", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID", ProductDTO.ProductID);
                command.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50)
                {
                    Value = ProductDTO.ProductName
                });
                command.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 1000)
                {
                    Value = ProductDTO.ProductDescription
                });
                command.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal)
                {
                    Value = ProductDTO.Price,
                    Precision = 10,
                    Scale = 2
                });
                command.Parameters.AddWithValue("@CategoryID", ProductDTO.CategoryID);
                command.Parameters.Add(new SqlParameter("@ImageUrl", SqlDbType.NVarChar, 2083)
                {
                    Value = ProductDTO.ImageUrl
                });
                command.Parameters.AddWithValue("@Calories", ProductDTO.Calories);
                command.Parameters.AddWithValue("@IsActive", ProductDTO.IsActive);

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
        public static bool DeleteProduct(int ProductID)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_DeleteProduct", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", ProductID);

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
        public static clsProductDTO GetProductByID(int ProductID)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetProductID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", ProductID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new clsProductDTO
                        (
                            ProductID,
                            reader.GetString(reader.GetOrdinal("ProductName")),
                            reader.GetString(reader.GetOrdinal("ProductDescription")),
                            reader.GetDecimal(reader.GetOrdinal("Price")),
                            reader.GetInt32(reader.GetOrdinal("CategoryID")),
                            reader.GetString(reader.GetOrdinal("ImageUrl")),
                            reader.GetInt32(reader.GetOrdinal("Calories")),
                            reader.GetBoolean(reader.GetOrdinal("IsActive"))
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public static List<clsProductDTO> GetAllProducts()
        {
            var ProductsList = new List<clsProductDTO>();

            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetAllProducts", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    int ProductIDIndex = reader.GetOrdinal("ProductID");
                    int ProductNameIndex = reader.GetOrdinal("ProductName");
                    int ProductDescriptionIndex = reader.GetOrdinal("ProductDescription");
                    int PriceIndex = reader.GetOrdinal("Price");
                    int CategoryIDIndex = reader.GetOrdinal("CategoryID");
                    int ImageURLIndex = reader.GetOrdinal("ImageURL");
                    int CaloriesIndex = reader.GetOrdinal("Calories");
                    int IsActive = reader.GetOrdinal("IsActive");

                    if (reader.Read())
                    {
                        ProductsList.Add(new clsProductDTO
                        (
                            reader.GetInt32(ProductIDIndex),
                            reader.GetString(ProductNameIndex),
                            reader.GetString(ProductDescriptionIndex),
                            reader.GetDecimal(PriceIndex),
                            reader.GetInt32(CategoryIDIndex),
                            reader.GetString(ImageURLIndex),
                            reader.GetInt32(CaloriesIndex),
                            reader.GetBoolean(IsActive)
                        ));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return ProductsList;
        }
        public static List<clsProductDTO> GetAllProducts(int CategoryID)
        {
            var ProductsList = new List<clsProductDTO>();

            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetProductsByCategoryID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@CategoryID", CategoryID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    int ProductIDIndex = reader.GetOrdinal("ProductID");
                    int ProductNameIndex = reader.GetOrdinal("ProductName");
                    int ProductDescriptionIndex = reader.GetOrdinal("ProductDescription");
                    int PriceIndex = reader.GetOrdinal("Price");
                    int CategoryIDIndex = reader.GetOrdinal("CategoryID");
                    int ImageURLIndex = reader.GetOrdinal("ImageURL");
                    int CaloriesIndex = reader.GetOrdinal("Calories");
                    int IsActive = reader.GetOrdinal("IsActive");

                    if (reader.Read())
                    {
                        ProductsList.Add(new clsProductDTO
                        (
                            reader.GetInt32(ProductIDIndex),
                            reader.GetString(ProductNameIndex),
                            reader.GetString(ProductDescriptionIndex),
                            reader.GetDecimal(PriceIndex),
                            reader.GetInt32(CategoryIDIndex),
                            reader.GetString(ImageURLIndex),
                            reader.GetInt32(CaloriesIndex),
                            reader.GetBoolean(IsActive)
                        ));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return ProductsList;
        }
        public static List<clsProductDTO> GetAllProductsByIDs(List<int>ProductIDs)
        {
            DataTable ProductIDsTable = new DataTable();
            ProductIDsTable.Columns.Add("ProductID", typeof(int));

            for (int i = 0; i < ProductIDs.Count; i++)
            {
                ProductIDsTable.Rows.Add(ProductIDs[i]);
            }

            var ProductsList = new List<clsProductDTO>();

            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetProductsByIDs", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter tvpParam = command.Parameters.AddWithValue("@ProductIDs", ProductIDsTable);
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "ProductIDTableType";

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    int ProductIDIndex = reader.GetOrdinal("ProductID");
                    int ProductNameIndex = reader.GetOrdinal("ProductName");
                    int ProductDescriptionIndex = reader.GetOrdinal("ProductDescription");
                    int PriceIndex = reader.GetOrdinal("Price");
                    int CategoryIDIndex = reader.GetOrdinal("CategoryID");
                    int ImageURLIndex = reader.GetOrdinal("ImageURL");
                    int CaloriesIndex = reader.GetOrdinal("Calories");
                    int IsActive = reader.GetOrdinal("IsActive");

                    if (reader.Read())
                    {
                        ProductsList.Add(new clsProductDTO
                        (
                            reader.GetInt32(ProductIDIndex),
                            reader.GetString(ProductNameIndex),
                            reader.GetString(ProductDescriptionIndex),
                            reader.GetDecimal(PriceIndex),
                            reader.GetInt32(CategoryIDIndex),
                            reader.GetString(ImageURLIndex),
                            reader.GetInt32(CaloriesIndex),
                            reader.GetBoolean(IsActive)
                        ));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return ProductsList;
        }
        public static List<clsProductDTO> GetAllProductsWithAddOn(int AddOnID)
        {
            var ProductsList = new List<clsProductDTO>();

            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetProductsWithAddOn", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@AddOnID", AddOnID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    int ProductIDIndex = reader.GetOrdinal("ProductID");
                    int ProductNameIndex = reader.GetOrdinal("ProductName");
                    int ProductDescriptionIndex = reader.GetOrdinal("ProductDescription");
                    int PriceIndex = reader.GetOrdinal("Price");
                    int CategoryIDIndex = reader.GetOrdinal("CategoryID");
                    int ImageURLIndex = reader.GetOrdinal("ImageURL");
                    int CaloriesIndex = reader.GetOrdinal("Calories");
                    int IsActive = reader.GetOrdinal("IsActive");

                    if (reader.Read())
                    {
                        ProductsList.Add(new clsProductDTO
                        (
                            reader.GetInt32(ProductIDIndex),
                            reader.GetString(ProductNameIndex),
                            reader.GetString(ProductDescriptionIndex),
                            reader.GetDecimal(PriceIndex),
                            reader.GetInt32(CategoryIDIndex),
                            reader.GetString(ImageURLIndex),
                            reader.GetInt32(CaloriesIndex),
                            reader.GetBoolean(IsActive)
                        ));
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return ProductsList;
        }
    }
}
