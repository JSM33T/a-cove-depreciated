using almondCove.Models.Domain;

namespace almondCove.Interefaces.Repositories
{
    public interface IUserRepository
    {
        Task<bool> IsPresent(UserProfile UserProfile);

    }
}
