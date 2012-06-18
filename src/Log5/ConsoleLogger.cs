namespace Log5
{
    using System;

    public class ConsoleLogger : Logger
    {

        protected override void LogInternal(string logLine)
        {
            Console.Write(logLine);
        }

    }
}
