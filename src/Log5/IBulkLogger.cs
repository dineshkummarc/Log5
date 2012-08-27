namespace Log5
{
    using System.Collections.Generic;


    public interface IBulkLogger : ILogger
    {
        void Log(IEnumerable<LogEntry> entries);
    }
}
