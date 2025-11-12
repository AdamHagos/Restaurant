using Microsoft.Data.SqlClient;
using System;
//using System.Collections.Generic;
using System.Data;
using RestaurantDTOs;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace RestaurantData
{
    public class clsAddOnsData
    {
        public static int AddAddOn(clsAddOnDTO AddOnDTO)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_AddNewAddOn", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50)
                {
                    Value = AddOnDTO.AddOnName
                });
                command.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal)
                {
                    Value = AddOnDTO.Price,
                    Precision = 10,
                    Scale = 2
                });
                command.Parameters.AddWithValue("@Active", AddOnDTO.IsActive);

                var ReturnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                ReturnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return (int)ReturnParam.Value;
            }
        }
        public static bool UpdateAddOn(clsAddOnDTO AddOnDTO)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_UpdateAddOn", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID", AddOnDTO.AddOnID);
                command.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50)
                {
                    Value = AddOnDTO.AddOnName
                });
                command.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal)
                {
                    Value = AddOnDTO.Price,
                    Precision = 10,
                    Scale = 2
                });
                command.Parameters.AddWithValue("@IsActive", AddOnDTO.IsActive);

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
        public static bool DeleteAddOn(int AddonID)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_DeleteAddOn", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", AddonID);

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal",SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
        public static clsAddOnDTO GetAddOnByID(int AddonID)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetAddOnByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", AddonID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    int AddOnNameIndex = reader.GetOrdinal("AddOnName");
                    int PriceIndex = reader.GetOrdinal("Price");
                    int IsActiveIndex = reader.GetOrdinal("IsActive");

                    if (reader.Read())
                    {
                        return new clsAddOnDTO
                        (
                            AddonID,
                            reader.GetString(AddOnNameIndex),
                            reader.GetInt32(PriceIndex),
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
        public static List<clsAddOnDTO> GetAllAddOns()
        {
            var AddOnsList = new List<clsAddOnDTO>();

            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetAllAddOns", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    int AddOnIDIndex = reader.GetOrdinal("AddOnID");
                    int AddOnNameIndex = reader.GetOrdinal("AddOnName");
                    int PriceIndex = reader.GetOrdinal("Price");
                    int IsActiveIndex = reader.GetOrdinal("IsActive");

                    if (reader.Read())
                    {
                        AddOnsList.Add(new clsAddOnDTO
                        (
                            reader.GetInt32(AddOnIDIndex),
                            reader.GetString(AddOnNameIndex),
                            reader.GetInt32(PriceIndex),
                            reader.GetBoolean(IsActiveIndex)
                        ));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return AddOnsList;
        }
        public static List<clsAddOnDTO> GetProductAddOns(int ProductID)
        {
            var ProductAddOnsList = new List<clsAddOnDTO>();

            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetProductAddOns", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ProductID", ProductID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    int AddOnIDIndex = reader.GetOrdinal("AddOnID");
                    int AddOnNameIndex = reader.GetOrdinal("AddOnName");
                    int PriceIndex = reader.GetOrdinal("Price");
                    int IsActiveIndex = reader.GetOrdinal("IsActive");

                    if (reader.Read())
                    {
                        ProductAddOnsList.Add(new clsAddOnDTO
                        (
                            reader.GetInt32(AddOnIDIndex),
                            reader.GetString(AddOnNameIndex),
                            reader.GetInt32(PriceIndex),
                            reader.GetBoolean(IsActiveIndex)
                        ));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return ProductAddOnsList;
        }
        public static List<clsAddOnDTO> GetAvailableAddOnsForProduct(int ProductID)
        {
            var AvailableAddOnsList = new List<clsAddOnDTO>();

            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetAvailableAddOnsForProduct", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ProductID", ProductID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    int AddOnIDIndex = reader.GetOrdinal("AddOnID");
                    int AddOnNameIndex = reader.GetOrdinal("AddOnName");
                    int PriceIndex = reader.GetOrdinal("Price");
                    int IsActiveIndex = reader.GetOrdinal("IsActive");

                    if (reader.Read())
                    {
                        AvailableAddOnsList.Add(new clsAddOnDTO
                        (
                            reader.GetInt32(AddOnIDIndex),
                            reader.GetString(AddOnNameIndex),
                            reader.GetInt32(PriceIndex),
                            reader.GetBoolean(IsActiveIndex)
                        ));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return AvailableAddOnsList;
        }
        public static List<clsAddOnDTO> GetAddOnsByIDs(List<int> AddOnIDs)
        {
            List<clsAddOnDTO> AddOnsList = new List<clsAddOnDTO>();

            DataTable AddOnIDsTable = new DataTable();
            AddOnIDsTable.Columns.Add("AddOnID", typeof(int));

            for (int i = 0; i < AddOnIDs.Count; i++)
            {
                AddOnIDsTable.Rows.Add(AddOnIDs[i]);
            }

            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetAddOnsByIDs", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter tvpParam = command.Parameters.AddWithValue("@AddOnIDs", AddOnIDsTable);
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "AddOnIDsTableType";

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    int AddOnIDIndex = reader.GetOrdinal("AddOnID");
                    int AddOnNameIndex = reader.GetOrdinal("AddOnName");
                    int PriceIndex = reader.GetOrdinal("Price");
                    int IsActiveIndex = reader.GetOrdinal("IsActive");

                    if (reader.Read())
                    {
                        AddOnsList.Add(new clsAddOnDTO
                        (
                            reader.GetInt32(AddOnIDIndex),
                            reader.GetString(AddOnNameIndex),
                            reader.GetInt32(PriceIndex),
                            reader.GetBoolean(IsActiveIndex)
                        ));
                    }
                    else
                    {
                        return null;
                    }
                }
                return AddOnsList;
            }
        }
    }
}