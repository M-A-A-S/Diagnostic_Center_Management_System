using Microsoft.Data.SqlClient;
using System.Data;

namespace Diagnostic_Center_Management_System_DataAccessLayer
{
    public class SpecialtyDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public SpecialtyDTO(int Id, string Title)
        {
            this.Id = Id;
            this.Title = Title;
        }

    }

    public class clsSpecialtyData
    {
        public static List<SpecialtyDTO> GetAllSpecialties()
        {
            var SpecialtiesList = new List<SpecialtyDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllSpecialties", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SpecialtiesList.Add(new SpecialtyDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Title"))
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

            return SpecialtiesList;
        }

        public static SpecialtyDTO GetSpecialtyById(int SpecialtyID)
        {
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_GetSpecialtyById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", SpecialtyID);


                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new SpecialtyDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Title"))
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

        public static int AddSpecialty(SpecialtyDTO SpecialtyDTO)
        {
            int SpecialtyId = -1;

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_AddNewSpecialty", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Title", SpecialtyDTO.Title);

                        var outputIdParam = new SqlParameter("@NewSpecialtyId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(outputIdParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        SpecialtyId = (int)outputIdParam.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return SpecialtyId;
        }

        public static bool UpdateSpecialty(SpecialtyDTO SpecialtyDTO)
        {
            int rowsAffected = 0;

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_UpdateSpecialty", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Id", SpecialtyDTO.Id);
                        command.Parameters.AddWithValue("@Title", SpecialtyDTO.Title);

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

        public static bool DeleteSpecialty(int SpecialtyID)
        {
            int rowsAffected = 0;

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_DeleteSpecialty", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Id", SpecialtyID);

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
