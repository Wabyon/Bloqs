using System;
using System.Web.Http.Filters;
using Bloqs.Logging;

namespace Bloqs.Filters
{
    public class GlobalHandleErrorAttribute : ExceptionFilterAttribute
    {
        private readonly ITraceLogger _traceLogger = LogManager.GetTraceLogger("APP");

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext == null) throw new ArgumentNullException("actionExecutedContext");

            _traceLogger.Error(actionExecutedContext.Exception);

            base.OnException(actionExecutedContext);
        }
    }
}