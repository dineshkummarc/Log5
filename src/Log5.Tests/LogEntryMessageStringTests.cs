namespace Log5.Tests
{
    using System;
    using System.Collections.Generic;

    using Common;
    using NUnit.Framework;

    public static class LogEntryMessageStringTests
    {
        [Test]
        public static void MessageStringTest1()
        {
            var expectedPartialString = "This is a test message. Foo = FOO, Bar = BAR, Qux = QUX" + Environment.NewLine;
            var formatter = new LogFormatter.Simple();
            var entry = new LogEntry(
                LogLevel.Error,
                "This is a test message. Foo = @foo, Bar = @bar, Qux = @qux",
                new Dictionary<string, Json>
                {
                    { "foo", "FOO" },
                    { "bar", "BAR" },
                    { "qux", "QUX" }
                }
            );

            var resultString = formatter.Format(entry);

            // This will pass if [resultString] ends with [expectedPartialString]
            var i = resultString.IndexOf(expectedPartialString, StringComparison.InvariantCulture);
            Assert.AreEqual(resultString.Length, i + expectedPartialString.Length);
        }


        [Test]
        public static void MessageStringTest2()
        {
            var expectedPartialString = "This is a test message. Foo = FOO, Bar = BAR, Qux = QUX" + Environment.NewLine;
            var formatter = new LogFormatter.Simple();
            var entry = new LogEntry(
                LogLevel.Error,
                "This is a test message. Foo = @0, Bar = @1, Qux = @2",
                "FOO", "BAR", "QUX"
            );

            var resultString = formatter.Format(entry);

            // This will pass if [resultString] ends with [expectedPartialString]
            var i = resultString.IndexOf(expectedPartialString, StringComparison.InvariantCulture);
            Assert.AreEqual(resultString.Length, i + expectedPartialString.Length);
        }
    }
}
