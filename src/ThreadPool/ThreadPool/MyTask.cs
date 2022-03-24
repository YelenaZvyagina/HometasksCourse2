using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ThreadPool
{
    public partial class MyThreadPool
    { 
        private class MyTask<TResult> : IMyTask<TResult> 
        {
            private readonly MyThreadPool _myThreadPool;
            private readonly CancellationTokenSource _cancellationTokenSource;
            private readonly Func<TResult> _taskFunction;
            private AutoResetEvent are = new(false);
            private ConcurrentQueue<Action> _continueTasks = new();
            private object _lockObject = new();
            public bool IsCompleted {get; set;}
            public TResult Result {get; set;}
            
            public MyTask(Func<TResult> func, MyThreadPool myThreadPool)
            {
                _taskFunction = func;
                _myThreadPool = myThreadPool;
                _cancellationTokenSource = _myThreadPool._cancellationTokenSource;
            }
            
            public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> function)
            {
                lock (_cancellationTokenSource)
                {
                    if (_cancellationTokenSource.IsCancellationRequested)
                    {
                        throw new  InvalidOperationException("ThreadPool was shut down");
                    }
                }
                var a = _myThreadPool.Submit(_taskFunction);
                TNewResult F1() => function(a.Result);
                if (IsCompleted)
                {
                    return _myThreadPool.Submit(F1);
                }
                var task = new MyTask<TNewResult>(F1, _myThreadPool);

                lock (task)
                {
                    _continueTasks.Enqueue(task.RunTask);
                }
                return task;
            }
    
            public void RunTask()
            {
                if (IsCompleted) return;
                try
                {
                    are.WaitOne();
                    Result = _taskFunction();
                    IsCompleted = true;
                    are.Set();
                    lock (_lockObject)
                    {
                        while (_continueTasks.TryDequeue(out var t))
                        {
                            t();
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new AggregateException($"Task failed with an exception {e}");
                }
            }
        }
    }
}