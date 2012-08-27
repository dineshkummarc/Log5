namespace Log5
{
    using System;

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

        #endregion


        #region Implementation of ILogger

        public virtual bool IsBulkLogger { get { return false; } }


        public abstract void Log(LogEntry logEntry);


        public void Log(LogLevel logLevel, string msg, params object[] args)
        {
            var entry = new LogEntry(logLevel, msg, args.Length == 0 ? null : args);
            Log(entry);
        }

        public void LogFormat(LogLevel logLevel, string msg, params object[] args)
        {
            var entry = new LogEntry(logLevel, msg, args);
            Log(entry);
        }

        public void Debug(string msg, params object[] args)
        {
            var entry = new LogEntry(LogLevel.Debug, msg, args.Length == 0 ? null : args);
            Log(entry);
        }

        public void DebugFormat(string msg, params object[] args)
        {
            var entry = new LogEntry(LogLevel.Debug, msg, args);
            Log(entry);
        }

        public void Info(string msg, params object[] args)
        {
            var entry = new LogEntry(LogLevel.Info, msg, args.Length == 0 ? null : args);
            Log(entry);
        }

        public void InfoFormat(string msg, params object[] args)
        {
            var entry = new LogEntry(LogLevel.Info, msg, args);
            Log(entry);
        }

        public void Warn(string msg, params object[] args)
        {
            var entry = new LogEntry(LogLevel.Warn, msg, args.Length == 0 ? null : args);
            Log(entry);
        }

        public void WarnFormat(string msg, params object[] args)
        {
            var entry = new LogEntry(LogLevel.Warn, msg, args);
            Log(entry);
        }

        public void Error(string msg, params object[] args)
        {
            var entry = new LogEntry(LogLevel.Error, msg, args.Length == 0 ? null : args);
            Log(entry);
        }

        public void ErrorFormat(string msg, params object[] args)
        {
            var entry = new LogEntry(LogLevel.Warn, msg, args);
            Log(entry);
        }

        public void Fatal(string msg, params object[] args)
        {
            var entry = new LogEntry(LogLevel.Debug, msg, args.Length == 0 ? null : args);
            Log(entry);
        }

        public void FatalFormat(string msg, params object[] args)
        {
            var entry = new LogEntry(LogLevel.Fatal, msg, args);
            Log(entry);
        }

        #endregion

    }
}
