using System.Web.Http;
using Bloqs.Data;

namespace Bloqs
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalConfiguration.Configuration.Filters);
            DataConfig.Start(GlobalSettings.DefaultConnectionString, false);
            MappingConfig.CreateMaps();
            LoggingConfig.Initialize();
        }
    }
}
