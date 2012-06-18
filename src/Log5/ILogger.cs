namespace Log5
{
    public interface ILogger
    {
        ILogFormatter LogFormatter { get; set; }

        void Log(LogLevel logLevel, string msg);
        void LogFormat(LogLevel logLevel, string msg, params object[] formatObjects);

        void Debug(string msg);
        void DebugFormat(string msg, params object[] formatObjects);

        void Info(string msg);
        void InfoFormat(string msg, params object[] formatObjects);

        void Warn(string msg);
        void WarnFormat(string msg, params object[] formatObjects);

        void Error(string msg);
        void ErrorFormat(string msg, params object[] formatObjects);

        void Fatal(string msg);
        void FatalFormat(string msg, params object[] formatObjects);
    }
}
