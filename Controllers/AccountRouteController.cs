using almondcove.Enums;
using almondcove.Models;
using almondcove.Modules;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace almondcove.Controllers
{
    public class AccountRouteController(ILogger<AccountRouteController> logger) : RootController
    {
        private readonly ILogger<AccountRouteController> _logger = logger;

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
           // _logger.LogError("derived: {something} and real: {sasas}",GetUserRole().ToString(), HttpContext.Session.GetString("role").ToString());
            
            //if (PermissionHelper.HasPermission(GetUserRole(), Perm.Guest))
            //{
            //    return View("Views/Account/AccountRecovery.cshtml");
            //}
            //else
            //{ return Redirect("/"); }
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
