using System.Configuration;
using Bloqs.Logging;

namespace Bloqs
{
    public class GlobalSettings
    {
        public static string DefaultConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Default"].ConnectionString; }
        }

        public static string LogDataConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Log"].ConnectionString; }
        }

        public static string ApiAddress
        {
            get { return ConfigurationManager.AppSettings["bloqs:ApiAddress"]; }
        }

        public static TraceLogLevel TraceLogLevel
        {
            get { return TraceLogLevel.FromString(ConfigurationManager.AppSettings["bloqs:TraceLogLevel"]); }
        }
    }
}