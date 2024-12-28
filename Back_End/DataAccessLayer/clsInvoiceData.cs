using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagnostic_Center_Management_System_DataAccessLayer
{

    public class InvoiceDTO
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int TestId { get; set; }

        public InvoiceDTO(int Id, int PatientId, int DoctorId, DateTime DeliveryDate, int TestId)
        {
            this.Id = Id;
            this.PatientId = PatientId;
            this.DoctorId = DoctorId;
            this.DeliveryDate = DeliveryDate;
            this.TestId = TestId;
        }

    }

    public class DetailedInvoiceDTO
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public PatientDTO PatientInfo { get; set; }
        public int DoctorId { get; set; }
        public DoctorDTO DoctorInfo { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int TestId { get; set; }
        public TestDTO TestInfo { get; set; }

        public DetailedInvoiceDTO(int Id, int PatientId, int DoctorId, DateTime DeliveryDate, int TestId)
        {
            this.Id = Id;
            this.PatientId = PatientId;
            this.PatientInfo = clsPatientData.GetPatientById(PatientId);
            this.DoctorId = DoctorId;
            this.DoctorInfo = clsDoctorData.GetDoctorById(DoctorId);
            this.DeliveryDate = DeliveryDate;
            this.TestId = TestId;
            this.TestInfo = clsTestData.GetTestById(TestId);
        }

    }

    public class clsInvoiceData
    {
        public static List<DetailedInvoiceDTO> GetAllInvoices()
        {
            var InvoicesList = new List<DetailedInvoiceDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllInvoices", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                InvoicesList.Add(new DetailedInvoiceDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetInt32(reader.GetOrdinal("PatientId")),
                                    reader.GetInt32(reader.GetOrdinal("DoctorId")),
                                    reader.GetDateTime(reader.GetOrdinal("DeliveryDate")),
                                    reader.GetInt32(reader.GetOrdinal("TestId"))
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

            return InvoicesList;
        }

        public static int GetIncome()
        {
            int Income = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetIncome", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            int.TryParse(result.ToString(), out Income);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return Income;
        }

        public static DetailedInvoiceDTO GetInvoiceById(int InvoiceID)
        {
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_GetInvoiceById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", InvoiceID);


                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new DetailedInvoiceDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetInt32(reader.GetOrdinal("PatientId")),
                                    reader.GetInt32(reader.GetOrdinal("DoctorId")),
                                    reader.GetDateTime(reader.GetOrdinal("DeliveryDate")),
                                    reader.GetInt32(reader.GetOrdinal("TestId"))
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

        public static int AddInvoice(InvoiceDTO InvoiceDTO)
        {
            int InvoiceId = -1;

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_AddNewInvoice", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PatientId", InvoiceDTO.PatientId);
                        command.Parameters.AddWithValue("@DoctorId", InvoiceDTO.DoctorId);
                        command.Parameters.AddWithValue("@DeliveryDate", InvoiceDTO.DeliveryDate);
                        command.Parameters.AddWithValue("@TestId", InvoiceDTO.TestId);

                        var outputIdParam = new SqlParameter("@NewInvoiceId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(outputIdParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        InvoiceId = (int)outputIdParam.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return InvoiceId;
        }

        public static bool UpdateInvoice(InvoiceDTO InvoiceDTO)
        {
            int rowsAffected = 0;

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_UpdateInvoice", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Id", InvoiceDTO.Id);
                        command.Parameters.AddWithValue("@PatientId", InvoiceDTO.PatientId);
                        command.Parameters.AddWithValue("@DoctorId", InvoiceDTO.DoctorId);
                        command.Parameters.AddWithValue("@DeliveryDate", InvoiceDTO.DeliveryDate);
                        command.Parameters.AddWithValue("@TestId", InvoiceDTO.TestId);

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

        public static bool DeleteInvoice(int InvoiceID)
        {
            int rowsAffected = 0;

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_DeleteInvoice", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Id", InvoiceID);

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
