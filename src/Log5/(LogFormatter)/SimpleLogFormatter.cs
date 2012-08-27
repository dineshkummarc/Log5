namespace Log5
{
    using System;


    public static partial class LogFormatter
    {

        public class Simple : ILogFormatter<string>
        {

            #region Default Properties

            public static readonly string DefaultDateFormat = "yyyy/MM/dd HH:mm:ss fff";
            public static readonly string DefaultMessageFormat = "[{0} : {1, -5}] {2}\n";

            #endregion


            /// <summary>
            /// This is the value passed into <code>DateTime.ToString</code> to format
            /// the date in which the log was created
            /// </summary>
            public string DateFormat { get; set; }
        

            /// <summary>
            /// This is the value passed in to <code>String.Format</code> to format the log
            /// message. It's paramaters are the date, the log level, and the message text
            /// </summary>
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
                        var testEntry = LogEntry.Info("Log message test");
                        Format(testEntry);
                    }
                    catch (ArgumentNullException)
                    {
                        _messageFormat = oldFormat;
                    }
                    catch (FormatException)
                    {
                        _messageFormat = oldFormat;
                    }
                }
            }

            public Simple()
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

            #region Implementation of ILogFormatter<out string>

            public string Format(LogEntry logEntry)
            {
                var dateString = logEntry.DateTime.ToString(DateFormat);

                string message = logEntry.Args != null
                    ? String.Format(logEntry.Message, logEntry.Args)
                    : logEntry.Message;

                return String.Format(MessageFormat, dateString, logEntry.LogLevel, message);
            }

            #endregion
        }
    }

    
}
