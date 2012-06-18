namespace Log5
{
    using System;

    public class LogFormatter : ILogFormatter
    {
        public string DateFormat { get; set; }
        public static readonly string DefaultDateFormat = "yyyy/MM/dd HH:mm:ss fff";

        public static readonly string DefaultMessageFormat = "[{0} : {1, -5}] {2}\n";
        private string _messageFormat;
        public string MessageFormat
        {
            get { return _messageFormat; }
            set
            {
                var oldFormat = _messageFormat;
                _messageFormat = value;

                // Check that this format string is valid
                try
                {
                    Format(DateTime.Now, LogLevel.Error, "Test message");
                }
                catch (Exception)
                {
                    _messageFormat = oldFormat;
                }
            }
        }

        public LogFormatter()
        {
            DateFormat = DefaultDateFormat;

            // We know this message format is valid, so skip the test
            _messageFormat = DefaultMessageFormat;
        }


        public string Format(DateTime dateTime, LogLevel logLevel, string message)
        {
            var dateString = DateTime.Now.ToString(DateFormat);
            return String.Format(MessageFormat, dateString, logLevel, message);
        }
    }
}
