namespace Log5
{
    using System.Collections.Generic;

    public partial class Logger
    {

        public abstract class Bulk : Logger, IBulkLogger
        {
            public override bool IsBulkLogger { get { return true; } }

            public abstract void Log(IEnumerable<LogEntry> entries);
        }
    }

}
