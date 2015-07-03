using System.Web.Mvc;
using Bloqs.Logging;

namespace Bloqs.Filters
{
    public class AccessLogFilter : ActionFilterAttribute
    {
        private readonly IWebAccessLogger _webAccessLogger = LogManager.GetWebAccessLogger();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _webAccessLogger.Write();
            base.OnActionExecuting(filterContext);
        }
    }
}