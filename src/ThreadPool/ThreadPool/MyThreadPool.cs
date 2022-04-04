using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ThreadPool
{
    public partial class MyThreadPool
    {
        private static readonly ConcurrentQueue<Action> TasksQueued = new();
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly Thread[] _threads;
        private readonly AutoResetEvent _newTask = new(false);
        private readonly AutoResetEvent threadStopped = new(false);
        private object lockObject = new ();
        private int numberOfTasks;
        private int numberOfWorkingThreads;
        


        public MyThreadPool(int threadCount)
        {
            if (threadCount < 1) throw new ArgumentException("Amount of threads should be positive");
            
            _threads = new Thread[threadCount];

            for (var i = 0; i < threadCount; ++i)
            {
                _threads[i] = new Thread(() =>
                {
                    while (true)
                    {
                        if (_cancellationTokenSource.Token.IsCancellationRequested && TasksQueued.IsEmpty)
                        {
                            Interlocked.Decrement(ref numberOfWorkingThreads);
                            threadStopped.Set();
                            return;
                        }

                        if (TasksQueued.TryDequeue(out var action))
                        {
                            action();
                        }
                        else
                        {
                            _newTask.WaitOne();
                        }
                    }
                });
                _threads[i].Start();

                Interlocked.Increment(ref numberOfWorkingThreads);
            }
        }
        
        private void EnqueueTask(Action task)
        {
            TasksQueued.Enqueue(task);
            _newTask.Set();
        }


        public IMyTask<TResult> Submit<TResult>(Func<TResult> function)
        {
            var task = new MyTask<TResult>(function, this);
            lock (lockObject)
            {
                ArgumentNullException.ThrowIfNull(function);

                if (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    if (numberOfWorkingThreads != 0)
                    {
                        EnqueueTask(task.RunTask);
                        return task;
                    }
                }
            }
            throw new OperationCanceledException("ThreadPool is already shut down, sorry(");
        }
        
        
        
        public void ShutDown()
        {
            lock (lockObject)
            {
                _cancellationTokenSource.Cancel();
            }
            while (numberOfWorkingThreads != 0)
            {
                _newTask.Set();

                threadStopped.WaitOne();
            }
        }
    }
}