using almondCove.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace almondCove.Controllers
{
    public class AccountRouteController : Controller
    {
        [Route("/account/")]
        public IActionResult Index()
        {
            return View("Views/Account/Index.cshtml");
        }

        [Route("/account/login/")]
        public IActionResult Login()
        {
            return View("Views/Account/Login.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
