using System.Diagnostics;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Bloqs.Logging;
using Newtonsoft.Json;

namespace Bloqs.Filters
{
    public class TraceLogFilter : ActionFilterAttribute
    {
        private readonly ITraceLogger _traceLogger = LogManager.GetTraceLogger("API");

        private Stopwatch _stopwatch;

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            _stopwatch = Stopwatch.StartNew();
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var controller = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var action = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            var area = string.Empty;
            var method = actionExecutedContext.Request.Method;
            var key = new { area, controller, action, method };

            _traceLogger.Trace(new TraceLogMessage(0, JsonConvert.SerializeObject(key), _stopwatch.ElapsedMilliseconds));

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}