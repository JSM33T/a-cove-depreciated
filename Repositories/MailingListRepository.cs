using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using almondcove.Models.Domain;
using Microsoft.Data.SqlClient;

namespace almondcove.Repositories
{
    public class MailingListRepository(IConfigManager configManager) : IMailingListRepository
    {
        public async Task<(bool Success, string Message)> PostMail(Mail mail)
        {
            int emailCount,maxId;

            string checkEmailQuery = @"
                SELECT 
                    COUNT(*) AS EmailCount, 
                    (SELECT MAX(Id) FROM TblMailingList) AS MaxId 
                FROM 
                    TblMailingList 
                WHERE 
                    Email = @Email";

            try
            {
                using var connection = new SqlConnection(configManager.GetConnString());
                await connection.OpenAsync();

                using var command = new SqlCommand(checkEmailQuery, connection);
                command.Parameters.AddWithValue("@Email", mail.Email);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    emailCount = reader.GetInt32(reader.GetOrdinal("EmailCount"));
                    maxId = reader.GetInt32(reader.GetOrdinal("MaxId"));
                }
                else
                {
                    return (false, "Invalid data");
                }
                await reader.CloseAsync();


                if (emailCount != 0) return (false, "Email already exists");
               
                string insertQuery = @"
                    INSERT INTO TblMailingList (Id, Email, Origin, DateAdded)
                    VALUES (@Id, @Email, @Origin, @DateAdded)";

                using var command2 = new SqlCommand(insertQuery, connection);
                command2.Parameters.AddWithValue("@Id", maxId + 1);
                command2.Parameters.AddWithValue("@Email", mail.Email?.Trim());
                command2.Parameters.AddWithValue("@Origin", mail.Origin?.Trim());
                command2.Parameters.AddWithValue("@DateAdded", DateTime.Now);

                int rowsAffected = await command2.ExecuteNonQueryAsync();
                return (true, "Email added successfully");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}");
            }
        }

    }
}
