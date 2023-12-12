using Microsoft.AspNetCore.Mvc;

namespace almondcove.Controllers
{
    public class RootRouteController : Controller
    {
        protected string GetUserRole()
        {
            // Retrieve user role from the session
            return HttpContext.Session.GetString("UserRole");
        }
    }
}
