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
    public class clsCartsData
    {
        public static List<clsCartDTO> GetCartItemsByOrderID(int OrderID)
        {
            var CartItemsList = new List<clsCartDTO>();

            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetCartItemsByOrderID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@OrderID", OrderID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    int ProductNameIndex = reader.GetOrdinal("ProductName");
                    int ProductDescriptionIndex = reader.GetOrdinal("ProductDescription");
                    int QuantityIndex = reader.GetOrdinal("Quantity");
                    int PriceIndex = reader.GetOrdinal("Price");
                    int ImageURLIndex = reader.GetOrdinal("ImageURL");
                    int CaloriesIndex = reader.GetOrdinal("Calories");
                    int NotesIndex = reader.GetOrdinal("Notes");

                    if (reader.Read())
                    {
                        string? Notes = !reader.IsDBNull(NotesIndex) ? reader.GetString(NotesIndex) : null;
                        CartItemsList.Add(new clsCartDTO
                        (
                            reader.GetInt32(ProductNameIndex),
                            reader.GetString(ProductDescriptionIndex),
                            reader.GetByte(QuantityIndex),
                            reader.GetDecimal(PriceIndex),
                            reader.GetString(ImageURLIndex),
                            reader.GetInt32(CaloriesIndex),
                            Notes
                        ));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return CartItemsList;
        }
        public static decimal CalculateCartTotalAmount(List<clsCheckoutCartItemsDTO> CheckoutCartItems)
        {
            DataTable CheckoutCartItemsTable = new DataTable();
            CheckoutCartItemsTable.Columns.Add("ProductID", typeof(int));
            CheckoutCartItemsTable.Columns.Add("Quantity", typeof(byte));
            CheckoutCartItemsTable.Columns.Add("Price", typeof(decimal));
            CheckoutCartItemsTable.Columns.Add("Notes", typeof(string));

            for (int i = 0; i < CheckoutCartItems.Count; i++)
            {
                CheckoutCartItemsTable.Rows.Add(CheckoutCartItems[i].ProductID, CheckoutCartItems[i].Quantity, CheckoutCartItems[i].Price, CheckoutCartItems[i].Notes ?? (object)DBNull.Value);
            }

            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_CalculateCartTotalAmount", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter tvpParam = command.Parameters.AddWithValue("@CartItems", CheckoutCartItemsTable);
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "CheckoutCartType";

                var TotalAmount = new SqlParameter("@TotalAmount", SqlDbType.TinyInt)
                {
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(TotalAmount);

                connection.Open();
                command.ExecuteNonQuery();

                return (decimal)TotalAmount.Value;
            }
        }
        public static List<clsCheckoutCartItemsDTO> SyncCartItemsWithDatabase(List<clsCheckoutCartItemsDTO> CheckoutCartItems)
        {
            var SyncedCartItemsList = new List<clsCheckoutCartItemsDTO>();

            DataTable CheckoutCartItemsTable = new DataTable();
            CheckoutCartItemsTable.Columns.Add("ProductID", typeof(int));
            CheckoutCartItemsTable.Columns.Add("Quantity", typeof(byte));
            CheckoutCartItemsTable.Columns.Add("Price", typeof(decimal));
            CheckoutCartItemsTable.Columns.Add("Notes", typeof(string));

            for (int i = 0; i < CheckoutCartItems.Count; i++)
            {
                CheckoutCartItemsTable.Rows.Add(CheckoutCartItems[i].ProductID, CheckoutCartItems[i].Quantity, CheckoutCartItems[i].Price, CheckoutCartItems[i].Notes ?? (object)DBNull.Value);
            }

            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_syncCartItemsWithDatabase", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter tvpParam = command.Parameters.AddWithValue("@CartItems", CheckoutCartItemsTable);
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "CheckoutCartType";

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    int ProductNameIndex = reader.GetOrdinal("ProductName");
                    int QuantityIndex = reader.GetOrdinal("Quantity");
                    int PriceIndex = reader.GetOrdinal("Price");
                    int NotesIndex = reader.GetOrdinal("Notes");
                    if (reader.Read())
                    {
                        string? Notes = !reader.IsDBNull(NotesIndex) ? reader.GetString(NotesIndex) : null;
                        SyncedCartItemsList.Add(new clsCheckoutCartItemsDTO
                        (
                            reader.GetInt32(ProductNameIndex),
                            reader.GetByte(QuantityIndex),
                            reader.GetDecimal(PriceIndex),
                            Notes
                        ));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return SyncedCartItemsList;
        }
    }
}
