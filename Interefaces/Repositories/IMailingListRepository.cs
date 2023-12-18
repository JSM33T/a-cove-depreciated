
using almondcove.Models.Domain;

namespace almondcove.Interefaces.Repositories
{
    public interface IMailingListRepository
    {
        public Task<(bool Success, string Message)> PostMail(Mail mail);
    }
}
    