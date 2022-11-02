namespace ThreadPool;

/// <summary>
/// Interface describing task
/// </summary>
public interface IMyTask<TResult>
{
    /// <summary>
    /// Completion status of a task
    /// </summary>
    bool IsCompleted { get; set; } 
    
    /// <summary>
    /// Task result
    /// </summary>
    TResult Result { get; } 
    
    /// <summary>
    /// Continues task using result of previous one
    /// </summary>
    IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func);
}