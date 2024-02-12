using almondcove.Filters;
using Microsoft.AspNetCore.Mvc;

namespace almondcove.Controllers.Routes
{
    public class ProfileRouteController : Controller
    {
        [Perm("user", "admin", "editor")]
        [Route("/profile")]
        public IActionResult Index() => View("/Views/Profile/Dashboard.cshtml");

        [Perm("user", "admin", "editor")]
        [Route("/profile/edit")]
        public IActionResult Edit() => View("/Views/Profile/EditProfile.cshtml");

    }
}
