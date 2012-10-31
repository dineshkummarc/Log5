namespace Log5
{
    using System;
    using System.Collections.Generic;

    using Common;

    public abstract partial class Logger : ILogger
    {
        protected readonly Guid Guid;

        protected Logger()
        {
            Guid = Guid.NewGuid();
        }

        #region Overrides of Object

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var logger = obj as Logger;
            return logger != null && Guid.Equals(logger.Guid);
        }

        #endregion

        #region Implementation of ILogger

        public virtual bool IsBulkLogger { get { return false; } }

        public abstract void Log(LogEntry logEntry);

        #region Log Methods


        #region Generic

        public void Log(LogLevel logLevel, string msg)
        {
            var entry = new LogEntry(logLevel, msg);
            Log(entry);
        }

        public void Log(LogLevel logLevel, string msg, params Json[] args)
        {
            var entry = new LogEntry(logLevel, msg, args);
            Log(entry);
        }

        public void Log(LogLevel logLevel, string msg, Dictionary<string, Json> parameters)
        {
            var entry = new LogEntry(logLevel, msg, parameters);
            Log(entry);
        }

        public void LogFormat(LogLevel logLevel, string msg, Dictionary<string, Json> parameters)
        {
            var entry = new LogEntry(logLevel, msg, parameters);
            Log(entry);
        }

        public void LogFormat(LogLevel logLevel, string msg, params Json[] args)
        {
            var entry = new LogEntry(logLevel, msg, args);
            Log(entry);
        }

        #endregion


        #region Debug

        public void Debug(string msg)
        {
            Log(LogLevel.Debug, msg);
        }

        public void Debug(string msg, Dictionary<string, Json> parameters)
        {
            LogFormat(LogLevel.Debug, msg, parameters);
        }

        public void Debug(string msg, params Json[] args)
        {
            LogFormat(LogLevel.Debug, msg, args);
        }

        public void DebugFormat(string msg, Dictionary<string, Json> parameters)
        {
            LogFormat(LogLevel.Debug, msg, parameters);
        }

        public void DebugFormat(string msg, params Json[] args)
        {
            LogFormat(LogLevel.Debug, msg, args);
        }

        #endregion


        #region Info

        public void Info(string msg)
        {
            Log(LogLevel.Info, msg);
        }

        public void Info(string msg, Dictionary<string, Json> parameters)
        {
            LogFormat(LogLevel.Info, msg, parameters);
        }

        public void Info(string msg, params Json[] args)
        {
            LogFormat(LogLevel.Info, msg, args);
        }

        public void InfoFormat(string msg, Dictionary<string, Json> parameters)
        {
            LogFormat(LogLevel.Info, msg, parameters);
        }

        public void InfoFormat(string msg, params Json[] args)
        {
            LogFormat(LogLevel.Info, msg, args);
        }

        #endregion


        #region Warn

        public void Warn(string msg)
        {
            Log(LogLevel.Warn, msg);
        }

        public void Warn(string msg, Dictionary<string, Json> parameters)
        {
            Log(LogLevel.Warn, msg, parameters);
        }

        public void Warn(string msg, params Json[] args)
        {
            Log(LogLevel.Warn, msg, args);
        }

        public void WarnFormat(string msg, Dictionary<string, Json> parameters)
        {
            LogFormat(LogLevel.Warn, msg, parameters);
        }

        public void WarnFormat(string msg, params Json[] args)
        {
            LogFormat(LogLevel.Warn, msg, args);
        }

        #endregion


        #region Error

        public void Error(string msg)
        {
            Log(LogLevel.Error, msg);
        }

        public void Error(string msg, params Json[] args)
        {
            LogFormat(LogLevel.Error, msg, args);
        }

        public void Error(string msg, Dictionary<string, Json> parameters)
        {
            LogFormat(LogLevel.Error, msg, parameters);
        }

        public void ErrorFormat(string msg, params Json[] args)
        {
            LogFormat(LogLevel.Error, msg, args);
        }

        public void ErrorFormat(string msg, Dictionary<string, Json> parameters)
        {
            LogFormat(LogLevel.Error, msg, parameters);
        }

        #endregion


        #region Fatal

        public void Fatal(string msg)
        {
            Log(LogLevel.Fatal, msg);
        }

        public void Fatal(string msg, Dictionary<string, Json> parameters)
        {
            LogFormat(LogLevel.Fatal, msg, parameters);
        }

        public void Fatal(string msg, params Json[] args)
        {
            LogFormat(LogLevel.Fatal, msg, args);
        }

        public void FatalFormat(string msg, Dictionary<string, Json> parameters)
        {
            Log(LogLevel.Fatal, msg, parameters);
        }

        public void FatalFormat(string msg, params Json[] args)
        {
            Log(LogLevel.Fatal, msg, args);
        }

        #endregion

        #endregion

        #endregion
    }
}
