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
            using (var connection = new SqlConnection(_connectionString))
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
            using (var connection = new SqlConnection(_connectionString))
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
            using (var connection = new SqlConnection(_connectionString))
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
            using (var connection = new SqlConnection(_connectionString))
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

            using (var connection = new SqlConnection(_connectionString))
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

            using (var connection = new SqlConnection(_connectionString))
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

            using (var connection = new SqlConnection(_connectionString))
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
    }
}
//using System;
//using System.Data;
//using Microsoft.Data.SqlClient;

//namespace StudentDataAccessLayer
//{
//    public class StudentDTO
//    {
//        public StudentDTO(int id, string name, int age, int grade)
//        {
//            this.Id = id;
//            this.Name = name;
//            this.Age = age;
//            this.Grade = grade;
//        }


//        public int Id { get; set; }
//        public string Name { get; set; }
//        public int Age { get; set; }
//        public int Grade { get; set; }
//    }

//    public class StudentData
//    {
//        static string _connectionString = "Server=localhost;Database=StudentsDB;User Id=sa;Password=sa;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;";

//        public static List<StudentDTO> GetAllStudents()
//        {
//            var StudentsList = new List<StudentDTO>();

//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                using (SqlCommand cmd = new SqlCommand("SP_GetAllStudents", conn))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;

//                    conn.Open();

//                    using (SqlDataReader reader = cmd.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            StudentsList.Add(new StudentDTO
//                            (
//                                reader.GetInt32(reader.GetOrdinal("Id")),
//                                reader.GetString(reader.GetOrdinal("Name")),
//                                reader.GetInt32(reader.GetOrdinal("Age")),
//                                reader.GetInt32(reader.GetOrdinal("Grade"))
//                            ));
//                        }
//                    }
//                }


//                return StudentsList;
//            }

//        }

//        public static List<StudentDTO> GetPassedStudents()
//        {
//            var StudentsList = new List<StudentDTO>();

//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                using (SqlCommand cmd = new SqlCommand("SP_GetPassedStudents", conn))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;

//                    conn.Open();

//                    using (SqlDataReader reader = cmd.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            StudentsList.Add(new StudentDTO
//                            (
//                                reader.GetInt32(reader.GetOrdinal("Id")),
//                                reader.GetString(reader.GetOrdinal("Name")),
//                                reader.GetInt32(reader.GetOrdinal("Age")),
//                                reader.GetInt32(reader.GetOrdinal("Grade"))
//                            ));
//                        }
//                    }
//                }


//                return StudentsList;
//            }

//        }

//        public static double GetAverageGrade()
//        {
//            double averageGrade = 0;

//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                using (SqlCommand cmd = new SqlCommand("SP_GetAverageGrade", conn))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;

//                    conn.Open();

//                    object result = cmd.ExecuteScalar();
//                    if (result != DBNull.Value)
//                    {
//                        averageGrade = Convert.ToDouble(result);
//                    }
//                    else
//                        averageGrade = 0;

//                }
//            }

//            return averageGrade;
//        }

//        public static StudentDTO GetStudentById(int studentId)
//        {
//            using (var connection = new SqlConnection(_connectionString))
//            using (var command = new SqlCommand("SP_GetStudentById", connection))
//            {
//                command.CommandType = CommandType.StoredProcedure;
//                command.Parameters.AddWithValue("@StudentId", studentId);

//                connection.Open();
//                using (var reader = command.ExecuteReader())
//                {
//                    if (reader.Read())
//                    {
//                        return new StudentDTO
//                        (
//                            reader.GetInt32(reader.GetOrdinal("Id")),
//                            reader.GetString(reader.GetOrdinal("Name")),
//                            reader.GetInt32(reader.GetOrdinal("Age")),
//                            reader.GetInt32(reader.GetOrdinal("Grade"))
//                        );
//                    }
//                    else
//                    {
//                        return null;
//                    }
//                }
//            }
//        }


//        public static int AddStudent(StudentDTO StudentDTO)
//        {
//            using (var connection = new SqlConnection(_connectionString))
//            using (var command = new SqlCommand("SP_AddStudent", connection))
//            {
//                command.CommandType = CommandType.StoredProcedure;

//                command.Parameters.AddWithValue("@Name", StudentDTO.Name);
//                command.Parameters.AddWithValue("@Age", StudentDTO.Age);
//                command.Parameters.AddWithValue("@Grade", StudentDTO.Grade);
//                var outputIdParam = new SqlParameter("@NewStudentId", SqlDbType.Int)
//                {
//                    Direction = ParameterDirection.Output
//                };
//                command.Parameters.Add(outputIdParam);

