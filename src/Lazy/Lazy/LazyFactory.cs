namespace Lazy;

using System;

/// <summary>
/// Class for creating lazy objects
/// </summary>
public static class LazyFactory
{
    /// <summary>
    /// Method that creates an instance of Lazy class
    /// </summary>
    public static ILazy<T> CreateSingleThreadedLazy<T>(Func<T> supplier)
        => new SingleThreadedLazy<T>(supplier);

    /// <summary>
    /// Method that creates an instance of LazyMultiThreaded class
    /// </summary>
    public static ILazy<T> CreateMultiThreadedLazy<T>(Func<T> supplier) 
        => new LazyMultiThreaded<T>(supplier);
    
}