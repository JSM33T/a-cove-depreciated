using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using almondcove.Models.DTO;
using Microsoft.Data.SqlClient;

namespace almondcove.Repositories
{
    public class SearchRepository(IConfigManager configManager) : ISearchRepository
    {
        private readonly IConfigManager _configManager = configManager;

        public async Task<LiveSearchDTO> GetSearchResultsBySlug(string Slug)
        {
            if (Slug.Length <= 1) return null;

            using SqlConnection conn = new(_configManager.GetConnString());
            await conn.OpenAsync();

            string SearchQuery = "SELECT * FROM TblSearchMaster WHERE Slug = @Slug";
            using SqlCommand searchCommand = new(SearchQuery, conn);
            searchCommand.Parameters.AddWithValue("@Slug", Slug);

            using SqlDataReader reader = await searchCommand.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                LiveSearchDTO searchResult = new()
                {
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"].ToString()
                };

                return searchResult;
            }
            else
            {
                return null;
            }
        }
    }
}
