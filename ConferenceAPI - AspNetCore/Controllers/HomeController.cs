using Microsoft.AspNetCore.Mvc;

namespace ConferenceAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => Content("Hi Guys");
    }
}
