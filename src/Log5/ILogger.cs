namespace Log5
{
    using Common;

    public interface ILogger
    {
        bool IsBulkLogger { get; }

        void Log(LogEntry logEntry);

        void Log(LogLevel logLevel, string msg);
        void Log(LogLevel logLevel, string msg, params Json[] args);
        void LogFormat(LogLevel logLevel, string msg, params Json[] args);

        void Debug(string msg);
        void Debug(string msg, params Json[] args);
        void DebugFormat(string msg, params Json[] args);

        void Info(string msg);
        void Info(string msg, params Json[] args);
        void InfoFormat(string msg, params Json[] args);

        void Warn(string msg);
        void Warn(string msg, params Json[] args);
        void WarnFormat(string msg, params Json[] args);

        void Error(string msg);
        void Error(string msg, params Json[] args);
        void ErrorFormat(string msg, params Json[] args);

        void Fatal(string msg);
        void Fatal(string msg, params Json[] args);
        void FatalFormat(string msg, params Json[] args);
    }
}
