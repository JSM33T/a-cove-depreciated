using Microsoft.AspNetCore.Mvc;

namespace almondcove.Controllers
{
    public class RootController : Controller
    {
        public string GetUserRole()
        {
            return HttpContext.Session.GetString("role");
        }
    }
}
