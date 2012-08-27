namespace Log5
{
    using System;
    using System.Collections.Generic;
    using System.Timers;

    public abstract partial class Logger
    {

        public class Buffer : Logger
        {
            private Timer _minTimer;
            private Timer _maxTimer;
            private List<LogEntry> _bufferedEntries;

            private readonly Object _bufferLock = new Object();
            private readonly Object _timerLock = new Object();

            public bool HasBulkLogger { get { return DestinationLogger.IsBulkLogger; } }

            public readonly ILogger DestinationLogger;
            public TimeSpan MinimumWaitTime { get; set; }
            public TimeSpan MaximumWaitTime { get; set; }

            public Buffer(ILogger destinationLogger, TimeSpan minimumWaitTime, TimeSpan maximumWaitTime)
            {

                #region Argument Checking

                if (destinationLogger == null)
                {
                    throw new ArgumentException("Destination logger cannot be null", "destinationLogger");
                }

                #endregion
                
                DestinationLogger = destinationLogger;
                MinimumWaitTime = minimumWaitTime;
                MaximumWaitTime = maximumWaitTime;
                _bufferedEntries = new List<LogEntry>();
            }

            public override void Log(LogEntry logEntry)
            {
                lock (_bufferLock)
                {
                    _bufferedEntries.Add(logEntry);
                }

                lock (_timerLock)
                {

                    if (_maxTimer == null)
                    {
                        _minTimer = new Timer(MinimumWaitTime.TotalMilliseconds) { AutoReset = false };
                        _maxTimer = new Timer(MaximumWaitTime.TotalMilliseconds) { AutoReset = false };
                        _minTimer.Elapsed += UnspoolBuffer;
                        _maxTimer.Elapsed += UnspoolBuffer;
                        _minTimer.Start();
                        _maxTimer.Start();
                    }
                    else
                    {
                        if (_minTimer != null)
                        {
                            _minTimer.Close();
                        }
                        _minTimer = new Timer(MinimumWaitTime.TotalMilliseconds) { AutoReset = false };
                        _minTimer.Elapsed += UnspoolBuffer;
                        _minTimer.Start();
                    }
                }
            }

            public void Flush()
            {
                UnspoolBuffer(null, null);
            }


            private void UnspoolBuffer(object sender, ElapsedEventArgs e)
            {
                System.Console.WriteLine("UnspoolBuffer called");

                lock (_timerLock)
                {
                    if (_maxTimer != null)
                    {
                        _maxTimer.Close();
                        _maxTimer = null;
                    }

                    if (_minTimer != null)
                    {
                        _minTimer.Close();
                        _minTimer = null;
                    }
                }

                List<LogEntry> entries;

                lock (_bufferLock)
                {
                    entries = _bufferedEntries;
                    _bufferedEntries = new List<LogEntry>();
                }

                System.Console.WriteLine("(Thread {1}) UnspoolBuffer: Got {0} entries",
                    entries.Count,
                    System.Threading.Thread.CurrentThread.ManagedThreadId
                );

                if (entries.Count == 0)
                {
                    return;
                }

                if (HasBulkLogger)
                {
                    ((Bulk)DestinationLogger).Log(entries);
                }
                else
                {
                    foreach (var entry in entries)
                    {
                        DestinationLogger.Log(entry);
                    }
                }
            }
        }
    }
}
