using System;
using Bloqs.Logging.Internals;

namespace Bloqs.Logging
{
    public sealed class LogManager
    {
        private static Type _webAccessLoggerType;

        private static Type _apiAccessLoggerType;

        public static void RegisterWebAccessLogger<T>()
            where T : IWebAccessLogger, new()
        {
            _webAccessLoggerType = typeof (T);
        }

        public static void RegisterApiAccessLogger<T>()
            where T : IApiAccessLogger, new()
        {
            _apiAccessLoggerType = typeof (T);
        }

        public static ITraceLogger GetTraceLogger(string name)
        {
            return new TraceLogger(name);
        }

        public static IWebAccessLogger GetWebAccessLogger()
        {
            if (_webAccessLoggerType == null) return new EmptyWebAccessLogger();
            var logger = Activator.CreateInstance(_webAccessLoggerType) as IWebAccessLogger;
            return logger ?? new EmptyWebAccessLogger();
        }

        public static IApiAccessLogger GetApiAccessLogger()
        {
            if (_apiAccessLoggerType == null) return new EmptyApiAccessLogger();
            var logger = Activator.CreateInstance(_apiAccessLoggerType) as IApiAccessLogger;
            return logger ?? new EmptyApiAccessLogger();
        }
    }
}
