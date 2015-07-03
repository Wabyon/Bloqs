using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Bloqs.Logging;

namespace Bloqs.Filters
{
    public class AccessLogFilter : ActionFilterAttribute
    {
        private readonly IApiAccessLogger _apiAccessLogger = LogManager.GetApiAccessLogger();

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            _apiAccessLogger.Write();
            base.OnActionExecuting(actionContext);
        }
    }
}