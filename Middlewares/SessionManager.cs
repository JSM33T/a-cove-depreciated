using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using Microsoft.Data.SqlClient;

namespace almondcove.Middlewares
{
    public class SessionManager(RequestDelegate next,IConfigManager configManager)
    {
        private readonly RequestDelegate _next = next;
        private readonly IConfigManager _configManager = configManager;
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Session.GetString("username") == null && context.Request.Cookies.ContainsKey("SessionKey"))
            {
                context.Request.Cookies.TryGetValue("SessionKey", out string cookieValue);
                using SqlConnection connection = new(_configManager.GetConnString());
                await connection.OpenAsync();
                SqlCommand checkcommand =
                    new(
                        @"select p.*,a.Image 
                        from TblUserProfile p,TblAvatarMaster a 
                        WHERE SessionKey = @sessionkey
                        and p.IsActive= 1
                        and p.IsVerified = 1
                        and p.AvatarId = a.Id ",
                        connection
                    );
                checkcommand.Parameters.AddWithValue("@sessionkey", cookieValue);
                using var reader = await checkcommand.ExecuteReaderAsync();
                if (reader.Read())
                {
                    var username = reader.GetString(reader.GetOrdinal("UserName"));
                    var user_id = reader.GetInt32(reader.GetOrdinal("Id"));
                    var firstname = reader.GetString(reader.GetOrdinal("FirstName"));
                    var fullname = firstname + " " + reader.GetString(reader.GetOrdinal("LastName"));
                    var role = reader.GetString(reader.GetOrdinal("Role"));
                    var avatar = reader.GetString(reader.GetOrdinal("Image"));
                    
                    //set session
                    context.Session.SetString("user_id", user_id.ToString());
                    context.Session.SetString("username", username);
                    context.Session.SetString("first_name", firstname);
                    context.Session.SetString("role", role);
                    context.Session.SetString("fullname", fullname);
                    context.Session.SetString("avatar", avatar.ToString());
                }
            }
            await _next(context);
        }
    }
}
