namespace Log5
{
    using Newtonsoft.Json;


    public static partial class LogFormatter
    {

        public class Json : ILogFormatter<string>
        {

            public string Format(LogEntry logEntry)
            {
                return JsonConvert.SerializeObject(logEntry);
            }
        }
    }
}
