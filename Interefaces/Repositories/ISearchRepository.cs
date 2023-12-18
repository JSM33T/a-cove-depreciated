using almondcove.Models.DTO;

namespace almondcove.Interefaces.Repositories
{
    public interface ISearchRepository
    {
        public Task<LiveSearchDTO> GetSearchResultsBySlug(string Slug);
    }
}
