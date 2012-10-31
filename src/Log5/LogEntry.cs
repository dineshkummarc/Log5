namespace Log5
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Internal;

    using Newtonsoft.Json;

    using Json = Common.Json;

    /// <summary>
    /// Represents a single log entry
    /// </summary>
    public class LogEntry : IEquatable<LogEntry>
    {
        private readonly DateTime _dateTime;
        private readonly LogLevel _logLevel;
        private readonly string _message;
        private readonly TagList _tagList;
        private readonly Dictionary<string, Json> _parameters;
        private readonly Dictionary<string, Json> _attachments;
        private readonly bool _format;


        #region Constructor and Static Constructor-like Methods

        public LogEntry(LogLevel logLevel, string message)
        {
            _logLevel = logLevel;
            _message = message;
            _parameters = new Dictionary<string, Json>();
            _dateTime = DateTime.UtcNow;
            _tagList = new TagList();
            _attachments = new Dictionary<string, Json>();
            _format = false;
        }


        public LogEntry(LogLevel logLevel, string message, Dictionary<string, Json> parameters)
        {
            _logLevel = logLevel;
            _message = message;
            _parameters = parameters;
            _dateTime = DateTime.UtcNow;
            _tagList = new TagList();
            _attachments = new Dictionary<string, Json>();
            _format = true;
        }


        public LogEntry(LogLevel logLevel, string message, params Json[] args) : this(logLevel, message, CreateParameters(args)) { }


        [JsonConstructor]
        private LogEntry(LogLevel logLevel, DateTime dateTime, string message, TagList tagList, Dictionary<string, Json> parameters, Dictionary<string, Json> attachments, bool format)
        {
            _logLevel = logLevel;
            _dateTime = dateTime;
            _message = message;
            _tagList = tagList;
            _parameters = parameters;
            _attachments = attachments;
            _format = format;
        }


        public static LogEntry CreateRawEntry(LogLevel logLevel, DateTime dateTime, string message, string[] args, TagList tagList, Dictionary<string, Json> attachments, bool format)
        {
            return new LogEntry(logLevel, dateTime, message, tagList, CreateParameters(args), attachments, format);
        }

        protected static LogEntry Log(LogLevel logLevel, string message)
        {
            return new LogEntry(logLevel, message);
        }


        protected static LogEntry LogFormat(LogLevel logLevel, string message, params Json[] args)
        {
            return new LogEntry(logLevel, message, args);
        }


        public static LogEntry Info(string message)
        {
            return Log(LogLevel.Info, message);
        }


        public static LogEntry Info(string message, params Json[] args)
        {
            return LogFormat(LogLevel.Info, message, args);
        }


        public static LogEntry InfoFormat(string message, params Json[] args)
        {
            return LogFormat(LogLevel.Info, message, args);
        }


        public static LogEntry Debug(string message)
        {
            return Log(LogLevel.Debug, message);
        }


        public static LogEntry Debug(string message, params Json[] args)
        {
            return LogFormat(LogLevel.Debug, message, args);
        }


        public static LogEntry DebugFormat(string message, params Json[] args)
        {
            return LogFormat(LogLevel.Debug, message, args);
        }


        public static LogEntry Warn(string message)
        {
            return Log(LogLevel.Warn, message);
        }


        public static LogEntry Warn(string message, params Json[] args)
        {
            return LogFormat(LogLevel.Warn, message, args);
        }


        public static LogEntry WarnFormat(string message, params Json[] args)
        {
            return LogFormat(LogLevel.Warn, message, args);
        }


        public static LogEntry Error(string message)
        {
            return Log(LogLevel.Error, message);
        }


        public static LogEntry Error(string message, params Json[] args)
        {
            return LogFormat(LogLevel.Error, message, args);
        }


        public static LogEntry ErrorFormat(string message, params Json[] args)
        {
            return LogFormat(LogLevel.Error, message, args);
        }


        public static LogEntry Fatal(string message)
        {
            return Log(LogLevel.Fatal, message);
        }


        public static LogEntry Fatal(string message, params Json[] args)
        {
            return LogFormat(LogLevel.Fatal, message, args);
        }


        public static LogEntry FatalFormat(string message, params Json[] args)
        {
            return LogFormat(LogLevel.Fatal, message, args);
        }

        #endregion


        #region Tag Operations

        public LogEntry Tag(List<string> tags)
        {
            foreach (var tag in tags)
            {
                _tagList.Add(tag);
            }

            return this;
        }


        public LogEntry Tag(string tags)
        {
            _tagList.AddMultiple(tags);
            return this;
        }

        #endregion


        #region Attached Object Operations

        public LogEntry AttachObject(string key, Json obj)
        {
            if (_attachments.ContainsKey(key))
            {
                throw new ArgumentException("An object with the key '" + key + "' already exists", "key");
            }

            _attachments[key] = obj;

            return this;
        }

        #endregion


        #region Field Exposure Properties

        public DateTime DateTime { get { return _dateTime; } }
        public LogLevel LogLevel { get { return _logLevel; } }
        public string Message { get { return _message; } }
        public TagList TagList { get { return _tagList; } }
        public Dictionary<string, Json> Parameters { get { return _parameters; } }
        public Dictionary<string, Json> Attachments { get { return _attachments; } }

        #endregion


        #region Implementation of IEquatable<Entry>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(LogEntry other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other != null
                   && _dateTime == other._dateTime
                   && _logLevel == other._logLevel
                   && _message == other._message
                   && _tagList.Equals(other._tagList)
                   && (_format == other._format || _parameters.Count != 0)
                   && Helpers.Equals(_parameters, other._parameters)
                   && Helpers.Equals(_attachments, other._attachments);
        }

        #endregion


        #region Overrides of Object

        public override string ToString()
        {
            if (!_format)
            {
                return Message;
            }

            return Helpers.AtFormat(Message, Parameters, permissive: true);
        }

        #endregion

        #region Helpers

        private static Dictionary<string, Json> CreateParameters(Json[] args)
        {
            var dict = new Dictionary<string, Json>();

            for (var i = 0; i < args.Length; i++)
            {
                var key = i.ToString(CultureInfo.InvariantCulture);
                dict[key] = args[i];
            }

            return dict;
        }

        private static Dictionary<string, Json> CreateParameters(string[] args)
        {
            var dict = new Dictionary<string, Json>();

            for (var i = 0; i < args.Length; i++)
            {
                var key = i.ToString(CultureInfo.InvariantCulture);
                dict[key] = args[i];
            }

            return dict;
        }

        #endregion
    }
}
