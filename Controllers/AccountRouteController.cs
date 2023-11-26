using almondCove.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace almondCove.Controllers
{
    public class AccountRouteController : Controller
    {
        [Route("/account")]
        public IActionResult Index()
        {
            return View("Views/Account/Index.cshtml");
        }

        [Route("/account/login")]
        public IActionResult Login()
        {
            return View("Views/Account/Login.cshtml");
        }

        [Route("/account/sign-up")]
        public IActionResult SignUp()
        {
            return View("Views/Account/SignUp.cshtml");
        }

        [Route("/account/recover-account")]
        public IActionResult AccountRecovery()
        {
            return View("Views/Account/AccountRecovery.cshtml");
        }

        [Route("/account/logout")]
        public void LogOut()
        {
            Response.Cookies.Delete("SessionKey");
            HttpContext.Session.Clear();
            Response.Redirect("/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
