using Microsoft.Data.SqlClient;
using RestaurantDTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantData
{
    public class clsProductAddOnsData
    {
        public static bool AddProductAddOns(int ProductID, List<int>AddOnIDs)
        {
            DataTable AddOnIDsTable = new DataTable();
            AddOnIDsTable.Columns.Add("AddOnID", typeof(int));

            for (int i = 0; i < AddOnIDs.Count; i++)
            {
                AddOnIDsTable.Rows.Add(AddOnIDs[i]);
            }
            
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_AddNewProductAddOns", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ProductID", ProductID);

                SqlParameter tvpParam = command.Parameters.AddWithValue("@AddOnIDs", AddOnIDsTable);
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "ProductAddOnIDsType";

                var ReturnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                ReturnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(ReturnParam.Value);
            }
        }
        public static bool DeleteProductAddOns(int ProductID, List<int> AddOnIDs)
        {
            DataTable AddOnIDsTable = new DataTable();
            AddOnIDsTable.Columns.Add("AddOnID", typeof(int));

            for (int i = 0; i < AddOnIDs.Count; i++)
            {
                AddOnIDsTable.Rows.Add(AddOnIDs[i]);
            }

            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_DeleteProductAddOns", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ProductID", ProductID);

                SqlParameter tvpParam = command.Parameters.AddWithValue("@AddOnIDs", AddOnIDsTable);
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "ProductAddOnIDsType";

                var ReturnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                ReturnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(ReturnParam.Value);
            }
        }
    }
}
