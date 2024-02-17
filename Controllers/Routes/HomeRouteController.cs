using Microsoft.AspNetCore.Mvc;

namespace almondcove.Controllers.Routes
{
    public class HomeRouteController() : Controller
    {
        [Route("/")]
        public IActionResult Index() => View("Views/Home/Index.cshtml");

        [Route("/about")]
        public IActionResult About() => View("Views/Home/About.cshtml");

        [Route("/faq")]
        public IActionResult Faq() => View("Views/Home/FAQs.cshtml");

        [Route("/changelog")]
        public IActionResult Changelog() => View("Views/Home/Changelog.cshtml");

        [Route("/attributions")]
        public IActionResult Attributions() => View("Views/Home/Attributions.cshtml");

        [Route("/contact")]
        public IActionResult Contact() => View("Views/Home/Contact.cshtml");

        [Route("/404")]
        public IActionResult NF() => View("Views/Home/404.cshtml");

        [Route("/401")]
        public IActionResult UA() => View("Views/Home/AccessDenied.cshtml");
    }
}