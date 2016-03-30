using Microsoft.AspNet.Mvc;

namespace ConferenceAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("Hi Guys");
        }
    }
}
