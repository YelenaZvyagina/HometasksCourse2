using System;
using Lazy;

namespace Lazy;

/// <summary>
/// Class for creating lazy objects
/// </summary>
public static class LazyFactory
{
    public static ILazy<T> CreateSingleThreadedLazy<T>(Func<T> supplier)
    {
        return new SingleThreadedLazy<T>(supplier);
    }

    public static ILazy<T> CreateMultiThreadedLazy<T>(Func<T> supplier)
    {
        return new LazyMultiThreaded<T>(supplier);
    }
}