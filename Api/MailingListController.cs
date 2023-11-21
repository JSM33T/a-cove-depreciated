using almondCove.Models.Domain;
using almondCove.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace almondCove.Api
{
    [ApiController]
    public class MailingListController : ControllerBase
    {
        private readonly ILogger<MailingListController> _logger;
        private readonly IConfigManager _configManager;
        public MailingListController(ILogger<MailingListController> logger,IConfigManager configManager)
        {
            _logger = logger;
            _configManager = configManager;
        }

        [HttpPost("/api/mailinglist/subscribe")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetAllMails([FromBody] Mail mail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int emailCount;
                    int maxId;
                    string checkEmailQuery = @"
                                SELECT COUNT(*) AS EmailCount, 
                                (SELECT MAX(Id) FROM TblMailingList) AS MaxId 
                                FROM TblMailingList 
                                WHERE Email = @Email
                            ";

                    using (var connection = new SqlConnection(_configManager.GetConnString()))
                    {
                        await connection.OpenAsync();

                        using var command = new SqlCommand(checkEmailQuery, connection);
                        command.Parameters.AddWithValue("@Email", mail.Email);

                        using var reader = await command.ExecuteReaderAsync();
                        if (reader.Read())
                        {
                            emailCount = (int)reader["EmailCount"];
                            maxId = (int)reader["MaxId"];
                        }
                        else
                        {
                            return BadRequest("Invalid data");
                        }
                    }

                    if (emailCount == 0)
                    {
                        string insertQuery = @"
                            INSERT INTO TblMailingList (Id, Email, Origin, DateAdded)
                            VALUES (@Id, @Email, @Origin, @DateAdded)
                        ";

                        using var connection = new SqlConnection(_configManager.GetConnString());
                        await connection.OpenAsync();

                        using var command = new SqlCommand(insertQuery, connection);
                        command.Parameters.AddWithValue("@Id", maxId + 1);
                        command.Parameters.AddWithValue("@Email", mail.Email?.Trim());
                        command.Parameters.AddWithValue("@Origin", mail.Origin?.Trim());
                        command.Parameters.AddWithValue("@DateAdded", DateTime.Now);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        _logger.LogInformation("email added:" + mail.Email?.ToString());
                        return Ok("Email added successfully");
                    }
                    else
                    {
                        return BadRequest("Email already exists");
                    }
                }
                else
                {
                    _logger.LogError("model invalid");
                    return BadRequest("Invalid data");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("error while email submission:" + ex.Message.ToString());
                return StatusCode(500, "An error occurred while processing the request.");
            }
            
        }

    
    }
}
