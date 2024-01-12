using almondcove.Filters;
using almondcove.Models.Domain;
using almondcove.Models.DTO.BlogDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace almondcove.Controllers.Api
{
    
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        [IgnoreAntiforgeryToken]
        [HttpGet("/api/admin/getallusers")]
        [Perm("admin")]
        public IActionResult GetUsers()
        {
            return Ok("done reaching");
        }

        [HttpGet("/api/admin/getallblogs")]
        [Perm("admin","editor")]
        public async Task<IActionResult> GetAllBlogs(BlogThumbsDTO blogThumbsDTO)
        {
            return Ok();
        }


    }
}