//                connection.Open();
//                command.ExecuteNonQuery();

//                return (int)outputIdParam.Value;
//            }
//        }

//        public static bool UpdateStudent(StudentDTO StudentDTO)
//        {
//            using (var connection = new SqlConnection(_connectionString))
//            using (var command = new SqlCommand("SP_UpdateStudent", connection))
//            {
//                command.CommandType = CommandType.StoredProcedure;

//                command.Parameters.AddWithValue("@StudentId", StudentDTO.Id);
//                command.Parameters.AddWithValue("@Name", StudentDTO.Name);
//                command.Parameters.AddWithValue("@Age", StudentDTO.Age);
//                command.Parameters.AddWithValue("@Grade", StudentDTO.Grade);

//                connection.Open();
//                command.ExecuteNonQuery();
//                return true;

//            }
//        }

//        public static bool DeleteStudent(int studentId)
//        {

//            using (var connection = new SqlConnection(_connectionString))
//            using (var command = new SqlCommand("SP_DeleteStudent", connection))
//            {
//                command.CommandType = CommandType.StoredProcedure;
//                command.Parameters.AddWithValue("@StudentId", studentId);

//                connection.Open();

//                int rowsAffected = (int)command.ExecuteScalar();
//                return (rowsAffected == 1);


//            }
//        }
//    }
//}
//using System;
//using System.Data;
//using System.Data.SqlClient;
//using static System.Net.Mime.MediaTypeNames;

//namespace DVLD_DataAccess
//{
//    public class clsApplicationsDataAccess
//    {
//        public static int AddNewApplication(int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
//        {
//            int ApplicationID = -1;

//            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

//            string Query = @"INSERT INTO Applications(ApplicantPersonID,ApplicationDate,ApplicationTypeID,ApplicationStatus,LastStatusDate,PaidFees,CreatedByUserID)
//                              VALUES(@ApplicantPersonID,@ApplicationDate,@ApplicationTypeID,@ApplicationStatus,@LastStatusDate,@PaidFees,@CreatedByUserID);
//                                                    SELECT SCOPE_IDENTITY();";

//            SqlCommand command = new SqlCommand(Query, connection);

//            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
//            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
//            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
//            command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
//            command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
//            command.Parameters.AddWithValue("@PaidFees", PaidFees);
//            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
//            try
//            {
//                connection.Open();

//                object result = command.ExecuteScalar();

//                if (int.TryParse(result.ToString(), out int insertedID))
//                {
//                    ApplicationID = insertedID;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Error" + ex.Message);
//            }
//            finally
//            {
//                connection.Close();
//            }
//            return ApplicationID;
//        }

//        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
//        {
//            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

//            string Query = @"UPDATE Applications
//                             SET ApplicantPersonID = @ApplicantPersonID, ApplicationDate = @ApplicationDate, ApplicationTypeID = @ApplicationTypeID, ApplicationStatus = @ApplicationStatus, LastStatusDate = @LastStatusDate, PaidFees = @PaidFees, CreatedByUserID = @CreatedByUserID
//                             WHERE ApplicationID = @ApplicationID;";

//            SqlCommand command = new SqlCommand(Query, connection);

//            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
//            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
//            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
//            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
//            command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
//            command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
//            command.Parameters.AddWithValue("@PaidFees", PaidFees);
//            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

//            int rowsAffected = 0;
//            try
//            {
//                connection.Open();
//                rowsAffected = command.ExecuteNonQuery();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Error" + ex.Message);
//            }
//            finally
//            {
//                connection.Close();
//            }
//            return (rowsAffected > 0);
//        }

//        public static DataTable GetAllApplications()
//        {
//            DataTable dt = new DataTable();

//            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

//            string Query = @"Select * From Applications;";

//            SqlCommand command = new SqlCommand(Query, connection);

//            try
//            {
//                connection.Open();

//                SqlDataReader reader = command.ExecuteReader();

//                if (reader.HasRows)
//                {
//                    dt.Load(reader);
//                }
//                reader.Close();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Error" + ex.Message);
//            }
//            finally
//            {
//                connection.Close();
//            }
//            return dt;
//        }

//        public static bool DeleteApplication(int ID)
//        {
//            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

//            string Query = @"DELETE FROM Applications
//                             WHERE ApplicationID = @ApplicationID;";

