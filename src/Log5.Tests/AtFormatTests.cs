namespace Log5.Tests
{
    using System.Collections.Generic;

    using Internal;

    using NUnit.Framework;

    public static class AtFormatTests
    {
        [TestCase("", "")]
        [TestCase("foobar", "foobar")]
        [TestCase("@@", "@")]
        [TestCase("x@@@@@@y", "x@@@y")]
        [TestCase("xyz@@abc@@@@def")]
        [TestCase("@@abc@@@@def")]
        [TestCase("@@abc@@@@")]
        [TestCase("a@@b@@c@@de@@fhijk@@@@@@")]
        public static void SimpleFormatTests(string formatString, string expectedString)
        {
            var resultString = Helpers.AtFormat(formatString, null);
            Assert.AreEqual(expectedString, resultString);
        }


        [Test]
        public static void SingleParameterTest()
        {
            var expectedString = "foo test bar";
            var formatString = "foo @xyz bar";
            var dict = new Dictionary<string, object> { { "xyz", "test" } };
            var resultString = Helpers.AtFormat(formatString, dict);
            Assert.AreEqual(expectedString, resultString);
        }


        [Test]
        public static void TwoParameterTests()
        {
            var expectedString = "foobar";
            var formatString = "@abc@def";
            var dict = new Dictionary<string, object>
            {
                { "abc", "foo" },
                { "def", "bar" }
            };
            var resultString = Helpers.AtFormat(formatString, dict);
            Assert.AreEqual(expectedString, resultString);
        }


        [Test]
        public static void CurlyBraceTest1()
        {
            var expectedString = "foodef";
            var formatString = "@{abc}def";
            var dict = new Dictionary<string, object>
            {
                { "abc", "foo" }
            };
            var resultString = Helpers.AtFormat(formatString, dict);
            Assert.AreEqual(expectedString, resultString);
        }

        [Test]
        public static void CurlyBraceTest2()
        {
            var expectedString = "foobar";
            var formatString = "@{abc}@{def}";
            var dict = new Dictionary<string, object>
            {
                { "abc", "foo" },
                { "def", "bar" }
            };
            var resultString = Helpers.AtFormat(formatString, dict);
            Assert.AreEqual(expectedString, resultString);
        }

    }
}
