using almondCove.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace almondCove.Controllers
{
    public class AuthRouteController : Controller
    {
        [Route("/account/{something?}")]
        public IActionResult Login()
        {
            return View("Views/Auth/Index.cshtml");
        }
     
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
