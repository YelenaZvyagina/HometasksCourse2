using System;
using System.Collections.Generic;
using NUnit.Framework;
using Test3;

namespace QueueTests
{
    public class Tests
    {
        BlockingQueue<string> testQueue = new BlockingQueue<string>( );

        [SetUp]
        public void Setup()
        {
            testQueue.Enqueue(1, "abc");
            testQueue.Enqueue(1, "notABC");
            testQueue.Enqueue(2, "idontknow");
            testQueue.Enqueue(3, "lowpriority");
        }

        [Test]
        public void DequeueTest()
        {
            var result = testQueue.Dequeue();
            Assert.AreEqual( "abc", result);
        }

        [Test]
        public void SizeTest()
        {
            var size = testQueue.Size();
            Assert.AreEqual(3, size);
        }

    }    
    
}
