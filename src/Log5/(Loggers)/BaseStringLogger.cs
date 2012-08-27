namespace Log5
{
    using System.Collections.Generic;

    public abstract partial class Logger
    {

        public static class Base
        {

            public abstract class String : Logger
            {
                public ILogFormatter<string> LogFormatter { get; private set; }


                protected String(ILogFormatter<string> logFormatter = null)
                {
                    LogFormatter = logFormatter ?? new LogFormatter.Simple();
                }


                public override void Log(LogEntry logEntry)
                {
                    var logLine = LogFormatter.Format(logEntry);
                    Log(logLine);
                }


                protected abstract void Log(string logLine);
            }



            public abstract class BulkString : Bulk
            {
                public ILogFormatter<string> LogFormatter { get; private set; }


                protected BulkString(ILogFormatter<string> logFormatter = null)
                {
                    LogFormatter = logFormatter ?? new LogFormatter.Simple();
                }


                public override void Log(LogEntry logEntry)
                {
                    var logLine = LogFormatter.Format(logEntry);
                    Log(logLine);
                }


                public override void Log(IEnumerable<LogEntry> entries)
                {
                    var logLines = new List<string>();
                    foreach (var entry in entries)
                    {
                        logLines.Add(LogFormatter.Format(entry));
                    }
                    Log(logLines);
                }


                protected abstract void Log(string logLine);

                protected abstract void Log(IList<string> logLines);
            }
        }
    }
}
