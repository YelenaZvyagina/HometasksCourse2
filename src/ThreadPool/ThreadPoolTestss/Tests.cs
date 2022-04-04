using System.Threading;
using NUnit.Framework;
using ThreadPool;

namespace ThreadPoolTestss
{
    public class Tests
    {
        // считает задачи корректно
        // досчитывает (или выкидывает исключение) после шатдауна
        // не принимает новые задачи после шатдауна
        // continuewith в целом считает корректно
        // continueWith досчитывается после шатдауна
        // обращение к самому трепулу из нескольких потоков -- проверить что если одновременно из разных вызвать continueWith и shutDown то все будет ок
        
        
        [Test]
        public void SimpleTest1()
        {
            MyThreadPool threadPool = new(1);
            var task = threadPool.Submit(() => 2 * 2);
            var task1 = threadPool.Submit(() => 30 + 70);
            Assert.AreEqual(4, task.Result);
            threadPool.ShutDown();
        }

        [Test]
        public void TasksResultCorrectnessTest()
        {
            MyThreadPool threadPool = new(5);
            var tasks = new IMyTask<int> [10];

            for (var i = 0; i < tasks.Length; i++)
            {
                var i1 = i;
                tasks[i] = threadPool.Submit(() => i1 * 10);
            }
            
            for (var i = 0; i < tasks.Length; ++i)
            {
                Assert.AreEqual(i * 10, tasks[i].Result);
            }
            
            threadPool.ShutDown();
        }

        [Test]
        public void ShutDownTest()
        {
            MyThreadPool threadPool = new(2);
            var tasks = new IMyTask<int> [3];
            for (var i = 0; i < tasks.Length; i++)
            {
                var i1 = i;
                tasks[i] = threadPool.Submit(() => i1 * 10);
            }
            threadPool.ShutDown();
            for (var i = 0; i < tasks.Length; ++i)
            {
                Assert.AreEqual(i * 10, tasks[i].Result);
            }
            var taskAfterShutdown = threadPool.Submit(() => 2 * 2);
            Assert.IsFalse(taskAfterShutdown.IsCompleted);
        }

        [Test]
        public void ContinueWithSimpleTest()
        {
            MyThreadPool threadPool = new(5);
            var myTask = threadPool.Submit(() => 2 * 2).ContinueWith(x => x.ToString());
            Assert.AreEqual("4", myTask.Result);

        }

        [Test]
        public void ContinuationAfterShutdownTest()
        {
            MyThreadPool threadPool = new(2);
            var task1 = threadPool.Submit(() => 2 * 2);
            var task2 = threadPool.Submit(() => 2 * 2).ContinueWith(x => x.ToString());
            threadPool.ShutDown();
            Assert.AreEqual("4", task2.Result);
        }
    }
}

