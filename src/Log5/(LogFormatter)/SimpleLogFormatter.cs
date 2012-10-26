namespace Log5
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    public static partial class LogFormatter
    {

        public class Simple : ILogFormatter<string>
        {

            /// <summary>
            /// This regular expression will match Windows, UNIX, or Mac newlines
            /// Before printing, the log formatter will convert any newlines found
            /// to <code>Environment.NewLine</code>
            /// </summary>
            private static readonly Regex ReNewLine = new Regex(@"(?:\r\n|\r|\n)");


            /// <summary>
            /// The default date format
            /// </summary>
            public static readonly string DefaultDateFormat = "yyyy/MM/dd HH:mm:ss fff";


            /// <summary>
            /// This is the value passed into <code>DateTime.ToString</code> to format
            /// the date in which the log was created
            /// </summary>
            public string DateFormat { get; set; }


            /// <summary>
            /// The time zone to print out the date in. Defaults to <code>TimeZone.CurrentTimeZone</code>
            /// </summary>
            public TimeZone TimeZone { get; set; }


            /// <summary>
            /// Create a simple log formatter
            /// </summary>
            public Simple()
            {
                DateFormat = DefaultDateFormat;
                TimeZone = TimeZone.CurrentTimeZone;
            }


            #region Implementation of ILogFormatter<out string>

            public virtual string Format(LogEntry logEntry)
            {
                var dateString = TimeZone.ToLocalTime(logEntry.DateTime).ToString(DateFormat);
                var prefix = PrefixString(dateString, logEntry.LogLevel, logEntry.TagList);
                var continuation = ContinuationString(prefix, logEntry);
                var message = logEntry.ToString();

                // Normalize newlines
                message = ReNewLine.Replace(message, Environment.NewLine);
                var messageLines = message.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                var builder = new StringBuilder();

                builder.Append(prefix);
                builder.AppendLine(messageLines[0]);

                for (var i = 1; i < messageLines.Length; i++)
                {
                    builder.Append(continuation);
                    builder.AppendLine(messageLines[i]);
                }

                return builder.ToString();
            }

            protected virtual string PrefixString(string dateString, LogLevel logLevel, TagList tagList)
            {
                return String.Format("[{0} : {1, -5}] ", dateString, logLevel);
            }

            protected virtual string ContinuationString(string prefixString, LogEntry logEntry)
            {
                return new String(' ', prefixString.Length - 2) + "| ";
            }

            #endregion
        }
    }
}
