namespace Log5.Tests
{
    using System;

    using Common;
    using NUnit.Framework;
    using Newtonsoft.Json;


    [TestFixture]
    public static class EntryTests
    {
        [TestCase(LogLevel.Error, "A simple log error message")]
        [TestCase(LogLevel.Info, "Info info ")]
        public static void CanSerializeSimpleEntryToJson(LogLevel level, string msg)
        {
            var entry = new LogEntry(level, msg);
            var dt = entry.DateTime;
            var dtstr = dt.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFFZ");
            var expectedJson = String.Format("{{\"DateTime\":\"{0}\",\"LogLevel\":{1},\"Message\":\"{2}\",\"TagList\":[],\"Parameters\":{{}},\"Attachments\":{{}}}}", dtstr, (int) level, msg);
            var json = JsonConvert.SerializeObject(entry);

            Assert.AreEqual(expectedJson, json);
        }


        [Test]
        public static void CanSerializeComplexEntryToJson()
        {
            var entry = new LogEntry(LogLevel.Debug, "DEBUG", new Json[] { "arg" });
            entry.Tag("tag");
            entry.AttachObject("two", 2);
            var dt = entry.DateTime;
            var dtstr = dt.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFFZ");
            var expectedJson = String.Format("{{\"DateTime\":\"{0}\",\"LogLevel\":{1},\"Message\":\"{2}\",\"TagList\":[\"tag\"],\"Parameters\":{{\"0\":\"arg\"}},\"Attachments\":{{\"two\":2}}}}", dtstr, (int)LogLevel.Debug, "DEBUG");
            var json = JsonConvert.SerializeObject(entry);

            Assert.AreEqual(expectedJson, json);
        }


        [Test]
        public static void CanDeserializeComplexEntry()
        {
            var entry = new LogEntry(LogLevel.Debug, "DEBUG", new Json[] { "arg" });
            entry.Tag("tag");
            entry.AttachObject("two", 2);
            var json = JsonConvert.SerializeObject(entry);

            var reconstructedEntry = JsonConvert.DeserializeObject<LogEntry>(json);

            Assert.AreEqual(entry, reconstructedEntry);
        }
    }
}
