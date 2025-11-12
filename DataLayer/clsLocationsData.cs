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
    public class clsLocationsData
    {
        public static int AddLocation(clsLocationDTO LocationDTO)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_AddNewLocation", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@LocationName", SqlDbType.NVarChar, 50)
                {
                    Value = LocationDTO.LocationName
                });
                command.Parameters.Add(new SqlParameter("@LocationAddress", SqlDbType.NVarChar, 50)
                {
                    Value = LocationDTO.LocationAddress
                });
                command.Parameters.Add(new SqlParameter("@Latitude", SqlDbType.Decimal)
                {
                    Value = LocationDTO.Latitude,
                    Precision = 9,
                    Scale = 6
                });
                command.Parameters.Add(new SqlParameter("@Longitude", SqlDbType.Decimal)
                {
                    Value = LocationDTO.Longitude,
                    Precision = 9,
                    Scale = 6
                });
                command.Parameters.AddWithValue("@UserID", LocationDTO.UserID);
                command.Parameters.AddWithValue("@IsActive", LocationDTO.IsActive);

                var ReturnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                ReturnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return (int)ReturnParam.Value;
            }
        }
        public static bool UpdateLocation(clsLocationDTO LocationDTO)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_UpdateLocation", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", LocationDTO.LocationID);
                command.Parameters.Add(new SqlParameter("@LocationName", SqlDbType.NVarChar, 50)
                {
                    Value = LocationDTO.LocationName
                });
                command.Parameters.Add(new SqlParameter("@LocationAddress", SqlDbType.NVarChar, 50)
                {
                    Value = LocationDTO.LocationAddress
                });
                command.Parameters.Add(new SqlParameter("@Latitude", SqlDbType.Decimal)
                {
                    Value = LocationDTO.Latitude,
                    Precision = 9,
                    Scale = 6
                });
                command.Parameters.Add(new SqlParameter("@Longitude", SqlDbType.Decimal)
                {
                    Value = LocationDTO.Longitude,
                    Precision = 9,
                    Scale = 6
                });
                command.Parameters.AddWithValue("@UserID", LocationDTO.UserID);
                command.Parameters.AddWithValue("@IsActive", LocationDTO.IsActive);

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
        public static bool DeleteLocation(int LocationID)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_DeleteLocation", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", LocationID);

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
        public static clsLocationDTO GetLocationByID(int LocationID)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetLocationByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LocationID", LocationID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    int LocationNameIndex = reader.GetOrdinal("LocationName");
                    int LocationAddressIndex = reader.GetOrdinal("LocationAddress");
                    int LatitudeIndex = reader.GetOrdinal("Latitude");
                    int LongitudeIndex = reader.GetOrdinal("Longitude");
                    int UserIDIndex = reader.GetOrdinal("UserID");
                    int IsActiveIndex = reader.GetOrdinal("IsActive");
                    if (reader.Read())
                    {
                        return new clsLocationDTO
                        (
                            LocationID,
                            reader.GetString(LocationNameIndex),
                            reader.GetString(LocationAddressIndex),
                            reader.GetDecimal(LatitudeIndex),
                            reader.GetDecimal(LongitudeIndex),
                            reader.GetInt32(UserIDIndex),
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
        public static List<clsLocationDTO> GetAllLocations(int UserID)
        {
            var LocationsList = new List<clsLocationDTO>();

            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetAllLocations", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", UserID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    int LocationIDIndex = reader.GetOrdinal("LocationID");
                    int LocationNameIndex = reader.GetOrdinal("LocationName");
                    int LocationAddressIndex = reader.GetOrdinal("LocationAddress");
                    int LatitudeIndex = reader.GetOrdinal("Latitude");
                    int LongitudeIndex = reader.GetOrdinal("Longitude");
                    int UserIDIndex = reader.GetOrdinal("UserID");
                    int IsActiveIndex = reader.GetOrdinal("IsActive");

                    if (reader.Read())
                    {
                        LocationsList.Add(new clsLocationDTO
                        (
                            reader.GetInt32(LocationIDIndex),
                            reader.GetString(LocationNameIndex),
                            reader.GetString(LocationAddressIndex),
                            reader.GetDecimal(LatitudeIndex),
                            reader.GetDecimal(LongitudeIndex),
                            reader.GetInt32(UserIDIndex),
                            reader.GetBoolean(IsActiveIndex)
                        )
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return LocationsList;
        }
    }
}
