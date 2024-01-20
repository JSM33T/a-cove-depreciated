using almondcove.Models.Domain;
using Microsoft.Data.SqlClient;

namespace almondcove.Interefaces.Repositories
{
    public interface IAuthRepository
    {
        public Task<bool> DisposeSessionKey(string Username);
        public Task<UserProfile> LogIn();
        public Task<bool> SaveOTPInDatabaseAsync(SqlConnection connection, int userId, string otp);
        public Task<UserProfile> GetUserByUsernameOrEmailAsync(SqlConnection connection, string usernameOrEmail);
    }
}
