namespace Log5
{
    using System.Collections.Generic;


    public partial class Logger
    {
        public class Memory : Logger
        {

            public List<LogEntry> Entries { get; private set; }


            public Memory()
            {
                Entries = new List<LogEntry>();
            }


            public override void Log(LogEntry logEntry)
            {
                Entries.Add(logEntry);
            }

        }
    }

}
