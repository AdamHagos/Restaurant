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
    public class clsOrderStatusesData
    {
        public static bool UpdateOrderStatus(int OrderID)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_UpdateOrderStatus", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@OrderID", OrderID);

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
        public static byte? GetOrderCurrentStatus(int OrderID)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetCurrentOrderStatus", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@OrderID", OrderID);

                var CurrentStatus = new SqlParameter("@CurrentStatus", SqlDbType.TinyInt)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(CurrentStatus);

                connection.Open();
                command.ExecuteNonQuery();

                if (CurrentStatus.Value == DBNull.Value)
                {
                    return null;
                }

                return (byte)CurrentStatus.Value;
            }
        }
        public static List<clsOrderStatusDTO> GetOrderStatuses(int OrderID)
        {
            var OrderStatusesList = new List<clsOrderStatusDTO>();

            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetOrderStatuses", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@OrderID", OrderID);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    int OrderIDIndex = reader.GetOrdinal("OrderID");
                    int OrderStatusIndex = reader.GetOrdinal("OrderStatus");
                    int StatusDateIndex = reader.GetOrdinal("StatusDate");

                    while (reader.Read())
                    {
                        OrderStatusesList.Add(new clsOrderStatusDTO
                        (
                            reader.GetInt32(OrderIDIndex),
                            reader.GetByte(OrderStatusIndex),
                            reader.GetDateTime(StatusDateIndex)
                        ));
                    }
                }
            }
            return OrderStatusesList;
        }
    }
}
