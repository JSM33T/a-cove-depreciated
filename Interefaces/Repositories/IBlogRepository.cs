using almondcove.Models.DTO.BlogDTOs;

namespace almondcove.Interefaces.Repositories
{
    public interface IBlogRepository
    {
        Task<List<BlogThumbsDTO>> GetTopBlogsAsync();
    }
}
