using System;
using NUnit.Framework;
using Test2;

namespace CheckSumTests
{
    public class Tests
    {
        [Test]
        public void CheckForException()
        {
            var exSeq = Assert.Throws<ArgumentException>(() => CheckSum.CheckSumSimple("whatever"));
            Assert.That(exSeq is { Message: "There is no fle or directory there" });
        }
        
        [Test]
        public void CheckForExceptionMultiThreaded()
        {
            var exSeq = Assert.ThrowsAsync<ArgumentException>(() => CheckSum.CheckSumMultiThreaded("somethingnotfileable") );
            Assert.That(exSeq is { Message: "There is no fle or directory there" });
        }

    }
}
