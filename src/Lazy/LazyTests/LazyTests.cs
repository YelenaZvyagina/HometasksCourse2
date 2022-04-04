namespace LazyTests;

using System;
using System.Threading;
using Lazy;
using NUnit.Framework;

/// <summary>
/// Test class for creating lazy objects and getting result correctly
/// </summary>
public class Tests
{
    private readonly Func<int> _supplier;
    private int _count;

    public Tests()
    {
        _supplier = () =>
        {
            _count += 15;
            return _count;
        };
    }

    [SetUp]
    public void Setup()
    {
        _count = 0;
    }

    [Test]
    public void RepeatedCallSingleThreaded()
    {
        var lazyObject = LazyFactory.CreateSingleThreadedLazy(_supplier);
        var resultOfLazy = lazyObject.Get();
        for (var i = 0; i < 10; i++)
        {
            Assert.AreEqual(resultOfLazy, lazyObject.Get());
            Assert.AreEqual(15, lazyObject.Get());
        }
    }
        
    [Test]
    public void RepeatedCallMultiThreaded()
    {
        var lazyObject = LazyFactory.CreateMultiThreadedLazy(_supplier);
        var resultOfLazy = lazyObject.Get();
        for (var i = 0; i < 10; i++)
        {
            Assert.AreEqual(resultOfLazy, lazyObject.Get());
            Assert.AreEqual(15, lazyObject.Get());
        }
    }

    [Test]
    public void IsThereARace()
    {
        var threads = new Thread[10];
        var count = 0;
            
        var lazyObject = LazyFactory.CreateMultiThreadedLazy(() => Interlocked.Increment(ref count));
            
        for (var i = 0; i < 10; i++)
        {
            threads[i] = new Thread(() => lazyObject.Get() );
        }
        foreach (var thread in threads)
        {
            thread.Start();
        }
        foreach (var thread in threads)
        {
            thread.Join();
        }
        Assert.AreEqual(1, count);
    }
}

