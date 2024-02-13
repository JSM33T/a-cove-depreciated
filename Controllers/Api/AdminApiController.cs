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
        
        [HttpGet("/api/admin/getallusers")]
        public IActionResult GetUsers() => Ok("done reaching");

        [HttpGet("/api/admin/getallblogs")]
        public IActionResult GetAllBlogs(BlogThumbsDTO blogThumbsDTO) => Ok(blogThumbsDTO);


    }
}
