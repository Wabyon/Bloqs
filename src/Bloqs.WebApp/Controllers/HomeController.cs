using System.Web.Mvc;
using Bloqs.Filters;

namespace Bloqs.Controllers
{
    [TraceLogFilter]
    [AccessLogFilter]
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