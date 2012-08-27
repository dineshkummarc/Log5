namespace Log5
{
    using System;
    using System.Collections.Generic;


    public partial class Logger
    {

        public class Intermediate : Logger
        {

            private ILogger _logger;
            public ILogger Logger
            {
                get
                {
                    return _logger;
                }

                set
                {
                    if (value == null)
                    {
                        throw new ArgumentException("Logger must not be null", "value");
                    }

                    _logger = value;
                }
            }

            public Func<string, string> MessageFilter;
            public readonly TagList TagList;
            public readonly Dictionary<string, object> AttachObjects;


            public Intermediate(ILogger logger)
            {
                Logger = logger;
                TagList = new TagList();
                AttachObjects = new Dictionary<string, object>();
            }


            public override void Log(LogEntry logEntry)
            {
                foreach (var tag in TagList)
                {
                    logEntry.Tag(tag);
                }

                foreach (var kv in AttachObjects)
                {
                    var key = kv.Key;
                    var obj = kv.Value;

                    logEntry.AttachObject(key, obj);
                }
            }
        }
    }
}
