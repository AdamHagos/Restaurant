using Microsoft.Data.SqlClient;
using RestaurantDTOs;
using System;
using System.Data;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace RestaurantData
{
    public class clsCoinsTransactionsData
    {
        public static List<clsCoinTransactionDTO> GetUserCoinsTransactionsHistory(int UserID)
        {
            var CoinsTransactionsHistoryList = new List<clsCoinTransactionDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetUserCoinsTransactionsHistory", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserID", UserID);

                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int TransactionStatusIndex = reader.GetOrdinal("TransactionStatus");
                        int TransactionDateIndex = reader.GetOrdinal("TransactionDate");
                        int CoinsIndex = reader.GetOrdinal("Coins");

                        while (reader.Read())
                        {
                            CoinsTransactionsHistoryList.Add(new clsCoinTransactionDTO
                            (
                                reader.GetByte(TransactionStatusIndex),
                                DateOnly.FromDateTime(reader.GetDateTime(TransactionDateIndex)),
                                reader.GetByte(CoinsIndex)
                            ));
                        }
                    }
                }

                return CoinsTransactionsHistoryList;
            }
        }
        public static int GetCoinsValue(int Coins)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_GetCoinsValue", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Coins", Coins);

                var CoinsValue = new SqlParameter("@CoinsValue", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(CoinsValue);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)CoinsValue.Value;
            }
        }
    }
}
