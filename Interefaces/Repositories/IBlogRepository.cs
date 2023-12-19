using almondcove.Models.DTO;

namespace almondcove.Interefaces.Repositories
{
    public interface IBlogRepository
    {
        Task<List<BlogThumbsDTO>> GetTopBlogsAsync();
    }
}
