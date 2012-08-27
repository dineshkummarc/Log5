namespace Log5.Tests
{
    using NUnit.Framework;

    using Newtonsoft.Json;


    [TestFixture]
    public static class TagListTests
    {
        [Test]
        public static void ClassLikeConstructorTest()
        {
            var tagList = new TagList("foo bar qux");
            Assert.IsTrue(tagList.Contains("foo"));
            Assert.IsTrue(tagList.Contains("bar"));
            Assert.IsTrue(tagList.Contains("qux"));
            Assert.IsFalse(tagList.Contains("baz"));
            Assert.IsFalse(tagList.Contains("fo"));
            Assert.IsFalse(tagList.Contains("whatever"));
        }


        [Test]
        public static void SerializeTest()
        {
            var tagList = new TagList("foo bar qux");
            var json = JsonConvert.SerializeObject(tagList);

            Assert.AreEqual("[\"foo\",\"bar\",\"qux\"]", json);
        }


        [Test]
        public static void DeserializeTest()
        {
            var tagList = new TagList("foo bar qux");
            var json = JsonConvert.SerializeObject(tagList);

            var reconstructed = JsonConvert.DeserializeObject<TagList>(json);

            Assert.AreEqual(tagList, reconstructed);
        }

    }
}
