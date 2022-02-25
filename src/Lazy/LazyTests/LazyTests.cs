using System;
using System.Threading;
using Lazy;
using NUnit.Framework;

namespace LazyTests;

    /// <summary>
    /// Test class for creating lazy objects and getting result correctly
    /// </summary>
public class Tests
{
    private readonly Func<int> _supplier;
    private int _countSmth;

    public Tests()
    {
        _supplier = () =>
        {
            _countSmth += 15;
            return _countSmth;
        };
    }

    [SetUp]
    public void Setup()
    {
        _countSmth = 0;
    }

    [Test]
    public void NullSupplierSingleThread()
    {
        Assert.Throws<ArgumentNullException>(() => LazyFactory.CreateSingleThreadedLazy<string>(null!));
    }

    [Test]
    public void NullSupplierMultiTread()
    {
        Assert.Throws<ArgumentNullException>(() => LazyFactory.CreateMultiThreadedLazy<string>(null!));
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

