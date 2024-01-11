using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using almondcove.Models.Domain;
using almondcove.Modules;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace almondcove.Repositories
{
    public class ProfileRepository(IConfigManager configManager, ILogger<ProfileRepository> logger) : IProfileRepository
    {
        private readonly IConfigManager _configManager = configManager;
        private readonly ILogger<ProfileRepository> _logger = logger;

        public async Task<UserProfile> GetProfileByUsername(string username)
        {
            try
            {
                using var connection = new SqlConnection(_configManager.GetConnString());
                await connection.OpenAsync();

                using var command = new SqlCommand(@"
                            SELECT a.FirstName, a.LastName, a.UserName, a.Role, a.Gender, a.Bio, a.DateJoined, a.EMail, b.Image ,b.Id
                            FROM TblUserProfile a
                            JOIN TblAvatarMaster b ON a.AvatarId = b.Id
                            WHERE a.UserName = @username
                        ", connection);
                command.Parameters.AddWithValue("@username", username);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    UserProfile userProfile = new()
                    {
                        FirstName = reader.IsDBNull(0) ? null : reader.GetString(0),
                        LastName = reader.IsDBNull(1) ? null : reader.GetString(1),
                        UserName = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Role = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Gender = reader.IsDBNull(4) ? null : reader.GetString(4),
                        Bio = reader.IsDBNull(5) ? null : reader.GetString(5),
                        DateElement = reader.IsDBNull(6) ? null : reader.GetDateTime(6).ToString("yyyy-MM-dd"),
                        EMail = reader.IsDBNull(7) ? null : reader.GetString(7),
                        AvatarImg = reader.IsDBNull(8) ? null : reader.GetString(8),
                        AvatarId = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                    };

                    return userProfile;
                }
                else
                {
                    _logger.LogError("error getting profile by username");
                    throw new Exception("User not found");
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserProfile> GetUserBySessionKeyAsync(string sessionKey)
        {
            using SqlConnection connection = new(_configManager.GetConnString());
            await connection.OpenAsync();

            SqlCommand checkCommand = new(
                @"select p.*,a.Image 
                from TblUserProfile p,TblAvatarMaster a 
                WHERE SessionKey = @sessionkey
                and p.IsActive= 1
                and p.IsVerified = 1
                and p.AvatarId = a.Id ",
                connection
            );

            checkCommand.Parameters.AddWithValue("@sessionkey", sessionKey);

            using var reader = await checkCommand.ExecuteReaderAsync();

            if (reader.Read())
            {
                // Map reader data to a User object
                var user = new UserProfile
                {
                    UserName = reader.GetString(reader.GetOrdinal("UserName")),
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    Role = reader.GetString(reader.GetOrdinal("Role")),
                    AvatarImg = reader.GetString(reader.GetOrdinal("Image"))
                };

                return user;
            }

            return null;
        }

        public async Task<List<Avatar>> GetAvatarsAsync()
        {
            try
            {
                using SqlConnection connection = new(_configManager.GetConnString());
                await connection.OpenAsync();

                string sql = "SELECT Id, Title, Image FROM TblAvatarMaster";

                using SqlCommand command = new(sql, connection);
                using SqlDataReader dataReader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                List<Avatar> entries = [];

                while (await dataReader.ReadAsync())
                {
                    Avatar entry = new()
                    {
                        Id = dataReader["Id"] as int? ?? 0,
                        Title = dataReader["Title"] as string ?? "",
                        Image = dataReader["Image"] as string ?? ""
                    };
                    entries.Add(entry);
                }

                return entries;
            }
            catch (SqlException ex)
            {
                _logger.LogError("error getting avatar{message}", ex.Message);
                throw;
            }
        }

        public async Task<bool> UpdatePassword(string username, string newPassword)
        {
            try
            {
                using SqlConnection connection = new(_configManager.GetConnString());
                await connection.OpenAsync();

                SqlCommand updateCommand = new(@"
                UPDATE TblUserProfile SET CryptedPassword = @cryptedpassword, DateUpdated = @dateupdated 
                WHERE UserName = @username", connection);

                updateCommand.Parameters.AddWithValue("@username", username);
                updateCommand.Parameters.AddWithValue("@cryptedpassword", EnDcryptor.Encrypt(newPassword, _configManager.GetCryptKey()));
                updateCommand.Parameters.Add("@dateupdated", SqlDbType.DateTime).Value = DateTime.Now;

                await updateCommand.ExecuteNonQueryAsync();
                await connection.CloseAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("error updating password{message}", ex.Message);
                return false;
            }
        }

        public async Task<Avatar> GetAvatarByIdAsync(int Id)
        {
            using var connection = new SqlConnection(_configManager.GetConnString());
            await connection.OpenAsync().ConfigureAwait(false);

            var sql = "SELECT * FROM TblAvatarMaster WHERE Id = @avtrid";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@avtrid", Id);
            try
            {
                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    Avatar _avatar = new()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Image = reader.GetString(reader.GetOrdinal("Image")),
                        Description = reader.GetString(reader.GetOrdinal("Description"))
                    };

                    return _avatar;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
