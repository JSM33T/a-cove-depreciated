using almondcove.Models.Domain;

namespace almondcove.Interefaces.Repositories
{
    public interface IProfileRepository
    {
        public Task<UserProfile> GetProfileByUsername(string Username);
        public Task<UserProfile> GetUserBySessionKeyAsync(string sessionKey);
    }
}
