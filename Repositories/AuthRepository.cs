using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using almondcove.Models.Domain;
using Microsoft.Data.SqlClient;
using System.Data;

namespace almondcove.Repositories
{
    public class AuthRepository(IConfigManager configManager,ILogger<AuthRepository> logger) : IAuthRepository
    {
        private readonly IConfigManager _configManager = configManager;
        private readonly ILogger<AuthRepository> _logger = logger;
        public Task<bool> DisposeSessionKey(string Username)
        {
            SqlConnection conn = new();
            SqlCommand cmd = new("UPDATE TblUserProfile SET SessionKey = '' WHERE UserName = @username", conn);
            cmd.Parameters.AddWithValue("@username", Username);
            try { 
                    cmd.ExecuteNonQuery();
                    return Task.FromResult(true);
                }
            catch {
                return Task.FromResult(true);
            }
           
        }

        public async Task<bool> SaveOTPInDatabaseAsync(SqlConnection connection, int userId, string otp)
        {
            try
            {
                var newId = await GetNextIdAsync(connection, "TblPasswordReset");

                // Insert the new record
                var sql = @"
                        INSERT INTO TblPasswordReset (Id, UserId, Token, DateAdded, IsValid)
                        VALUES (@id, @userId, @token, @dateAdded, @isValid)";

                using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", newId);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@token", otp);
                cmd.Parameters.AddWithValue("@isValid", true);
                cmd.Parameters.AddWithValue("@dateAdded", DateTime.Now);

                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error saving OTP in the database: {message}", ex.Message);
                return false;
            }
        }

        private static async Task<int> GetNextIdAsync(SqlConnection connection, string tableName)
        {
            var sql = $"SELECT ISNULL(MAX(Id), 0) + 1 FROM {tableName}";

            using var cmd = new SqlCommand(sql, connection);
            return await cmd.ExecuteScalarAsync() as int? ?? 1;
        }

        public async Task<UserProfile> GetUserByUsernameOrEmailAsync(SqlConnection connection, string usernameOrEmail)
        {
            var sql = "SELECT * FROM TblUserProfile WHERE UserName = @username OR EMail = @username";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@username", usernameOrEmail);
            using var reader = await command.ExecuteReaderAsync();

            return await Task.FromResult(
                    reader.Read()
                    ? new UserProfile
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        UserName = reader.GetString(reader.GetOrdinal("UserName")),
                        EMail = reader.GetString(reader.GetOrdinal("EMail"))
                    }
                    : null
                );
        }


        public Task<UserProfile> LogIn()
        {
            throw new NotImplementedException();
        }
    }
}
