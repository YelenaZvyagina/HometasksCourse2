using System.Collections.Concurrent;

namespace ThreadPool;

public class MyThreadPool
{
    private static readonly ConcurrentQueue<Action> TasksQueued = new();
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly AutoResetEvent _newTask = new(false);
    private readonly AutoResetEvent _are = new(false);
    private readonly object _lockObject = new();
    private readonly int _executingThreads;
    private readonly Thread[] _threads;

    public MyThreadPool(int threadCount)
    {
        if (threadCount < 1) throw new ArgumentException("Amount of threads should be positive");
        _threads = new Thread[threadCount]; 
        
        for (var i = 0; i < threadCount; ++i)
        {
            _threads[i] = new Thread(() =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    if (TasksQueued.TryDequeue(out Action action))
                    {
                        action();
                    }
                    else
                    {
                        _newTask.WaitOne();
                    }
                    if (!TasksQueued.IsEmpty)
                    {
                        _newTask.Set();
                    }
                }
            });
            _threads[i].Start();
            Interlocked.Increment(ref _executingThreads);
        }
    }
        
    private void EnqueueTask(Action task)
    {
        TasksQueued.Enqueue(task);
    }
        
    public IMyTask<TResult> Submit<TResult>(Func<TResult> function)
    {
        var task = new MyTask<TResult>(function, this);
        ArgumentNullException.ThrowIfNull(function);
        lock (_lockObject)
        {
            if (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                EnqueueTask(task.RunTask);
                _newTask.Set();
                return task;
            } 
        }
        throw new OperationCanceledException("ThreadPool is already shut down, sorry(");
    }
        
    public void ShutDown()
    {
        lock (_lockObject)
        {
            _cancellationTokenSource.Cancel();
        }
        while (_executingThreads != _threads.Length)
        {
            _newTask.Set();
        }
    }
    
    private class MyTask<TResult> : IMyTask<TResult> 
    {
        private static MyThreadPool _myThreadPool;
        private Func<TResult> _taskFunction;
        private readonly ManualResetEvent _taskCompleted = new(false);
        private object lockObject = new();
        private readonly Queue<Action> continueQueue = new();

        public bool IsCompleted {get; set;}

        private TResult _result;
        public TResult Result { 
            get
            {
                _taskCompleted.WaitOne();
                return _result;
            }
            set => _result = value;
        }

        public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func)
        {
            var newTask = new MyTask<TNewResult>(() => func(Result), _myThreadPool);
            lock (lockObject)
            {
                if (!IsCompleted)
                {
                    continueQueue.Enqueue(newTask.RunTask);
                }
                else
                {
                    _myThreadPool.EnqueueTask(newTask.RunTask);
                }
                return newTask;
            }
        }

        public MyTask(Func<TResult> func, MyThreadPool myThreadPool)
        {
            _taskFunction = func;
            _myThreadPool = myThreadPool;
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
                IsCompleted = true;
                _taskCompleted.Set();
                while (continueQueue.Count != 0)
                {
                    _myThreadPool.EnqueueTask(continueQueue.Dequeue());
                }
            }
        }
    }
}