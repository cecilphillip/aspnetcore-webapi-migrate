using System.Web.Mvc;

namespace ConferenceAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {          
            return Content("Hi Guys");
        }
    }
}
