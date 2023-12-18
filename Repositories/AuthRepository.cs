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
            return Task.FromResult(true);
        }

        public bool SaveOTPInDatabaseAsync(SqlConnection connection, int userId, string otp)
        {
            try
            {
                var maxIdCommand = new SqlCommand("SELECT ISNULL(MAX(Id), 0) + 1 FROM TblPasswordReset", connection);
                int newId = Convert.ToInt32(maxIdCommand.ExecuteScalar());

                var cmd = new SqlCommand("INSERT INTO TblPasswordReset (Id, UserId, Token, DateAdded, IsValid) VALUES (@id, @userid, @token, @dateadded, @isvalid)", connection);
                cmd.Parameters.AddWithValue("@id", newId);
                cmd.Parameters.AddWithValue("@userid", userId);
                cmd.Parameters.AddWithValue("@token", otp);
                cmd.Parameters.AddWithValue("@isvalid", true);
                cmd.Parameters.Add("@dateadded", SqlDbType.DateTime).Value = DateTime.Now;

                cmd.ExecuteNonQuery();

                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError("error sending email {message}", ex.Message);
                return false;
            }
        }

        public Task<UserProfile> GetUserByUsernameOrEmail(SqlConnection connection, string usernameOrEmail)
        {
            var sql = "SELECT * FROM TblUserProfile WHERE UserName = @username OR EMail = @username";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@username", usernameOrEmail);
            using var reader = command.ExecuteReader();

            return Task.FromResult(
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
