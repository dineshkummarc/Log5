namespace Log5
{
    public class DebugConsoleLogger : Logger
    {
        protected override void LogInternal(string logLine)
        {
            #if DEBUG

                System.Diagnostics.Debug.WriteLine(logLine);

            #endif
        }
    }
}
