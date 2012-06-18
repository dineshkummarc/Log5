namespace Log5
{
    using System;

    public abstract class Logger : ILogger
    {
        #region Implementation of ILogger

        public abstract void Log(LogLevel logLevel, string msg);

        public void LogFormat(LogLevel logLevel, string msg, params object[] formatObjects)
        {
            var formattedMsg = String.Format(msg, formatObjects);
            Log(logLevel, formattedMsg);
        }

        public void Debug(string msg)
        {
            Log(LogLevel.Debug, msg);
        }

        public void DebugFormat(string msg, params object[] formatObjects)
        {
            LogFormat(LogLevel.Debug, msg, formatObjects);
        }

        public void Info(string msg)
        {
            Log(LogLevel.Info, msg);
        }

        public void InfoFormat(string msg, params object[] formatObjects)
        {
            LogFormat(LogLevel.Info, msg, formatObjects);
        }

        public void Warn(string msg)
        {
            Log(LogLevel.Warn, msg);
        }

        public void WarnFormat(string msg, params object[] formatObjects)
        {
            LogFormat(LogLevel.Warn, msg, formatObjects);
        }

        public void Error(string msg)
        {
            Log(LogLevel.Error, msg);
        }

        public void ErrorFormat(string msg, params object[] formatObjects)
        {
            LogFormat(LogLevel.Error, msg, formatObjects);
        }

        public void Fatal(string msg)
        {
            Log(LogLevel.Error, msg);
        }

        public void FatalFormat(string msg, params object[] formatObjects)
        {
            LogFormat(LogLevel.Fatal, msg, formatObjects);
        }

        #endregion

        public static readonly NullLogger Null = new NullLogger();
    }
}
