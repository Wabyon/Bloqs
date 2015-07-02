using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bloqs.Internals;
using Bloqs.Logging.Data;
using Newtonsoft.Json;

namespace Bloqs.Logging
{
    public class WebAccessLogger : WebAccessLoggerBase
    {
        public WebAccessLogger() : base(GlobalSettings.LogDataConnectionString)
        {
        }

        protected override Func<WebAccessLog> GetWebAccessLog
        {
            get
            {
                return () =>
                {
                    var request = HttpContext.Current.Request;

                    string actionName = null;
                    string controllerName = null;
                    var directRouteActions = request.RequestContext.RouteData.DataTokens["MS_DirectRouteActions"];
                    ActionDescriptor[] actionDescriptors = null;
                    var actions = directRouteActions as ActionDescriptor[];
                    if (actions != null)
                    {
                        actionDescriptors = actions;
                    }
                    if (actionDescriptors != null && actionDescriptors.Any())
                    {
                        var action = actionDescriptors[0];
                        actionName = action.ActionName;
                        controllerName = action.ControllerDescriptor.ControllerName;
                    }

                    var form = request.Form.ToDictionary();
                    form.Remove("__RequestVerificationToken");
                    form.Remove("Password");

                    var log = new WebAccessLog
                    {
                        UserName = (request.LogonUserIdentity == null) ? "" : request.LogonUserIdentity.Name,
                        ServerName = HttpContext.Current.Server.MachineName,
                        Url = request.Url.ToString(),
                        HttpMethod = request.HttpMethod,
                        Path = request.Path,
                        Query = JsonConvert.SerializeObject(request.QueryString.ToDictionary()),
                        Form = JsonConvert.SerializeObject(form),
                        Controller = controllerName,
                        Action = actionName,
                        UserAgent = request.UserAgent,
                        UserHostAddress = request.UserHostAddress
                    };

                    return log;
                };
            }
        }
    }
}