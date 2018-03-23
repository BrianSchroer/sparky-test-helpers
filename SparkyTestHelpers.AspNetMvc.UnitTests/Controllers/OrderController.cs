using System.Web.Mvc;

namespace SparkyTestHelpers.AspNetMvc.UnitTests.Controllers
{
    public class OrderController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }
    }
}
