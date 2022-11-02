namespace ThreadPool;

using System.Collections.Concurrent;

/// <summary>
/// Custom threadpool for parallel task executing
/// </summary>
public class MyThreadPool
{
    private static readonly ConcurrentQueue<Action> TasksQueued = new();
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly AutoResetEvent _newTask = new(false);
    private readonly AutoResetEvent _taskDone = new(false);
    private readonly object _lockObject = new();
    private int _doneThreads;
    private readonly Thread[] _threads;

    public MyThreadPool(int threadCount)
    {
        if (threadCount < 1)
        {
            throw new ArgumentException("Amount of threads should be positive");
        }
        _threads = new Thread[threadCount]; 
        
        for (var i = 0; i < threadCount; i++)
        {
            _threads[i] = new Thread(() =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested || !TasksQueued.IsEmpty)
                {
                    if (TasksQueued.TryDequeue(out var action))
                    {
                        _newTask.Set();
                        action();
                    }
                    else
                    {
                        _newTask.WaitOne();
                    }
                }
                Interlocked.Increment(ref _doneThreads);
                _taskDone.Set();
            });
            _threads[i].Start();
        }
    }
    
    /// <summary>
    /// Adds task to a queue
    /// </summary>
    private void EnqueueTask(Action task)
    {
        TasksQueued.Enqueue(task);
        _newTask.Set();
    }
        
    /// <summary>
    /// Submitting task to execute on threadpool
    /// </summary>
    public IMyTask<TResult> Submit<TResult>(Func<TResult> function)
    {
        ArgumentNullException.ThrowIfNull(function);
        var task = new MyTask<TResult>(function, this);
        lock (_lockObject)
        {
            if (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                EnqueueTask(task.RunTask);
                return task;
            } 
        }
        throw new OperationCanceledException("ThreadPool is already shut down, sorry");
    }
        
    /// <summary>
    /// Stops threadpool work
    /// </summary>
    public void ShutDown()
    {
        lock (_lockObject)
        {
            _cancellationTokenSource.Cancel();
        }
        while (_doneThreads != _threads.Length)
        {
            _newTask.Set();
            _taskDone.WaitOne();
        }
    }
    
    /// <summary>
    /// Task executing in threadpool
    /// </summary>
    private class MyTask<TResult> : IMyTask<TResult> 
    {
        private static MyThreadPool _myThreadPool;
        private Func<TResult> _taskFunction;
        private readonly ManualResetEvent _taskCompleted = new(false);
        private readonly object lockObject = new();
        private readonly Queue<Action> _continueQueue = new();
        private Exception _exception;

        public bool IsCompleted {get; set;}

        private TResult _result;
        public TResult Result { 
            get
            {
                _taskCompleted.WaitOne();
                if (_exception != null)
                {
                    throw new AggregateException($"Task failed: {_exception}");
                }
                return _result;
            } 
            set
            {
                _result = value;
            } 
        }

        /// <summary>
        /// For executing tasks one by one, using result of previous task in the next one
        /// </summary>
        public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func)
        {
            var newTask = new MyTask<TNewResult>(() => func(Result), _myThreadPool);
            lock (lockObject)
            {
                if (!IsCompleted)
                {
                    _continueQueue.Enqueue(newTask.RunTask);
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
            
        /// <summary>
        /// Executes task
        /// </summary>
        public void RunTask()
        {
            try
            {
                Result = _taskFunction();
            }
            catch (Exception exception)
            {
                _exception = new AggregateException(exception);
            }
            finally
            {
                lock (lockObject)
                {
                    _taskFunction = null;
                    IsCompleted = true;
                    _taskCompleted.Set();
                    while (_continueQueue.Count != 0)
                    {
                        _myThreadPool.EnqueueTask(_continueQueue.Dequeue());
                    }
                }
            }
        }
    }
}