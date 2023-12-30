using almondcove.Interefaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace almondcove.Controllers.Api
{
    [ApiController]
    public class SearchApiController(ISearchRepository searchRepository, ILogger<SearchApiController> logger) : ControllerBase
    {
        private readonly ILogger<SearchApiController> _logger = logger;
        private readonly ISearchRepository _searchRepo = searchRepository;

        [HttpGet]
        [Route("/api/liversearch/all/{Slug}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> LiveSearch(string Slug)
        {
            try
            {
                return Ok(await _searchRepo.GetSearchResultsBySlug(Slug));
            }
            catch (Exception ex)
            {
                _logger.LogError("error fetching search results for:{Slug} ,message:{message}", Slug,ex.Message);
                return BadRequest("unable to fetch search results");
            }
        }
    }
}
