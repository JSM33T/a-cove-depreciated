using almondcove.Interefaces.Repositories;

namespace almondcove.Repositories
{
    public class ContactRepository : IContactRepository
    {
        public Task<(bool Success, string Message)> ContactWithDetails()
        {
            throw new NotImplementedException();
        }

        public Task<(bool Success, string Message)> SubmitMessage()
        {
            throw new NotImplementedException();
        }
    }
}
