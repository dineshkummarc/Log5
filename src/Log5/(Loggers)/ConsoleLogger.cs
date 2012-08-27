namespace Log5
{

    public partial class Logger
    {
        public class Console : Base.String
        {

            public Console(ILogFormatter<string> logFormatter = null) : base(logFormatter) { }

            protected override void Log(string logLine)
            {
                System.Console.Write(logLine);
            }
        }
    }

}
