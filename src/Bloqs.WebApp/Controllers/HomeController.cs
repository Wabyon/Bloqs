using System.Web.Mvc;

namespace Bloqs.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}