//            SqlCommand command = new SqlCommand(Query, connection);

//            command.Parameters.AddWithValue("@ApplicationID", ID);

//            int rowsAffected = 0;
//            try
//            {
//                connection.Open();
//                rowsAffected = command.ExecuteNonQuery();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Error" + ex.Message);
//            }
//            finally
//            {
//                connection.Close();
//            }
//            return (rowsAffected > 0);
//        }

//        public static bool GetApplicationInfoByID(int ApplicationID, ref int ApplicantPersonID, ref DateTime ApplicationDate, ref int ApplicationTypeID, ref byte ApplicationStatus, ref DateTime LastStatusDate, ref float PaidFees, ref int CreatedByUserID)
//        {
//            bool IsRecordFound = false;

//            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

//            string Query = @"SELECT * FROM Applications
//                             WHERE ApplicationID = @ApplicationID;";

//            SqlCommand command = new SqlCommand(Query, connection);

//            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

//            try
//            {
//                connection.Open();

//                SqlDataReader reader = command.ExecuteReader();

//                if (reader.Read())
//                {
//                    ApplicantPersonID = (int)reader["ApplicantPersonID"];
//                    ApplicationTypeID = (int)reader["ApplicationTypeID"];
//                    ApplicationDate = (DateTime)reader["ApplicationDate"];
//                    ApplicationStatus = (byte)reader["ApplicationStatus"];
//                    LastStatusDate = (DateTime)reader["LastStatusDate"];
//                    PaidFees = (float)(decimal)reader["PaidFees"];
//                    CreatedByUserID = (int)reader["CreatedByUserID"];

//                    IsRecordFound = true;
//                }
//                reader.Close();

//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Error" + ex.Message);
//            }
//            finally
//            {
//                connection.Close();
//            }
//            return IsRecordFound;
//        }
//    }
//}
//Sr------------------------------------------------
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static System.Net.Mime.MediaTypeNames;

//namespace DVLD_DataAccess
//{
//    public class clsApplicationData
//    {


//        public static bool GetApplicationInfoByID(int ApplicationID,
//            ref int ApplicantPersonID, ref DateTime ApplicationDate, ref int ApplicationTypeID,
//            ref byte ApplicationStatus, ref DateTime LastStatusDate,
//            ref float PaidFees, ref int CreatedByUserID)
//        {
//            bool isFound = false;

//            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

//            string query = "SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";

//            SqlCommand command = new SqlCommand(query, connection);

//            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

//            try
//            {
//                connection.Open();
//                SqlDataReader reader = command.ExecuteReader();

//                if (reader.Read())
//                {

//                    // The record was found
//                    isFound = true;

//                    ApplicantPersonID = (int)reader["ApplicantPersonID"];
//                    ApplicationDate = (DateTime)reader["ApplicationDate"];
//                    ApplicationTypeID = (int)reader["ApplicationTypeID"];
//                    ApplicationStatus = (byte)reader["ApplicationStatus"];
//                    LastStatusDate = (DateTime)reader["LastStatusDate"];
//                    PaidFees = Convert.ToSingle(reader["PaidFees"]);
//                    CreatedByUserID = (int)reader["CreatedByUserID"];


//                }
//                else
//                {
//                    // The record was not found
//                    isFound = false;
//                }

//                reader.Close();


//            }
//            catch (Exception ex)
//            {
//                //Console.WriteLine("Error: " + ex.Message);
//                isFound = false;
//            }
//            finally
//            {
//                connection.Close();
//            }

//            return isFound;
//        }

//        public static DataTable GetAllApplications()
//        {

//            DataTable dt = new DataTable();
//            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

//            string query = "select * from ApplicationsList_View order by ApplicationDate desc";

//            SqlCommand command = new SqlCommand(query, connection);

//            try
//            {
//                connection.Open();

//                SqlDataReader reader = command.ExecuteReader();

//                if (reader.HasRows)

//                {
//                    dt.Load(reader);
//                }

//                reader.Close();


//            }

//            catch (Exception ex)
//            {
//                // Console.WriteLine("Error: " + ex.Message);
//            }
//            finally
//            {
//                connection.Close();
//            }

//            return dt;

//        }

//        public static int AddNewApplication(int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
//             byte ApplicationStatus, DateTime LastStatusDate,
//             float PaidFees, int CreatedByUserID)
//        {

//            //this function will return the new person id if succeeded and -1 if not.
//            int ApplicationID = -1;

//            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

