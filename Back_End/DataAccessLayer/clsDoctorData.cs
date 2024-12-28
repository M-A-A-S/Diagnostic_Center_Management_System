using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagnostic_Center_Management_System_DataAccessLayer
{
    public class DoctorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int SpecialityId { get; set; }
        public DateTime JoinDate { get; set; }

        public DoctorDTO(int Id, string Name, DateTime DateOfBirth, string Phone, string Address, int SpecialityId, DateTime JoinDate)
        {
            this.Id = Id;
            this.Name = Name;
            this.DateOfBirth = DateOfBirth;
            this.Phone = Phone;
            this.Address = Address;
            this.SpecialityId = SpecialityId;
            this.JoinDate = JoinDate;
        }

    }

    public class clsDoctorData
    {
        public static List<DoctorDTO> GetAllDoctors()
        {
            var DoctorsList = new List<DoctorDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllDoctors", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DoctorsList.Add(new DoctorDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                    reader.GetString(reader.GetOrdinal("Phone")),
                                    reader.GetString(reader.GetOrdinal("Address")),
                                    reader.GetInt32(reader.GetOrdinal("SpecialityId")),
                                    reader.GetDateTime(reader.GetOrdinal("JoinDate"))
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

            return DoctorsList;
        }

        public static int GetNumberOfDoctors()
        {
            int numberOfDoctors = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetNumberOfDoctors", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            int.TryParse(result.ToString(), out numberOfDoctors);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return numberOfDoctors;
        }

        public static DoctorDTO GetDoctorById(int DoctorID)
        {
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_GetDoctorById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", DoctorID);


                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new DoctorDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                    reader.GetString(reader.GetOrdinal("Phone")),
                                    reader.GetString(reader.GetOrdinal("Address")),
                                    reader.GetInt32(reader.GetOrdinal("SpecialityId")),
                                    reader.GetDateTime(reader.GetOrdinal("JoinDate"))
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

        public static int AddDoctor(DoctorDTO DoctorDTO)
        {
            int DoctorId = -1;

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_AddNewDoctor", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Name", DoctorDTO.Name);
                        command.Parameters.AddWithValue("@DateOfBirth", DoctorDTO.DateOfBirth);
                        command.Parameters.AddWithValue("@Phone", DoctorDTO.Phone);
                        command.Parameters.AddWithValue("@Address", DoctorDTO.Address);
                        command.Parameters.AddWithValue("@SpecialityId", DoctorDTO.SpecialityId);
                        command.Parameters.AddWithValue("@JoinDate", DoctorDTO.JoinDate);

                        var outputIdParam = new SqlParameter("@NewDoctorId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(outputIdParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        DoctorId = (int)outputIdParam.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return DoctorId;
        }

        public static bool UpdateDoctor(DoctorDTO DoctorDTO)
        {
            int rowsAffected = 0;

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_UpdateDoctor", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Id", DoctorDTO.Id);
                        command.Parameters.AddWithValue("@Name", DoctorDTO.Name);
                        command.Parameters.AddWithValue("@DateOfBirth", DoctorDTO.DateOfBirth);
                        command.Parameters.AddWithValue("@Phone", DoctorDTO.Phone);
                        command.Parameters.AddWithValue("@Address", DoctorDTO.Address);
                        command.Parameters.AddWithValue("@SpecialityId", DoctorDTO.SpecialityId);
                        command.Parameters.AddWithValue("@JoinDate", DoctorDTO.JoinDate);

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

        public static bool DeleteDoctor(int DoctorID)
        {
            int rowsAffected = 0;

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_DeleteDoctor", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Id", DoctorID);

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
