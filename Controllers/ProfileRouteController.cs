using Microsoft.AspNetCore.Mvc;

namespace almondcove.Controllers
{
    public class ProfileRouteController : Controller
    {
        [Route("/profile")]
        public IActionResult Index()
        {
            return View("/Views/Profile/Dashboard.cshtml");
        }

        [Route("/profile/edit")]
        public IActionResult Edit()
        {
            return View("/Views/Profile/EditProfile.cshtml");
        }


    }
}
