namespace ThreadPoolTestss;

using System;
using System.Threading;
using NUnit.Framework;
using ThreadPool;

[TestFixture]
public class Tests
{
    [Test]
    public void TasksDoingCorrectlyTest()
    {
        var threadPool = new MyThreadPool(5);
        var tasks = new IMyTask<int>[10];
        for (var i = 0; i < tasks.Length; i++)
        {
            var k = i;
            tasks[i] = threadPool.Submit(() => k * 2);
        }
        for (var i = 0; i < tasks.Length; ++i)
        {
            Assert.AreEqual(i * 2, tasks[i].Result);
        }
        threadPool.ShutDown();
    }
    
    [Test]
    public void ContinueWithSimpleTest()
    {
        var threadPool = new MyThreadPool(5);
        var task = threadPool.Submit(() => 2 * 2).ContinueWith(x => x.ToString());
        Assert.AreEqual("4", task.Result);
        threadPool.ShutDown();
    }

    [Test]
    public void ExceptionTest()
    {
        var threadPool = new MyThreadPool(3);
        var task = threadPool.Submit(() => new int[1]);
        var wrongTask = task.ContinueWith(x => x[2]);
        Assert.Throws<AggregateException>(() => _ = wrongTask.Result);
        threadPool.ShutDown();
    }
    
    [Test]
    public void SeveralContinuationsTest()
    {
        var threadPool = new MyThreadPool(2);
        var task = threadPool.Submit(() => 2);
        var task1 = task.ContinueWith(x => x * 2);
        var task2 = task.ContinueWith(x => x * 5);
        Assert.AreEqual(4, task1.Result);
        Assert.AreEqual(10, task2.Result);
        threadPool.ShutDown();
    }

    [Test]
    public void ConcurrentTest()
    {
        var threads = new Thread[5];
        var threadPool = new MyThreadPool(1);
        var tasks = new IMyTask<int>[5];
        var firstTask = threadPool.Submit(() => 0);
        for (var i = 0; i < threads.Length; ++i)
        {
            var k = i;
            threads[i] = new Thread(() => 
            {
                tasks[k] = threadPool.Submit(() => firstTask.Result + k);
            });
        }
        foreach (var thread in threads)
        {
            thread.Start();
        }
        foreach (var thread in threads)
        {
            thread.Join();
        }
        for (var i = 0; i < 5; ++i)
        {
            Assert.IsTrue(tasks[i].Result == i);
        }
        threadPool.ShutDown();
    }

    [Test]
    public void ConcurrentTest1()
    {
        var threads = new Thread[5];
        var pool = new MyThreadPool(1);
        var tasks = new IMyTask<int>[5];
        for (var i = 0; i < 4; ++i)
        {
            var k = i;
            threads[i] = new Thread(() => 
            {
                tasks[k] = pool.Submit(() => k * 2);
            });
        }
        threads[4] = new Thread(() =>
        { 
            pool.ShutDown();
        });
        foreach (var thread in threads)
        {
            thread.Start();
        }
        foreach (var thread in threads)
        {
            thread.Join();
        }
        for (var i = 0; i < 4; ++i)
        {
            Assert.IsTrue(tasks[i].Result == i * 2);
        }
        pool.ShutDown();
    }
}