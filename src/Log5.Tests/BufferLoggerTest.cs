namespace Log5.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using NUnit.Framework;


    [TestFixture]
    public static class BufferLoggerTest
    {

        internal class BulkMemoryLogger : Logger.Bulk
        {

            public List<LogEntry[]> Entries { get; private set; }


            public BulkMemoryLogger()
            {
                Entries = new List<LogEntry[]>();
            }


            public override void Log(LogEntry logEntry)
            {
                Entries.Add(new []{ logEntry });
            }


            public override void Log(IEnumerable<LogEntry> logEntries)
            {
                Entries.Add(logEntries.ToArray());
            }

        }


        [TestCase(1)]
        public static void CombiningLogFlushTest(int i)
        {
            var minimumWaitTime = new TimeSpan(10000);   // 1 millisecond
            var maximumWaitTime = new TimeSpan(10000);   // 1 millisecond

            var memoryLogger = new BulkMemoryLogger();
            var bufferLogger = new Logger.Buffer(memoryLogger, minimumWaitTime, maximumWaitTime);

            Task.Factory.StartNew(() =>
            {
                bufferLogger.Error("blah 1");
                bufferLogger.Error("blah 2");
                bufferLogger.Error("blah 3");
                bufferLogger.Flush();
            });

            var result =Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(100);
                return Tuple.Create(
                    memoryLogger.Entries.Count,
                    memoryLogger.Entries.Count > 0 ? memoryLogger.Entries[0].Length : -1
                );
            }).Result;

            Assert.AreEqual(1, result.Item1);
            Assert.AreEqual(3, result.Item2);
        }


        [TestCase(1)]

        public static void CombiningLogTest(int i)
        {
            var minimumWaitTime = new TimeSpan(10000);   // 1 millisecond
            var maximumWaitTime = new TimeSpan(200000);  // 20 milliseconds

            var memoryLogger = new BulkMemoryLogger();
            var bufferLogger = new Logger.Buffer(memoryLogger, minimumWaitTime, maximumWaitTime);

            Task.Factory.StartNew(() =>
            {
                bufferLogger.Error("blah 1");
                bufferLogger.Error("blah 2");
                bufferLogger.Error("blah 3");
            });

            var result = Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(100);
                return Tuple.Create(
                    memoryLogger.Entries.Count,
                    memoryLogger.Entries.Count > 0 ? memoryLogger.Entries[0].Length : -1
                );
            }).Result;

            Assert.AreEqual(1, result.Item1);
            Assert.AreEqual(3, result.Item2);
        }
    }
}
