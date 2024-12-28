using Diagnostic_Center_Management_System_DataAccessLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class RefreshTokenDTO
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; }
        public bool IsUsed { get; set; }

        public RefreshTokenDTO(string Token, int UserId, DateTime Expires, bool IsRevoked, bool IsUsed) 
        { 
            this.Token = Token;
            this.UserId = UserId;
            this.Expires = Expires;
            this.IsRevoked = IsRevoked;
            this.IsUsed = IsUsed;
        }
    }

    public class clsRefreshTokenData
    {
        public static void AddNewRefreshToken(RefreshTokenDTO RefreshTokenDTO)
        {
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_AddNewRefreshToken", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Token", RefreshTokenDTO.Token);
                        command.Parameters.AddWithValue("@UserId", RefreshTokenDTO.UserId);
                        command.Parameters.AddWithValue("@Expires", RefreshTokenDTO.Expires);
                        command.Parameters.AddWithValue("@IsRevoked", RefreshTokenDTO.IsRevoked);
                        command.Parameters.AddWithValue("@IsUsed", RefreshTokenDTO.IsUsed);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }
        }

        public static void UpdateRefreshToken(RefreshTokenDTO RefreshTokenDTO)
        {
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_UpdateRefreshToken", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Token", RefreshTokenDTO.Token);
                        command.Parameters.AddWithValue("@UserId", RefreshTokenDTO.UserId);
                        command.Parameters.AddWithValue("@Expires", RefreshTokenDTO.Expires);
                        command.Parameters.AddWithValue("@IsRevoked", RefreshTokenDTO.IsRevoked);
                        command.Parameters.AddWithValue("@IsUsed", RefreshTokenDTO.IsUsed);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }
        }

        public static bool IsRefreshTokenForUser(int userId)
        {
            //SP_CheckRefreshTokenExistsForUser
            bool isExists = false;

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_CheckRefreshTokenExistsForUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", userId);
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            isExists = reader.HasRows;
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return isExists;
        }

        public static RefreshTokenDTO GetRefreshToken(string token)
        {
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_GetRefreshTokenByToken", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Token", token);
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new RefreshTokenDTO
                                (
                                    reader.GetString(reader.GetOrdinal("@Token")),
                                    reader.GetInt32(reader.GetOrdinal("@UserId")),
                                    reader.GetDateTime(reader.GetOrdinal("@Expires")),
                                    reader.GetBoolean(reader.GetOrdinal("@IsRevoked")),
                                    reader.GetBoolean(reader.GetOrdinal("@IsUsed"))
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
    }
}
