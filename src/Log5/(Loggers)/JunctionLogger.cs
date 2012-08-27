namespace Log5
{
    using System;
    using System.Collections.Generic;


    public partial class Logger
    {

        public Junction Filter(ILogger logger, Func<LogEntry, bool> func)
        {
            var junction = new Junction();
            junction.Add(logger, func);

            return junction;
        }


        public class Junction : Logger
        {

            private readonly Dictionary<ILogger, Func<LogEntry, bool>> _map;


            public Junction()
            {
                _map = new Dictionary<ILogger, Func<LogEntry, bool>>();
            }


            public void Add(ILogger logger, Func<LogEntry, bool> func)
            {
                if (_map.ContainsKey(logger))
                {
                    var oldFunc = _map[logger];
                    _map[logger] = l => oldFunc(l) || func(l);
                }
                else
                {
                    _map[logger] = func;
                }
            }


            public void Subtract(ILogger logger, Func<LogEntry, bool> func)
            {
                if (!_map.ContainsKey(logger))
                {
                    return;
                }

                var oldFunc = _map[logger];
                _map[logger] = l => oldFunc(l) && !func(l);
            }


            public bool Remove(ILogger logger)
            {
                return _map.Remove(logger);
            }


            public void Clear()
            {
                _map.Clear();
            }


            #region Overrides of Logger

            public override void Log(LogEntry logEntry)
            {
                foreach (var kv in _map)
                {
                    if (kv.Value(logEntry))
                    {
                        kv.Key.Log(logEntry);
                    }
                }
            }

            #endregion
        }
    }
}
