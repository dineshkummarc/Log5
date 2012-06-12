namespace Log5
{
    using System;

    public class ConsoleLogger : Logger
    {

        public override void Log(LogLevel logLevel, string msg)
        {
            Console.Write(String.Format("[{0} : {1}] {2}", DateTime.Now.ToLongTimeString(), logLevel, msg));
        }

    }
}
