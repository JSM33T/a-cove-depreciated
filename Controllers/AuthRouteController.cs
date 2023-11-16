using almondCove.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace almondCove.Controllers
{
    public class AuthRouteController : Controller
    {
        [Route("/login")]
        public IActionResult Login()
        {
            return View("Views/Auth/Login.cshtml");
        }
        [Route("/sing-up")]
        public IActionResult SignUp()
        {
            return View("Views/Auth/SiugnUp.cshtml");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
