using Bloqs.Logging;

namespace Bloqs
{
    public class LoggingConfig
    {
        public static void Initialize()
        {
            DefaultTraceLogInitializer.Initialize(GlobalSettings.LogDataConnectionString, GlobalSettings.TraceLogLevel);
            LogManager.RegisterApiAccessLogger<ApiAccessLogger>();
        }
    }
}