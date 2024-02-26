using almondcove.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace almondcove.Controllers.Routes
{
    public class AccountRouteController() : Controller
    {

        
        [Route("/account")]
        public IActionResult Index() => View("Views/Account/Index.cshtml");
         
        [Route("/account/login")]
        public IActionResult Login() => View("Views/Account/Login.cshtml");
        
        [Route("/account/sign-up")]
        public IActionResult SignUp() => View("Views/Account/SignUp.cshtml");
        
        [Route("/account/recover-account")]
        public IActionResult AccountRecovery() => View("Views/Account/AccountRecovery.cshtml");

        [Route("/account/logout")]
        public void LogOut()
        {   
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Redirect("/");   
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
