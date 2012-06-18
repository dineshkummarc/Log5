namespace Log5
{
    using System;
    using System.Collections.Generic;

    public class MemoryLogger : Logger
    {

        public List<string> Messages { get; private set; }


        public MemoryLogger()
        {
            Messages = new List<string>();
        }


        public override void Log(LogLevel logLevel, string msg)
        {
            var dateString = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss fff");
            var message = String.Format("[{0,-5} : {1}] {2}\n", dateString, logLevel, msg);
            Messages.Add(message);
        }

    }
}
