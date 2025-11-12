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
    public class clsShiftsData
    {
        public static int AddShift(clsShiftDTO ShiftDTO)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_AddNewShift", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Day", ShiftDTO.ShiftDay);
                command.Parameters.AddWithValue("@ShiftStart", ShiftDTO.ShiftStart);
                command.Parameters.AddWithValue("@ShiftEnd", ShiftDTO.ShiftEnd);

                var ReturnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                ReturnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return (int)ReturnParam.Value;
            }
        }
        public static bool UpdateShift(clsShiftDTO ShiftDTO)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_UpdateShift", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", ShiftDTO.ShiftID);
                command.Parameters.AddWithValue("@Day", ShiftDTO.ShiftDay);
                command.Parameters.AddWithValue("@ShiftStart", ShiftDTO.ShiftStart);
                command.Parameters.AddWithValue("@ShiftEnd", ShiftDTO.ShiftEnd);

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
        public static bool DeleteShift(int ShiftID)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_DeleteShift", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", ShiftID);

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
        public static clsShiftDTO GetShiftByID(int ID)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetShiftByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", ID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new clsShiftDTO
                        (
                            ID,
                            reader.GetByte(reader.GetOrdinal("ShiftDay")),
                            reader.GetTimeSpan(reader.GetOrdinal("ShiftStart")),
                            reader.GetTimeSpan(reader.GetOrdinal("ShiftEnd"))
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public static List<clsShiftDTO> GetAllShifts()
        {
            var ShiftsList = new List<clsShiftDTO>();

            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetAllShifts", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    int ShiftIDIndex = reader.GetOrdinal("ShiftID");
                    int ShiftDayIndex = reader.GetOrdinal("ShiftDay");
                    int ShiftStartIndex = reader.GetOrdinal("ShiftStart");
                    int ShiftEndIndex = reader.GetOrdinal("ShiftEnd");

                    if (reader.Read())
                    {
                        ShiftsList.Add(new clsShiftDTO
                        (
                            reader.GetInt32(ShiftIDIndex),
                            reader.GetByte(ShiftDayIndex),
                            reader.GetTimeSpan(ShiftStartIndex),
                            reader.GetTimeSpan(ShiftEndIndex)
                        ));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return ShiftsList;
        }
        public static bool IsRestaurantOpenNow()
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_IsRestaurantOpenNow", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Day", (int)DateTime.Now.DayOfWeek);
                command.Parameters.AddWithValue("@CurrentTime", DateTime.Now.TimeOfDay);

                SqlParameter returnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToBoolean(returnParam.Value);
            }
        }
        public static List<int> GetDaysWithoutSchedule()
        {
            var GetDaysWithoutShiftList = new List<int>();

            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetDaysWithoutSchedule", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    int NumIndex = reader.GetOrdinal("Num");
                    
                    if (reader.Read())
                    {
                        GetDaysWithoutShiftList.Add(reader.GetInt32(NumIndex));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return GetDaysWithoutShiftList;
        }
    }
}
