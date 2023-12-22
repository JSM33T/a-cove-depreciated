using almondcove.Filters;
using Microsoft.AspNetCore.Mvc;

namespace almondcove.Controllers.Routes
{
    public class ProfileRouteController : Controller
    {
        [Perm("user", "admin", "editor")]
        [Route("/profile")]
        public IActionResult Index()
        {
            return View("/Views/Profile/Dashboard.cshtml");
        }

        [Perm("user", "admin", "editor")]
        [Route("/profile/edit")]
        public IActionResult Edit()
        {
            return View("/Views/Profile/EditProfile.cshtml");
        }


    }
}