//            string query = @"INSERT INTO Applications ( 
//                            ApplicantPersonID,ApplicationDate,ApplicationTypeID,
//                            ApplicationStatus,LastStatusDate,
//                            PaidFees,CreatedByUserID)
//                             VALUES (@ApplicantPersonID,@ApplicationDate,@ApplicationTypeID,
//                                      @ApplicationStatus,@LastStatusDate,
//                                      @PaidFees,   @CreatedByUserID);
//                             SELECT SCOPE_IDENTITY();";

//            SqlCommand command = new SqlCommand(query, connection);

//            command.Parameters.AddWithValue("ApplicantPersonID", @ApplicantPersonID);
//            command.Parameters.AddWithValue("ApplicationDate", @ApplicationDate);
//            command.Parameters.AddWithValue("ApplicationTypeID", @ApplicationTypeID);
//            command.Parameters.AddWithValue("ApplicationStatus", @ApplicationStatus);
//            command.Parameters.AddWithValue("LastStatusDate", @LastStatusDate);
//            command.Parameters.AddWithValue("PaidFees", @PaidFees);
//            command.Parameters.AddWithValue("CreatedByUserID", @CreatedByUserID);




//            try
//            {
//                connection.Open();

//                object result = command.ExecuteScalar();

//                if (result != null && int.TryParse(result.ToString(), out int insertedID))
//                {
//                    ApplicationID = insertedID;
//                }
//            }

//            catch (Exception ex)
//            {
//                //Console.WriteLine("Error: " + ex.Message);

//            }

//            finally
//            {
//                connection.Close();
//            }


//            return ApplicationID;
//        }


//        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
//             byte ApplicationStatus, DateTime LastStatusDate,
//             float PaidFees, int CreatedByUserID)
//        {

//            int rowsAffected = 0;
//            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

//            string query = @"Update  Applications  
//                            set ApplicantPersonID = @ApplicantPersonID,
//                                ApplicationDate = @ApplicationDate,
//                                ApplicationTypeID = @ApplicationTypeID,
//                                ApplicationStatus = @ApplicationStatus, 
//                                LastStatusDate = @LastStatusDate,
//                                PaidFees = @PaidFees,
//                                CreatedByUserID=@CreatedByUserID
//                            where ApplicationID=@ApplicationID";

//            SqlCommand command = new SqlCommand(query, connection);

//            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
//            command.Parameters.AddWithValue("ApplicantPersonID", @ApplicantPersonID);
//            command.Parameters.AddWithValue("ApplicationDate", @ApplicationDate);
//            command.Parameters.AddWithValue("ApplicationTypeID", @ApplicationTypeID);
//            command.Parameters.AddWithValue("ApplicationStatus", @ApplicationStatus);
//            command.Parameters.AddWithValue("LastStatusDate", @LastStatusDate);
//            command.Parameters.AddWithValue("PaidFees", @PaidFees);
//            command.Parameters.AddWithValue("CreatedByUserID", @CreatedByUserID);


//            try
//            {
//                connection.Open();
//                rowsAffected = command.ExecuteNonQuery();

//            }
//            catch (Exception ex)
//            {
//                //Console.WriteLine("Error: " + ex.Message);
//                return false;
//            }

//            finally
//            {
//                connection.Close();
//            }

//            return (rowsAffected > 0);
//        }

//        public static bool DeleteApplication(int ApplicationID)
//        {

//            int rowsAffected = 0;

//            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

//            string query = @"Delete Applications 
//                                where ApplicationID = @ApplicationID";

//            SqlCommand command = new SqlCommand(query, connection);

//            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

//            try
//            {
//                connection.Open();

//                rowsAffected = command.ExecuteNonQuery();

//            }
//            catch (Exception ex)
//            {
//                // Console.WriteLine("Error: " + ex.Message);
//            }
//            finally
//            {

//                connection.Close();

//            }

//            return (rowsAffected > 0);

//        }

//        public static bool IsApplicationExist(int ApplicationID)
//        {
//            bool isFound = false;

//            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

//            string query = "SELECT Found=1 FROM Applications WHERE ApplicationID = @ApplicationID";

//            SqlCommand command = new SqlCommand(query, connection);

//            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

//            try
//            {
//                connection.Open();
//                SqlDataReader reader = command.ExecuteReader();

//                isFound = reader.HasRows;

//                reader.Close();
//            }
//            catch (Exception ex)
//            {
//                //Console.WriteLine("Error: " + ex.Message);
//                isFound = false;
//            }
//            finally
//            {
//                connection.Close();
//            }

