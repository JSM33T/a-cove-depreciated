using almondcove.Models.Domain;
using Microsoft.Data.SqlClient;

namespace almondcove.Interefaces.Repositories
{
    public interface IAuthRepository
    {
        public Task<bool> DisposeSessionKey(string Username);
        public Task<UserProfile> LogIn();
        public bool SaveOTPInDatabaseAsync(SqlConnection connection, int userId, string otp);
        public Task<UserProfile> GetUserByUsernameOrEmail(SqlConnection connection, string usernameOrEmail);
    }
}
