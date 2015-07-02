using Bloqs.Logging;
using Bloqs.Logging.Data;

namespace Bloqs
{
    public class LoggingConfig
    {
        public static void Initialize()
        {
            LogDataConfig.Initialize(GlobalSettings.LogDataConnectionString);
            DefaultTraceLogInitializer.Initialize(GlobalSettings.LogDataConnectionString, GlobalSettings.TraceLogLevel);
            LogManager.RegisterWebAccessLogger<WebAccessLogger>();
        }
    }
}