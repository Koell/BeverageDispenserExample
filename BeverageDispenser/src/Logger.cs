using System.Diagnostics;

namespace BeverageDispenser {
    public sealed class Logger
    {
        private static Logger? _instance = null;
        private static readonly object padlock = new object();

        private Logger()
        {
            // initialize logger
        }

        public static Logger? Instance
        {
            get
            {
                lock (padlock) {
                    return _instance ??= new Logger();
                }
            }
        }

        public void Log(string message)
        {
            Trace.TraceInformation(message);
        }

        public void LogError(string errorMessage, Exception? ex = null)
        {
            Trace.TraceError(errorMessage);
            Trace.TraceError(ex?.ToString());
        }
    }
}
