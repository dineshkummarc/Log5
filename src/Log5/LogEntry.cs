namespace Log5
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;


    /// <summary>
    /// Represents a single log entry
    /// </summary>
    public class LogEntry : IEquatable<LogEntry>
    {

        private readonly DateTime _dateTime;
        private readonly LogLevel _logLevel;
        private readonly string _message;
        private readonly object[] _args;
        private readonly TagList _tagList;
        private readonly Dictionary<string, Object> _attachedObjects;


        #region Constructor and Static Constructor-like Methods

        public LogEntry(LogLevel logLevel, string message, params object[] args)
        {
            _logLevel = logLevel;
            _message = message;
            _args = args;
            _dateTime = DateTime.UtcNow;
            _tagList = new TagList();
            _attachedObjects = new Dictionary<string, object>();
        }


        [JsonConstructor]
        private LogEntry(LogLevel logLevel, DateTime dateTime, string message, object[] args, TagList tagList, Dictionary<string, Object> attachedObjects)
        {
            _logLevel = logLevel;
            _dateTime = dateTime;
            _message = message;
            _args = args;
            _tagList = tagList;
            _attachedObjects = attachedObjects;
        }


        public LogEntry CreateRawEntry(LogLevel logLevel, DateTime dateTime, string message, object[] args, TagList tagList, Dictionary<string, Object> attachedObjects)
        {
            return new LogEntry(logLevel, dateTime, message, args, tagList, attachedObjects);
        }


        public static LogEntry Info(string message, params object[] args)
        {
            return new LogEntry(LogLevel.Info, message, args.Length != 0 ? args : null);
        }

        public static LogEntry InfoFormat(string message, params object[] args)
        {
            return new LogEntry(LogLevel.Info, message, args);
        }

        public static LogEntry Debug(string message, params object[] args)
        {
            return new LogEntry(LogLevel.Debug, message, args.Length != 0 ? args : null);
        }

        public static LogEntry DebugFormat(string message, params object[] args)
        {
            return new LogEntry(LogLevel.Debug, message, args);
        }

        public static LogEntry Warn(string message, params object[] args)
        {
            return new LogEntry(LogLevel.Warn, message, args.Length != 0 ? args : null);
        }

        public static LogEntry WarnFormat(string message, params object[] args)
        {
            return new LogEntry(LogLevel.Warn, message, args);
        }

        public static LogEntry Error(string message, params object[] args)
        {
            return new LogEntry(LogLevel.Error, message, args.Length != 0 ? args : null);
        }

        public static LogEntry ErrorFormat(string message, params object[] args)
        {
            return new LogEntry(LogLevel.Error, message, args);
        }

        public static LogEntry Fatal(string message, params object[] args)
        {
            return new LogEntry(LogLevel.Fatal, message, args.Length != 0 ? args : null);
        }

        public static LogEntry FatalFormat(string message, params object[] args)
        {
            return new LogEntry(LogLevel.Fatal, message, args);
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

        public LogEntry AttachObject(string key, Object obj)
        {
            if (_attachedObjects.ContainsKey(key))
            {
                throw new ArgumentException("An object with the key '" + key + "' already exists", "key");
            }

            _attachedObjects[key] = obj;

            return this;
        }

        #endregion


        #region Field Exposure Properties

        public DateTime DateTime { get { return _dateTime; } }
        public LogLevel LogLevel { get { return _logLevel; } }
        public string Message { get { return _message; } }
        public object[] Args { get { return _args; } }
        public TagList TagList { get { return _tagList; } }
        public Dictionary<string, Object> AttachedObjects { get { return _attachedObjects; } }

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
                   && Helpers.Equals(_args, other._args)
                   && Helpers.Equals(_attachedObjects, other._attachedObjects);
        }

        #endregion
    }
}
