using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagnostic_Center_Management_System_DataAccessLayer
{
    public class PatientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public bool Sex { get; set; } // 0 - Male, 1 - Female 

        public PatientDTO(int Id, string Name, DateTime DateOfBirth, string Phone, bool Sex)
        {
            this.Id = Id;
            this.Name = Name;
            this.DateOfBirth = DateOfBirth;
            this.Phone = Phone;
            this.Sex = Sex;
        }

    }

    public class clsPatientData
    {
        public static List<PatientDTO> GetAllPatients()
        {
            var PatientsList = new List<PatientDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllPatients", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PatientsList.Add(new PatientDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                    reader.GetString(reader.GetOrdinal("Phone")),
                                    reader.GetBoolean(reader.GetOrdinal("Sex"))
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return PatientsList;
        }

        public static int GetNumberOfPatients()
        {
            int numberOfPatients = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetNumberOfPatients", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            int.TryParse(result.ToString(), out numberOfPatients);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return numberOfPatients;
        }

        public static PatientDTO GetPatientById(int PatientID)
        {
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_GetPatientById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", PatientID);


                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new PatientDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                    reader.GetString(reader.GetOrdinal("Phone")),
                                    reader.GetBoolean(reader.GetOrdinal("Sex"))
                                );
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return null;
        }

        public static int AddPatient(PatientDTO PatientDTO)
        {
            int PatientId = -1;

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_AddNewPatient", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Name", PatientDTO.Name);
                        command.Parameters.AddWithValue("@DateOfBirth", PatientDTO.DateOfBirth);
                        command.Parameters.AddWithValue("@Phone", PatientDTO.Phone);
                        command.Parameters.AddWithValue("@Sex", PatientDTO.Sex);

                        var outputIdParam = new SqlParameter("@NewPatientId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(outputIdParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        PatientId = (int)outputIdParam.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return PatientId;
        }

        public static bool UpdatePatient(PatientDTO PatientDTO)
        {
            int rowsAffected = 0;

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_UpdatePatient", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Id", PatientDTO.Id);
                        command.Parameters.AddWithValue("@Name", PatientDTO.Name);
                        command.Parameters.AddWithValue("@DateOfBirth", PatientDTO.DateOfBirth);
                        command.Parameters.AddWithValue("@Phone", PatientDTO.Phone);
                        command.Parameters.AddWithValue("@Sex", PatientDTO.Sex);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return (rowsAffected > 0);
        }

        public static bool DeletePatient(int PatientID)
        {
            int rowsAffected = 0;

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_DeletePatient", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Id", PatientID);

                        connection.Open();

                        rowsAffected = (int)command.ExecuteNonQuery();

                        return (rowsAffected == 1);
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return (rowsAffected > 0);
        }
    }
}
