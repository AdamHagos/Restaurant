using System;
using System.Data;
using Microsoft.Data.SqlClient;
using RestaurantDTOs;

namespace RestaurantData
{
    public class clsCategoriesData
    {
        public static int AddCategory(clsCategoryDTO CategoryDTO)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_AddNewCategory", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50)
                {
                    Value = CategoryDTO.CategoryName
                });
                command.Parameters.Add(new SqlParameter("@ImageURL", SqlDbType.NVarChar, 50)
                {
                    Value = CategoryDTO.CategoryImageUrl
                });
                command.Parameters.AddWithValue("@Active", CategoryDTO.IsActive);

                var ReturnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                ReturnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return (int)ReturnParam.Value;
            }
        }
        public static bool UpdateCategory(clsCategoryDTO CategoryDTO)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_UpdateCategory", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", CategoryDTO.CategoryID);
                command.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50)
                {
                    Value = CategoryDTO.CategoryName
                });
                command.Parameters.Add(new SqlParameter("@ImageURL", SqlDbType.NVarChar, 50)
                {
                    Value = CategoryDTO.CategoryImageUrl
                });
                command.Parameters.AddWithValue("@Active", CategoryDTO.IsActive);

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
        public static bool DeleteCategory(int CategoryID)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_DeleteCategory", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", CategoryID);

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
        public static clsCategoryDTO GetCategoryByID(int CategoryID)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetCategoryByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CategoryID", CategoryID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    int CategoryNameIndex = reader.GetOrdinal("CategoryName");
                    int CategoryImageUrlIndex = reader.GetOrdinal("CategoryImageUrl");
                    int IsActiveIndex = reader.GetOrdinal("IsActive");

                    if (reader.Read())
                    {
                        return new clsCategoryDTO
                        (
                            CategoryID,
                            reader.GetString(CategoryNameIndex),
                            reader.GetString(CategoryImageUrlIndex),
                            reader.GetBoolean(IsActiveIndex)
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public static List<clsCategoryDTO> GetAllCategories()
        {
            var CategoriesList = new List<clsCategoryDTO>();

            using (SqlConnection conn = new SqlConnection(clsDataSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllCategories", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int CategoryIDIndex = reader.GetOrdinal("CategoryID");
                        int CategoryNameIndex = reader.GetOrdinal("CategoryName");
                        int CategoryImageUrlIndex = reader.GetOrdinal("CategoryImageUrl");
                        int IsActiveIndex = reader.GetOrdinal("IsActive");
                        
                        while (reader.Read())
                        {
                            CategoriesList.Add(new clsCategoryDTO
                            (
                                reader.GetInt32(CategoryIDIndex),
                                reader.GetString(CategoryNameIndex),
                                reader.GetString(CategoryImageUrlIndex),
                                reader.GetBoolean(IsActiveIndex)
                            ));
                        }
                    }
                }

                return CategoriesList;
            }
        }
    }
}
