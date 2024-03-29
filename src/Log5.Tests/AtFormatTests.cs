﻿namespace Log5.Tests
{
    using System.Collections.Generic;

    using Common;
    using NUnit.Framework;

    using Helpers = Internal.Helpers;

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
            var dict = new Dictionary<string, Json> { { "xyz", "test" } };
            var resultString = Helpers.AtFormat(formatString, dict);
            Assert.AreEqual(expectedString, resultString);
        }


        [Test]
        public static void TwoParameterTests()
        {
            var expectedString = "foobar";
            var formatString = "@abc@def";
            var dict = new Dictionary<string, Json>
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
            var dict = new Dictionary<string, Json>
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
            var dict = new Dictionary<string, Json>
            {
                { "abc", "foo" },
                { "def", "bar" }
            };
            var resultString = Helpers.AtFormat(formatString, dict);
            Assert.AreEqual(expectedString, resultString);
        }

        [Test]
        public static void PermissiveTest1()
        {
            var expectedString = "foo @v2 qux";
            var formatString = "@v1 @v2 @v3";
            var dict = new Dictionary<string, Json>
            {
                { "v1", "foo" },
                { "V2", "bar" },
                { "v3", "qux" },
            };

            var resultString = Helpers.AtFormat(formatString, dict, permissive: true);
            Assert.AreEqual(expectedString, resultString);
        }

        [Test]
        public static void PermissiveTest2()
        {
            var expectedString = "foo @{v2 @{v3}";
            var formatString = "@{v1} @{v2 @{v3}";
            var dict = new Dictionary<string, Json>
            {
                { "v1", "foo" },
                { "v2", "bar" },
                { "v3", "qux" },
            };

            var resultString = Helpers.AtFormat(formatString, dict, permissive: true);
            Assert.AreEqual(expectedString, resultString);
        }

        [Test]
        public static void PermissiveTest3()
        {
            var expectedString = "foo @{v2 @v3";
            var formatString = "@{v1} @{v2 @v3";
            var dict = new Dictionary<string, Json>
            {
                { "v1", "foo" },
                { "v2", "bar" },
                { "v3", "qux" },
            };

            var resultString = Helpers.AtFormat(formatString, dict, permissive: true);
            Assert.AreEqual(expectedString, resultString);
        }
    }
}
