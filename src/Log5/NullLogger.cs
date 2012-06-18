namespace Log5
{
    public class NullLogger : Logger
    {

        protected override void LogInternal(string logLine)
        {
            // Do nothing
        }

    }
}
