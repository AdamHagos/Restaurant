using System;
using System.Data;
//using System.Security.Cryptography.X509Certificates;
using Microsoft.Data.SqlClient;
using RestaurantDTOs;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace RestaurantData
{
    public class clsPaymentsInfoData
    {
        public static clsPaymentInfoDTO GetOrderPaymentsInfo(int OrderID)
        {
            List<byte> PaymentMethodsList = new List<byte>();
            List<decimal?> AmountList = new List<decimal?>();

            using (SqlConnection conn = new SqlConnection(clsDataSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetOrderPaymentsInfo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@OrderID", OrderID);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int AmountIndex = reader.GetOrdinal("Amount");
                        int PaymentMethodIndex = reader.GetOrdinal("PaymentMethod");

                        while (reader.Read())
                        {
                            PaymentMethodsList.Add(reader.GetByte(PaymentMethodIndex));
                            
                            decimal? Amount = !reader.IsDBNull(AmountIndex) ? reader.GetDecimal(AmountIndex) : null;

                            AmountList.Add(Amount);
                        }
                    }
                }
            }
            return new clsPaymentInfoDTO(OrderID,PaymentMethodsList,AmountList);
        }
    }
}
