using almondcove.Interefaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace almondcove.Controllers.Api
{
    [ApiController]
    public class SearchApiController(ISearchRepository searchRepository) : ControllerBase
    {
        private readonly ISearchRepository _searchRepo = searchRepository;

        [HttpGet("/api/liversearch/all/{Slug}")]
        public async Task<IActionResult> LiveSearch(string Slug) => Ok(await _searchRepo.GetSearchResultsBySlug(Slug));

    }
}
