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
            private Func<TResult> _taskFunction;
            private AutoResetEvent are = new(false);
            private ConcurrentQueue<Action> _continueTasks = new();
            private object _lockObject = new();
            private readonly ManualResetEvent isCompletedEvent = new(false);
            private readonly object queueLockObject = new();

            public bool IsCompleted {get; set;}

            public TResult Result { 
                get
                {
                    isCompletedEvent.WaitOne();
                    return Result;
                }
                set => Result = value;
            }
            
            public MyTask(Func<TResult> func, MyThreadPool myThreadPool)
            {
                _taskFunction = func;
                _myThreadPool = myThreadPool;
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
    
            public void RunTask1() 
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

            public void RunTask()
            {
                try
                {
                    Result = _taskFunction();
                }
                catch (Exception exception)
                {
                    throw new AggregateException($"Task failed with an exception {exception}");
                }
                finally
                {
                    _taskFunction = null;

                    lock (queueLockObject)
                    {
                        IsCompleted = true;
                        isCompletedEvent.Set();

                        while (_continueTasks.Count != 0)
                        {
                            _continueTasks.TryDequeue(out var taska);
                            _myThreadPool.EnqueueTask(taska);
                        }
                    }
                }
            }
        }
    }
}