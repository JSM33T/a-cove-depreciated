using almondCove.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace almondCove.Controllers
{
    public class AccountRouteController : Controller
    {
        [Authorize(Policy = "RequireAdminRole")]
        [Route("/account")]
        public IActionResult Index()
        {
            return View("Views/Account/Index.cshtml");
        }

        [AllowAnonymous]
        [Route("/account/login")]
        public IActionResult Login()
        {
            return View("Views/Account/Login.cshtml");
            //return Unauthorized();
        }

        [AllowAnonymous]
        [Route("/account/sign-up")]
        public IActionResult SignUp()
        {
            return View("Views/Account/SignUp.cshtml");
        }

        [AllowAnonymous]
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
