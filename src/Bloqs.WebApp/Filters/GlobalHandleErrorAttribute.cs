using System;
using System.Diagnostics;
using System.Net;
using System.Web.Mvc;

namespace Bloqs.Filters
{
    public class GlobalHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null) throw new ArgumentNullException("filterContext");

            Trace.TraceError(filterContext.Exception.Message);

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                HandleAjaxRequestException(filterContext);
            }
            else
            {
                base.OnException(filterContext);
            }
        }

        private static void HandleAjaxRequestException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled) return;

            filterContext.Result = new JsonResult
            {
                Data = new
                {
                    filterContext.Exception.Message,
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode =
                (int) HttpStatusCode.InternalServerError;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}