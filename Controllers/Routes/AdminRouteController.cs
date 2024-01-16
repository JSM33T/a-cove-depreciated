using Microsoft.AspNetCore.Mvc;

namespace almondcove.Controllers.Routes
{
    public class AdminRouteController : Controller
    {
        public IActionResult Index() => View();
    }
}
