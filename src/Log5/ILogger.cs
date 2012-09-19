namespace Log5
{
    public interface ILogger
    {
        bool IsBulkLogger { get; }

        void Log(LogEntry logEntry);

        void Log(LogLevel logLevel, string msg);
        void Log(LogLevel logLevel, string msg, params object[] args);
        void LogFormat(LogLevel logLevel, string msg, params object[] args);

        void Debug(string msg);
        void Debug(string msg, params object[] args);
        void DebugFormat(string msg, params object[] args);

        void Info(string msg);
        void Info(string msg, params object[] args);
        void InfoFormat(string msg, params object[] args);

        void Warn(string msg);
        void Warn(string msg, params object[] args);
        void WarnFormat(string msg, params object[] args);

        void Error(string msg);
        void Error(string msg, params object[] args);
        void ErrorFormat(string msg, params object[] args);

        void Fatal(string msg);
        void Fatal(string msg, params object[] args);
        void FatalFormat(string msg, params object[] args);
    }
}
