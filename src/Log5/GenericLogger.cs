namespace Log5
{
    using System;

    public class GenericLogger : Logger
    {

        /// <summary>
        /// The function to send log strings to. If this is null,
        /// then logging does nothing
        /// </summary>
        public Action<string> LogFunction { get; set; }


        /// <summary>
        /// Construct a generic logger, with the given log function
        /// </summary>
        /// <param name="logFunction">The function to call when logging</param>
        public GenericLogger(Action<string> logFunction)
        {
            LogFunction = logFunction;
        }


        /// <summary>
        /// The internal log function. Every public logging method
        /// feeds into this function
        /// </summary>
        /// <param name="logLine">The log string</param>
        protected override void LogInternal(string logLine)
        {
            if (LogFunction != null)
            {
                LogFunction(logLine);
            }
        }
    }
}
