using almondcove.Interefaces.Services;
using almondcove.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace almondcove.Controllers.Api
{
    [Route("api/message")]
    [ApiController]
    public class MessageApiController(IConfigManager configManager) : ControllerBase
    {
        public readonly IConfigManager _cnfig = configManager;

        [HttpPost]
        public async Task<IActionResult> SendMessage(MessageDTO _message)
        {
            string email = _message.Email;
            if (User.Identity.IsAuthenticated) email = User.FindFirst(ClaimTypes.Email)?.Value.ToString(); 

            string sqlins = "INSERT INTO TblMessages (Name,Email,Topic,Message,DateAdded,IsRead) VALUES  (@name,@email,@topic, @message ,@dateadded,'false')";
            SqlConnection conn = new(_cnfig.GetConnString());
            await conn.OpenAsync();
            using SqlCommand cmd= new(sqlins, conn);
            cmd.Parameters.AddWithValue("@name", _message.Name);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@topic", _message.Topic);
            cmd.Parameters.AddWithValue("@message", _message.Message);
            cmd.Parameters.AddWithValue("@dateadded", DateTime.Now);
            await cmd.ExecuteNonQueryAsync();

            return Ok("message sent");
        }
    }
}
