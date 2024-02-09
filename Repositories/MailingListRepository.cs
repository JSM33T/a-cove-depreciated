using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using almondcove.Models.Domain;
using Microsoft.Data.SqlClient;

namespace almondcove.Repositories
{
    public class MailingListRepository(IConfigManager configManager) : IMailingListRepository
    {
        public async Task<(bool Success,string Message)> PostMail(Mail mail)
        {
            int emailCount;
            int maxId;
            string checkEmailQuery = @"
                                SELECT COUNT(*) AS EmailCount, 
                                (SELECT MAX(Id) FROM TblMailingList) AS MaxId 
                                FROM TblMailingList 
                                WHERE Email = @Email
                            ";

            using (var connection = new SqlConnection(configManager.GetConnString()))
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
                    return (false,"Invalid data");
                }
            }

            if (emailCount == 0)
            {
                string insertQuery = @"
                            INSERT INTO TblMailingList (Id, Email, Origin, DateAdded)
                            VALUES (@Id, @Email, @Origin, @DateAdded)
                        ";

                using var connection = new SqlConnection(configManager.GetConnString());
                await connection.OpenAsync();

                using var command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@Id", maxId + 1);
                command.Parameters.AddWithValue("@Email", mail.Email?.Trim());
                command.Parameters.AddWithValue("@Origin", mail.Origin?.Trim());
                command.Parameters.AddWithValue("@DateAdded", DateTime.Now);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return (true, "Email added successfully");
            }
            else
            {
                return (false, "Email already exists");
            }
        }
    }
}
