namespace Log5
{
    using System;

    public partial class Logger
    {
        public class GenericLogger : Logger
        {

            /// <summary>
            /// The function to send log strings to. If this is null,
            /// then logging does nothing
            /// </summary>
            public Action<LogEntry> LogFunction { get; set; }


            /// <summary>
            /// Construct a generic logger, with the given log function
            /// </summary>
            /// <param name="logFunction">The function to call when logging</param>
            public GenericLogger(Action<LogEntry> logFunction)
            {
                LogFunction = logFunction;
            }


            /// <summary>
            /// The internal log function. Every public logging method
            /// feeds into this function
            /// </summary>
            /// <param name="logEntry">The log entry</param>
            public override void Log(LogEntry logEntry)
            {
                if (LogFunction != null)
                {
                    LogFunction(logEntry);
                }
            }
        }
    }
}
