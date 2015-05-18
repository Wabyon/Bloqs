using System.Configuration;
using System.Web.Http;
using Bloqs.Data;

namespace Bloqs
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            DataConfig.Start(ConfigurationManager.ConnectionStrings["Default"].ConnectionString, false);
        }
    }
}
