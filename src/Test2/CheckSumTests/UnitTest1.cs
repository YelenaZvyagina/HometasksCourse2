using System;
using NUnit.Framework;
using Test2;

namespace CheckSumTests
{
    public class Tests
    {
        [SetUp]
        public void CheckForException()
        {
            var exSeq = Assert.Throws<ArgumentException>(() => CheckSum.CheckSumSimple("whatever"));
            Assert.That(exSeq is { Message: "There is no fle or directory there" });
        }
        
        [SetUp]
        public void CheckForExceptionMultiThreaded()
        {
            var exSeq = Assert.Throws<ArgumentException>(() => CheckSum.CheckSumMultiThreaded("something not fileable"));
            Assert.That(exSeq is { Message: "There is no fle or directory there" });
        }

    }
}
