namespace Log5
{
    using System.Collections.Generic;

    public class MemoryLogger : Logger
    {

        public List<string> Messages { get; private set; }


        public MemoryLogger()
        {
            Messages = new List<string>();
        }


        protected override void LogInternal(string logLine)
        {
            Messages.Add(logLine);
        }

    }
}