//            return isFound;
//        }

//        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
//        {

//            //incase the ActiveApplication ID !=-1 return true.
//            return (GetActiveApplicationID(PersonID, ApplicationTypeID) != -1);
//        }

//        public static int GetActiveApplicationID(int PersonID, int ApplicationTypeID)
//        {
//            int ActiveApplicationID = -1;

//            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

//            string query = "SELECT ActiveApplicationID=ApplicationID FROM Applications WHERE ApplicantPersonID = @ApplicantPersonID and ApplicationTypeID=@ApplicationTypeID and ApplicationStatus=1";

//            SqlCommand command = new SqlCommand(query, connection);

//            command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
//            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

//            try
//            {
//                connection.Open();
//                object result = command.ExecuteScalar();


//                if (result != null && int.TryParse(result.ToString(), out int AppID))
//                {
//                    ActiveApplicationID = AppID;
//                }
//            }
//            catch (Exception ex)
//            {
//                //Console.WriteLine("Error: " + ex.Message);
//                return ActiveApplicationID;
//            }
//            finally
//            {
//                connection.Close();
//            }

//            return ActiveApplicationID;
//        }

//        public static int GetActiveApplicationIDForLicenseClass(int PersonID, int ApplicationTypeID, int LicenseClassID)
//        {
//            int ActiveApplicationID = -1;

//            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

//            string query = @"SELECT ActiveApplicationID=Applications.ApplicationID  
//                            From
//                            Applications INNER JOIN
//                            LocalDrivingLicenseApplications ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
//                            WHERE ApplicantPersonID = @ApplicantPersonID 
//                            and ApplicationTypeID=@ApplicationTypeID 
//							and LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
//                            and ApplicationStatus=1";

//            SqlCommand command = new SqlCommand(query, connection);

//            command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
//            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
//            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
//            try
//            {
//                connection.Open();
//                object result = command.ExecuteScalar();


//                if (result != null && int.TryParse(result.ToString(), out int AppID))
//                {
//                    ActiveApplicationID = AppID;
//                }
//            }
//            catch (Exception ex)
//            {
//                //Console.WriteLine("Error: " + ex.Message);
//                return ActiveApplicationID;
//            }
//            finally
//            {
//                connection.Close();
//            }

//            return ActiveApplicationID;
//        }

//        public static bool UpdateStatus(int ApplicationID, short NewStatus)
//        {

//            int rowsAffected = 0;
//            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

//            string query = @"Update  Applications  
//                            set 
//                                ApplicationStatus = @NewStatus, 
//                                LastStatusDate = @LastStatusDate
//                            where ApplicationID=@ApplicationID;";

//            SqlCommand command = new SqlCommand(query, connection);

//            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
//            command.Parameters.AddWithValue("@NewStatus", NewStatus);
//            command.Parameters.AddWithValue("LastStatusDate", DateTime.Now);


//            try
//            {
//                connection.Open();
//                rowsAffected = command.ExecuteNonQuery();

//            }
//            catch (Exception ex)
//            {
//                //Console.WriteLine("Error: " + ex.Message);
//                return false;
//            }

//            finally
//            {
//                connection.Close();
//            }

//            return (rowsAffected > 0);
//        }

//    }
//}
//---------------------------------------------------------------------------------------------------------
//using (var reader = command.ExecuteReader())
//{
//    int ProductNameIndex = reader.GetOrdinal("ProductName");
//    int ProductDescriptionIndex = reader.GetOrdinal("ProductDescription");
//    int QuantityIndex = reader.GetOrdinal("Quantity");
//    int PriceIndex = reader.GetOrdinal("Price");
//    int ImageURLIndex = reader.GetOrdinal("ImageURL");
//    int CaloriesIndex = reader.GetOrdinal("Calories");
//    int NotesIndex = reader.GetOrdinal("Notes");

//    if (reader.Read())
//    {
//        string? Notes = !reader.IsDBNull(NotesIndex) ? reader.GetString(NotesIndex) : null;
//        CartItemsList.Add(new clsCartDTO
//        (
//            reader.GetInt32(ProductNameIndex),
//            reader.GetString(ProductDescriptionIndex),
//            reader.GetByte(QuantityIndex),
//            reader.GetDecimal(PriceIndex),
//            reader.GetString(ImageURLIndex),
//            reader.GetInt32(CaloriesIndex),
//            Notes
//        ));
//    }
//    else
//    {
//        return null;
//    }
//}