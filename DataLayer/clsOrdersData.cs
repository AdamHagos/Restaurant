using Microsoft.Data.SqlClient;
using RestaurantDTOs;
using System;
using System.Data;
using System.Drawing;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace RestaurantData
{
    public class clsOrdersData
    {
        public static int AddOrder(clsOrderDTO OrderDTO, clsPaymentInfoDTO PaymentInfoDTO, List<clsCheckoutCartItemsDTO> CheckoutCartItemsDTO)
        {
            DataTable CheckoutCartItemsTable = new DataTable();
            CheckoutCartItemsTable.Columns.Add("ProductID", typeof(int));
            CheckoutCartItemsTable.Columns.Add("Quantity", typeof(byte));
            CheckoutCartItemsTable.Columns.Add("Price", typeof(decimal));
            CheckoutCartItemsTable.Columns.Add("Notes", typeof(string));

            for (int i = 0; i < CheckoutCartItemsDTO.Count; i++)
            {
                CheckoutCartItemsTable.Rows.Add(CheckoutCartItemsDTO[i].ProductID, CheckoutCartItemsDTO[i].Quantity, CheckoutCartItemsDTO[i].Price, CheckoutCartItemsDTO[i].Notes ?? (object)DBNull.Value);
            }

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_ProcessOrder", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@OrderDate", OrderDTO.OrderDate);
                command.Parameters.AddWithValue("@ServiceType", OrderDTO.ServiceType);
                command.Parameters.Add(new SqlParameter("@TotalAmount", SqlDbType.Decimal)
                {
                    Value = OrderDTO.TotalAmount,
                    Precision = 10,
                    Scale = 2
                });
                command.Parameters.Add(new SqlParameter("@DeliveryFee", SqlDbType.Decimal)
                {
                    Value = (object?)OrderDTO.DeliveryFee ?? DBNull.Value,
                    Precision = 10,
                    Scale = 2
                });
                command.Parameters.AddWithValue("@LocationID", (object?)OrderDTO.LocationID ?? DBNull.Value);
                command.Parameters.AddWithValue("@UserID", OrderDTO.UserID);
                command.Parameters.AddWithValue("@DriverID", (object?)OrderDTO.DriverID ?? DBNull.Value);
                command.Parameters.AddWithValue("@FirstPaymentMethod", PaymentInfoDTO.PaymentMethod[0]);
                command.Parameters.Add(new SqlParameter("@FirstAmount", SqlDbType.Decimal)
                {
                    Value = (object?)PaymentInfoDTO.Amount[0] ?? DBNull.Value,
                    Precision = 10,
                    Scale = 2
                });
                command.Parameters.AddWithValue("@SecondPaymentMethod", PaymentInfoDTO.PaymentMethod[1]);
                command.Parameters.AddWithValue("@SecondAmount", (object?)PaymentInfoDTO.Amount[1] ?? DBNull.Value);
                command.Parameters.Add(new SqlParameter("@SecondAmount", SqlDbType.Decimal)
                {
                    Value = (object?)PaymentInfoDTO.Amount[1] ?? DBNull.Value,
                    Precision = 10,
                    Scale = 2
                });

                SqlParameter tvpParam = command.Parameters.AddWithValue("@CartItems", CheckoutCartItemsTable);
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "CheckoutCartType";

                var ReturnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                ReturnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return (int)ReturnParam.Value;
            }
        }
        public static bool UpdateOrder(clsOrderDTO OrderDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_UpdateOrder", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", OrderDTO.OrderID);
                command.Parameters.AddWithValue("@OrderDate", OrderDTO.OrderDate);
                command.Parameters.AddWithValue("@ServiceType", OrderDTO.ServiceType);
                command.Parameters.AddWithValue("@TotalAmount", OrderDTO.TotalAmount);
                command.Parameters.AddWithValue("@DeliveryFee", OrderDTO.DeliveryFee);
                command.Parameters.AddWithValue("@LocationID", OrderDTO.LocationID);
                command.Parameters.AddWithValue("@UserID", OrderDTO.UserID);
                command.Parameters.AddWithValue("@DriverID", OrderDTO.DriverID);

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
        public static bool UpdateOrderDriver(int OrderID, int NewDriverID)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_UpdateOrderDriver", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", OrderID);
                command.Parameters.AddWithValue("@DriverID", NewDriverID);

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
        public static clsOrderDTO? GetOrderByID(int OrderID)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_GetOrderByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@OrderID", OrderID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new clsOrderDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("OrderID")),
                            reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                            reader.GetByte(reader.GetOrdinal("ServiceType")),
                            reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                            reader.GetDecimal(reader.GetOrdinal("DeliveryFee")),
                            reader.GetInt32(reader.GetOrdinal("LocationID")),
                            reader.GetInt32(reader.GetOrdinal("UserID")),
                            reader.GetInt32(reader.GetOrdinal("DriverID")),
                            reader.GetBoolean(reader.GetOrdinal("IsComplete"))
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public static List<clsOrderDTO> GetAllOrders()
        {
            var AllOrdersList = new List<clsOrderDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllOrders", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int OrderIDIndex = reader.GetOrdinal("OrderID");
                        int OrderDateIndex = reader.GetOrdinal("OrderDate");
                        int ServiceTypeIndex = reader.GetOrdinal("ServiceType");
                        int TotalAmountIndex = reader.GetOrdinal("TotalAmount");
                        int DeliveryFeeIndex = reader.GetOrdinal("DeliveryFee");
                        int LocationIDIndex = reader.GetOrdinal("LocationID");
                        int UserIDIndex = reader.GetOrdinal("UserID");
                        int DriverIDIndex = reader.GetOrdinal("DriverID");
                        int IsCompleteIndex = reader.GetOrdinal("IsComplete");

                        while (reader.Read())
                        {
                            AllOrdersList.Add(new clsOrderDTO
                            (
                                reader.GetInt32(OrderIDIndex),
                                reader.GetDateTime(OrderDateIndex),
                                reader.GetByte(ServiceTypeIndex),
                                reader.GetDecimal(TotalAmountIndex),
                                reader.GetDecimal(DeliveryFeeIndex),
                                reader.GetInt32(LocationIDIndex),
                                reader.GetInt32(UserIDIndex),
                                reader.GetInt32(DriverIDIndex),
                                reader.GetBoolean(IsCompleteIndex)
                            ));
                        }
                    }
                }
            }

            return AllOrdersList;
        }
        public static List<clsOrderDTO> GetAllUserOrders(int UserID)
        {
            var UserOrdersList = new List<clsOrderDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllUserOrders", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", UserID);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int OrderIDIndex = reader.GetOrdinal("OrderID");
                        int OrderDateIndex = reader.GetOrdinal("OrderDate");
                        int ServiceTypeIndex = reader.GetOrdinal("ServiceType");
                        int TotalAmountIndex = reader.GetOrdinal("TotalAmount");
                        int DeliveryFeeIndex = reader.GetOrdinal("DeliveryFee");
                        int LocationIDIndex = reader.GetOrdinal("LocationID");
                        int UserIDIndex = reader.GetOrdinal("UserID");
                        int DriverIDIndex = reader.GetOrdinal("DriverID");
                        int IsCompleteIndex = reader.GetOrdinal("IsComplete");

                        while (reader.Read())
                        {
                            UserOrdersList.Add(new clsOrderDTO
                            (
                                reader.GetInt32(OrderIDIndex),
                                reader.GetDateTime(OrderDateIndex),
                                reader.GetByte(ServiceTypeIndex),
                                reader.GetDecimal(TotalAmountIndex),
                                reader.GetDecimal(DeliveryFeeIndex),
                                reader.GetInt32(LocationIDIndex),
                                reader.GetInt32(UserIDIndex),
                                reader.GetInt32(DriverIDIndex),
                                reader.GetBoolean(IsCompleteIndex)
                            ));
                        }
                    }
                }
            }
            return UserOrdersList;
        }
        public static List<clsOrderDTO> GetAllOrders(int OrderStatus)
        {
            var OrdersList = new List<clsOrderDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllOrdersByOrderStatus", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@OrderStatus", OrderStatus);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int OrderIDIndex = reader.GetOrdinal("OrderID");
                        int OrderDateIndex = reader.GetOrdinal("OrderDate");
                        int ServiceTypeIndex = reader.GetOrdinal("ServiceType");
                        int TotalAmountIndex = reader.GetOrdinal("TotalAmount");
                        int DeliveryFeeIndex = reader.GetOrdinal("DeliveryFee");
                        int LocationIDIndex = reader.GetOrdinal("LocationID");
                        int UserIDIndex = reader.GetOrdinal("UserID");
                        int DriverIDIndex = reader.GetOrdinal("DriverID");
                        int IsCompleteIndex = reader.GetOrdinal("IsComplete");

                        while (reader.Read())
                        {
                            OrdersList.Add(new clsOrderDTO
                            (
                                reader.GetInt32(OrderIDIndex),
                                reader.GetDateTime(OrderDateIndex),
                                reader.GetByte(ServiceTypeIndex),
                                reader.GetDecimal(TotalAmountIndex),
                                reader.GetDecimal(DeliveryFeeIndex),
                                reader.GetInt32(LocationIDIndex),
                                reader.GetInt32(UserIDIndex),
                                reader.GetInt32(DriverIDIndex),
                                reader.GetBoolean(IsCompleteIndex)
                            ));
                        }
                    }
                }
            }
            return OrdersList;
        }
        public static int? GetUserLastOrderID(int UserID)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_GetUserLastOrderID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@UserID", UserID);

                var UserLastOrderID = new SqlParameter("@UserLastOrderID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(UserLastOrderID);

                connection.Open();
                command.ExecuteNonQuery();

                if (UserLastOrderID.Value == DBNull.Value)
                {
                    return null;
                }

                return (int)UserLastOrderID.Value;
            }
        }
        public static decimal GetOrderRemainingAmount(int OrderID, int CoinsValue, decimal TotalAmount)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_GetOrderRemainingAmount", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@OrderID", OrderID);
                command.Parameters.AddWithValue("@CoinsValue", CoinsValue);
                command.Parameters.AddWithValue("@TotalAmount", TotalAmount);

                var RemainingAmount = new SqlParameter("@RemainingAmount", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(RemainingAmount);

                connection.Open();
                command.ExecuteNonQuery();

                return (decimal)RemainingAmount.Value;
            }
        }
        public static decimal GetOrderTotalAmount(int OrderID)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_GetOrderTotalAmount", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@OrderID", OrderID);

                var OrderTotalAmount = new SqlParameter("@OrderTotalAmount", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(OrderTotalAmount);

                connection.Open();
                command.ExecuteNonQuery();

                return (decimal)OrderTotalAmount.Value;
            }
        }
        public static bool IsOrderDelivery(int OrderID)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_IsOrderDelivery", connection))
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
        public static bool IsOrderAssignedToDriver(int OrderID, int DriverID)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_IsOrderAssignedToDriver", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@OrderID", OrderID);
                command.Parameters.AddWithValue("@DriverID", DriverID);

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
        public static bool MarkOrderAsCompleted(int OrderID, decimal RemainingAmount)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_MarkOrderAsCompleted", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@OrderIdentity", OrderID);
                command.Parameters.AddWithValue("@RemainingAmount", RemainingAmount);

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
    }
}
