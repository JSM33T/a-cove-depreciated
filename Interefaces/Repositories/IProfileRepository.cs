using almondcove.Models.Domain;

namespace almondcove.Interefaces.Repositories
{
    public interface IProfileRepository
    {
        //profiles
        public Task<UserProfile> GetProfileByUsername(string Username);
        public Task<UserProfile> GetUserBySessionKeyAsync(string sessionKey);

        //get avatars
        public Task<List<Avatar>> GetAvatarsAsync();
        public Task<Avatar> GetAvatarByIdAsync(int Id);
        public Task<bool> UpdatePassword(string username, string newPassword);

    }
}
