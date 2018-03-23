using System.Web.Mvc;

namespace SparkyTestHelpers.AspNetMvc.UnitTests.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
