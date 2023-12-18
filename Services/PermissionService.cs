using Almondcove.Interefaces.Services;

namespace Almondcove.Services
{
    public class PermissionService(IHttpContextAccessor httpContextAccessor, ILogger<PermissionService> logger) : IPermissionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<PermissionService> _logger = logger;

        public string CurrentRole()
        {
            try
            {
                string role = _httpContextAccessor.HttpContext.Session.GetString("role");

                if (string.IsNullOrEmpty(role)) return "guest";

                return role.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting permission, message: {message}", ex.Message);
                return "guest";
            }
        }

        public Task<bool> IsActionAllowed()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsPageAllowed(int UserId)
        {
            throw new NotImplementedException();
        }
    }
}