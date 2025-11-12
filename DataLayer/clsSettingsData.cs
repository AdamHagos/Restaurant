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
    public class clsSettingsData
    {
        public static clsSettingDTO Get()
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetSettingsData", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new clsSettingDTO
                        (
                            reader.GetByte(reader.GetOrdinal("CoinsToEarn")),
                            reader.GetInt32(reader.GetOrdinal("AmountToSpend")),
                            reader.GetDecimal(reader.GetOrdinal("DeliveryFeePerKilo")),
                            reader.GetInt32(reader.GetOrdinal("CoinWorth")),
                            reader.GetString(reader.GetOrdinal("RestaurantAddress")),
                            reader.GetDecimal(reader.GetOrdinal("RestaurantLatitude")),
                            reader.GetDecimal(reader.GetOrdinal("RestaurantLongitude")),
                            reader.GetString(reader.GetOrdinal("Currency"))
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public static bool Update(clsSettingDTO SettingDTO)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_UpdateSettingsData", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CoinsToEarn", SettingDTO.CoinsToEarn);
                command.Parameters.AddWithValue("@AmountToSpend", SettingDTO.AmountToSpend);
                command.Parameters.AddWithValue("@CoinWorth", SettingDTO.CoinWorth);
                command.Parameters.Add(new SqlParameter("@DeliveryFeePerKilo", SqlDbType.Decimal)
                {
                    Value = SettingDTO.DeliveryFeePerKilo,
                    Precision = 10,
                    Scale = 2
                });
                command.Parameters.Add(new SqlParameter("@RestaurantAddress", SqlDbType.NVarChar, 100)
                {
                    Value = SettingDTO.RestaurantAddress
                });
                command.Parameters.Add(new SqlParameter("@RestaurantLatitude", SqlDbType.Decimal)
                {
                    Value = SettingDTO.RestaurantLatitude,
                    Precision = 9,
                    Scale = 6
                });
                command.Parameters.Add(new SqlParameter("@RestaurantLongitude", SqlDbType.Decimal)
                {
                    Value = SettingDTO.RestaurantLongitude,
                    Precision = 9,
                    Scale = 6
                });
                command.Parameters.Add(new SqlParameter("@Currency", SqlDbType.NVarChar, 100)
                {
                    Value = SettingDTO.Currency
                });

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
    }
}
