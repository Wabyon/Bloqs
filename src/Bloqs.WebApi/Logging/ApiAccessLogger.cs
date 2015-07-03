using System;
using System.Web;
using Bloqs.Logging.Data;
using Bloqs.Internals;
using Newtonsoft.Json;

namespace Bloqs.Logging
{
    public class ApiAccessLogger : ApiAccessLoggerBase
    {
        public ApiAccessLogger() : base(GlobalSettings.LogDataConnectionString)
        {
        }

        protected override Func<ApiAccessLog> GetApiAccessLog
        {
            get
            {
                return () =>
                {
                    var request = HttpContext.Current.Request;

                    //TODO
                    string actionName = null;
                    string controllerName = null;

                    var form = request.Form.ToDictionary();
                    form.Remove("__RequestVerificationToken");
                    form.Remove("Password");

                    var log = new ApiAccessLog
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