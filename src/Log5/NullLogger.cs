namespace Log5
{
    public class NullLogger : Logger
    {

        public override void Log(LogLevel logLevel, string msg)
        {
            // Do nothing
        }

    }
}
