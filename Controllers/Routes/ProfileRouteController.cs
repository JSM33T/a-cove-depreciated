using almondcove.Filters;
using Microsoft.AspNetCore.Mvc;

namespace almondcove.Controllers.Routes
{
    public class ProfileRouteController : Controller
    {
        
        [Route("/profile")]
        public IActionResult Index() => View("/Views/Profile/Dashboard.cshtml");

        
        [Route("/profile/edit")]
        public IActionResult Edit() => View("/Views/Profile/EditProfile.cshtml");

    }
}
