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
    public class clsUsersData
    {
        public static int AddUser(clsUserDTO UserDTO)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_AddNewUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
                {
                    Value = UserDTO.UserName
                });
                command.Parameters.AddWithValue("@DateCreated", UserDTO.DateCreated);
                command.Parameters.AddWithValue("@Coins", UserDTO.Coins);
                command.Parameters.Add(new SqlParameter("@DeviceToken", SqlDbType.NVarChar, 255)
                {
                    Value = UserDTO.DeviceToken
                });
                command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50)
                {
                    Value = UserDTO.Email
                });
                command.Parameters.Add(new SqlParameter("@PasswordHash", SqlDbType.NVarChar, 255)
                {
                    Value = UserDTO.PasswordHash
                });
                command.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NVarChar, 20)
                {
                    Value = UserDTO.Phone
                });

                var ReturnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                ReturnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return (int)ReturnParam.Value;
            }
        }
        public static bool UpdateUser(clsUserDTO UserDTO)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_UpdateUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID", UserDTO.UserID);
                command.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
                {
                    Value = UserDTO.UserName
                });
                command.Parameters.AddWithValue("@DateCreated", UserDTO.DateCreated);
                command.Parameters.AddWithValue("@Coins", UserDTO.Coins);
                command.Parameters.Add(new SqlParameter("@DeviceToken", SqlDbType.NVarChar, 255)
                {
                    Value = UserDTO.DeviceToken
                });
                command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50)
                {
                    Value = UserDTO.Email
                });
                command.Parameters.Add(new SqlParameter("@PasswordHash", SqlDbType.NVarChar, 255)
                {
                    Value = UserDTO.PasswordHash
                });
                command.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NVarChar, 20)
                {
                    Value = UserDTO.Phone
                });

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
        public static clsUserDTO GetUserByID(int ID)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetUserByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@UserID", ID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new clsUserDTO
                        (
                            ID,
                            reader.GetString(reader.GetOrdinal("UserName")),
                            reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                            reader.GetInt32(reader.GetOrdinal("Coins")),
                            reader.GetString(reader.GetOrdinal("DeviceToken")),
                            reader.GetString(reader.GetOrdinal("Email")),
                            reader.GetString(reader.GetOrdinal("PasswordHash")),
                            reader.GetString(reader.GetOrdinal("Phone"))
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public static clsUserDTO GetUserByUserNameAndPassword(string UserName, string PasswordHash)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetUserByUserNameANDPassword", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@UserName", UserName);
                command.Parameters.AddWithValue("@PasswordHash", PasswordHash);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new clsUserDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("UserID")),
                            UserName,
                            reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                            reader.GetInt32(reader.GetOrdinal("Coins")),
                            reader.GetString(reader.GetOrdinal("DeviceToken")),
                            reader.GetString(reader.GetOrdinal("Email")),
                            PasswordHash,
                            reader.GetString(reader.GetOrdinal("Phone"))
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public static clsUserDTO GetUserByUserName(string UserName)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetUserByUserName", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@UserName", UserName);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new clsUserDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("UserID")),
                            UserName,
                            reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                            reader.GetInt32(reader.GetOrdinal("Coins")),
                            reader.GetString(reader.GetOrdinal("DeviceToken")),
                            reader.GetString(reader.GetOrdinal("Email")),
                            reader.GetString(reader.GetOrdinal("PasswordHash")),
                            reader.GetString(reader.GetOrdinal("Phone"))
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public static List<clsUserDTO> GetAllUsers()
        {
            var UsersList = new List<clsUserDTO>();

            using (SqlConnection conn = new SqlConnection(clsDataSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllUsers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int UserIDIndex = reader.GetOrdinal("UserID");
                        int UserNameIndex = reader.GetOrdinal("UserName");
                        int DateCreatedIndex = reader.GetOrdinal("DateCreated");
                        int CoinsIndex = reader.GetOrdinal("Coins");
                        int DeviceTokenIndex = reader.GetOrdinal("DeviceToken");
                        int EmailIndex = reader.GetOrdinal("Email");
                        int PasswordHashIndex = reader.GetOrdinal("PasswordHash");
                        int PhoneIndex = reader.GetOrdinal("Phone");

                        while (reader.Read())
                        {
                            UsersList.Add(new clsUserDTO
                            (
                                reader.GetInt32(UserIDIndex),
                                reader.GetString(UserNameIndex),
                                reader.GetDateTime(DateCreatedIndex),
                                reader.GetInt32(CoinsIndex),
                                reader.GetString(DeviceTokenIndex),
                                reader.GetString(EmailIndex),
                                reader.GetString(PasswordHashIndex),
                                reader.GetString(PhoneIndex)
                            ));
                        }
                    }
                }
                return UsersList;
            }
        }
    }
}
