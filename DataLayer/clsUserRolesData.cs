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
    public class clsUserRolesData
    {
        public static int AddUserRole(clsUserRoleDTO UserRoleDTO)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_AddNewUserRole", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Role", UserRoleDTO.UserRole);
                command.Parameters.AddWithValue("@UserID", UserRoleDTO.UserID);

                var ReturnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                ReturnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return (int)ReturnParam.Value;
            }
        }
        public static bool UpdateUserRole(clsUserRoleDTO UserRoleDTO)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_UpdateUserRole", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", UserRoleDTO.UserID);
                command.Parameters.AddWithValue("@UserRole", UserRoleDTO.UserRole);

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
        public static bool DeleteUserRole(int UserID)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_DeleteUserRole", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", UserID);

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
        public static clsUserRoleDTO GetUserRoleByID(int ID)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetUserRoleByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", ID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new clsUserRoleDTO
                        (
                            ID,
                            reader.GetByte(reader.GetOrdinal("UserRole")),
                            reader.GetInt32(reader.GetOrdinal("UserID"))
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public static byte? GetUsersRole(int UserID)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetUsersRole", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@UserID", UserID);

                var UserRole = new SqlParameter("@UserRole", SqlDbType.TinyInt)
                {
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(UserRole);

                connection.Open();
                command.ExecuteNonQuery();

                if (UserRole.Value == DBNull.Value)
                {
                    return null;
                }
                return (byte)UserRole.Value;
            }
        }
        public static List<clsUserRoleWithNameDTO> GetAllUserRoles()
        {
            var UsersRolesList = new List<clsUserRoleWithNameDTO>();

            using (SqlConnection conn = new SqlConnection(clsDataSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllUserRoles", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int UserRoleIDIndex = reader.GetOrdinal("UserRoleID");
                        int UserRoleIndex = reader.GetOrdinal("UserRole");
                        int UserIDIndex = reader.GetOrdinal("UserID");
                        int UserNameIndex = reader.GetOrdinal("UserName");

                        while (reader.Read())
                        {
                            UsersRolesList.Add(new clsUserRoleWithNameDTO
                            (
                                reader.GetInt32(UserRoleIDIndex),
                                reader.GetByte(UserRoleIndex),
                                reader.GetInt32(UserIDIndex),
                                reader.GetString(UserNameIndex)
                            ));
                        }
                    }
                }
            }
            return UsersRolesList;
        }
        public static bool DoesUserHaveRole(int UserID)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_DoesUserHaveRole", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@UserID", UserID);

                var outputParam = new SqlParameter("@UserID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputParam);

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(outputParam.Value);
            }
        }
        public static int DistributeDriverOrders(int DriverID)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_DistributeDriverOrders", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@DriverID", DriverID);

                var NumOfRowsAffected = new SqlParameter("@NumOfRowsAffected", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(NumOfRowsAffected);

                connection.Open();
                command.ExecuteNonQuery();
                
                return (int)NumOfRowsAffected.Value;
            }
        }
        public static List<clsUserRoleWithNameDTO> GetAllDrivers()
        {
            var DriversList = new List<clsUserRoleWithNameDTO>();

            using (SqlConnection conn = new SqlConnection(clsDataSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllDrivers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int UserRoleIDIndex = reader.GetOrdinal("UserRoleID");
                        int UserRoleIndex = reader.GetOrdinal("UserRole");
                        int UserIDIndex = reader.GetOrdinal("UserID");
                        int UserNameIndex = reader.GetOrdinal("UserName");

                        while (reader.Read())
                        {
                            DriversList.Add(new clsUserRoleWithNameDTO
                            (
                                reader.GetInt32(UserRoleIDIndex),
                                reader.GetByte(UserRoleIndex),
                                reader.GetInt32(UserIDIndex),
                                reader.GetString(UserNameIndex)
                            ));
                        }
                    }
                }
            }
            return DriversList;
        }
    }
}
//using (var reader = command.ExecuteReader())
//{
//    int ProductNameIndex = reader.GetOrdinal("ProductName");
//    int ProductDescriptionIndex = reader.GetOrdinal("ProductDescription");
//    int QuantityIndex = reader.GetOrdinal("Quantity");
//    int PriceIndex = reader.GetOrdinal("Price");
//    int ImageURLIndex = reader.GetOrdinal("ImageURL");
//    int CaloriesIndex = reader.GetOrdinal("Calories");
//    int NotesIndex = reader.GetOrdinal("Notes");

//    if (reader.Read())
//    {
//        string? Notes = !reader.IsDBNull(NotesIndex) ? reader.GetString(NotesIndex) : null;
//        CartItemsList.Add(new clsCartDTO
//        (
//            reader.GetInt32(ProductNameIndex),
//            reader.GetString(ProductDescriptionIndex),
//            reader.GetByte(QuantityIndex),
//            reader.GetDecimal(PriceIndex),
//            reader.GetString(ImageURLIndex),
//            reader.GetInt32(CaloriesIndex),
//            Notes
//        ));
//    }
//    else
//    {
//        return null;
//    }
//}
