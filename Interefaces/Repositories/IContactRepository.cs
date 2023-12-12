namespace almondcove.Interefaces.Repositories
{
    public interface IContactRepository
    {
        public Task<(bool Success, string Message)> SubmitMessage();
        public Task<(bool Success, string Message)> ContactWithDetails();
    }
}
