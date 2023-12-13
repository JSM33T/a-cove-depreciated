using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using almondcove.Models.Domain;
using Microsoft.Data.SqlClient;

namespace almondcove.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly IConfigManager _configManager;

        public ProfileRepository(IConfigManager configManager)
        {
            _configManager = configManager;
        }
        public async Task<UserProfile> GetProfileByUsername(string username)
        {
            try
            {
                using (var connection = new SqlConnection(_configManager.GetConnString()))
                {
                    await connection.OpenAsync();

                    using var command = new SqlCommand(@"
                            SELECT a.FirstName, a.LastName, a.UserName, a.Role, a.Gender, a.Bio, a.DateJoined, a.EMail, b.Image 
                            FROM TblUserProfile a
                            JOIN TblAvatarMaster b ON a.AvatarId = b.Id
                            WHERE a.UserName = @username
                        ", connection);
                    command.Parameters.AddWithValue("@username", username);

                    using var reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        UserProfile userProfile = new UserProfile
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
                        };

                        return userProfile;
                    }
                    else
                    {
                        // User not found, handle accordingly
                        throw new Exception("User not found");
                    }
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
    }
}
