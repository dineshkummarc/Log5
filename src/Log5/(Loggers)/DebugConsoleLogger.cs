namespace Log5
{
    public partial class Logger
    {

        public class DebugConsoleLogger : Logger
        {
            public ILogFormatter<string> LogFormatter { get; private set; }

            public DebugConsoleLogger(ILogFormatter<string> logFormatter = null)
            {
                LogFormatter = logFormatter ?? new LogFormatter.Simple();
            }


            public override void Log(LogEntry logEntry)
            {
                var logLine = LogFormatter.Format(logEntry);

                #if DEBUG

                    System.Diagnostics.Debug.WriteLine(logLine);

                #endif
            }
        }
    }
}
