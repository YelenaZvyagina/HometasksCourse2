using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ThreadPool
{
    public partial class MyThreadPool
    {
        private int _threadCount;
        private static readonly ConcurrentQueue<Action> TasksQueued = new();
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly Thread[] _threads;
        private readonly AutoResetEvent _newTask = new(false);

        public MyThreadPool(int threadCount)
        {
            if (threadCount < 1) throw new ArgumentException("Amount of threads should be positive");
            _threadCount = threadCount;
            _threads = new Thread[threadCount];
            
            for (var i = 0; i < threadCount; i++)
            {
                _threads[i] = new Thread(() =>
                {
                    while (!_cancellationTokenSource.IsCancellationRequested)
                    {
                        if (TasksQueued.TryDequeue(out var task))
                        {
                            _newTask.Set();
                            task();
                        }
                        else
                        {
                            _newTask.WaitOne();
                        }
                    }
                });
                _threads[i].Start();
            }
        }

        public IMyTask<TResult> Submit<TResult>(Func<TResult> function)
        {
            lock (_cancellationTokenSource)
            {
                if (_cancellationTokenSource.IsCancellationRequested) 
                    throw new OperationCanceledException("ThreadPool is already shut down, sorry(");
                ArgumentNullException.ThrowIfNull(function);
                var task = new MyTask<TResult>(function, this);
                TasksQueued.Enqueue(task.RunTask);
                return task;
            }
        }
        
        public void ShutDown()
        {
            lock (_cancellationTokenSource)
            {
                _cancellationTokenSource.Cancel();
            }
            foreach (var workingThread in _threads)
            {
                workingThread.Join();
            }
            _newTask.Reset();
        }
    }
}