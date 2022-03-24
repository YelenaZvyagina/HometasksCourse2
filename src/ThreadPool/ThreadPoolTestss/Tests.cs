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

        [SetUp]
        public void SetUp()
        {
            
            

        }
        
        [Test]
        public void SimpleTest1()
        {
            MyThreadPool threadPool = new(5);
            var task = threadPool.Submit(() => 2 * 2);
            Assert.AreEqual(4, task.Result);
            threadPool.ShutDown();
        }
        
        [Test]
        public void TaskResultShouldBeCorrect()
        {
            var tasks = new IMyTask<int>[5];
            var threadPool = new MyThreadPool(5);

            for (var i = 0; i < tasks.Length; ++i)
            {
                var localI = i;
                tasks[i] = threadPool.Submit(() => localI * 2);
            }
            
            for (int i = 0; i < tasks.Length; ++i)
            {
                Assert.AreEqual(i * 2, tasks[i].Result);
            }
            
            threadPool.ShutDown();

        }
    }
}

