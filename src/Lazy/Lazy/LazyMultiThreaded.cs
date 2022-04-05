namespace Lazy;

using System;

/// <summary>
/// Class, implementing multi-threaded lazy calculations for given function
/// </summary>
public class LazyMultiThreaded<T> : ILazy<T>
{
    private bool _isCalculated;
    private T _result;
    private Func<T> _supplier;
    private readonly object _lockObject = new();

    public LazyMultiThreaded(Func<T> supplier)
    {
        ArgumentNullException.ThrowIfNull(supplier);
        _supplier = supplier;
    }
        
    public T Get()
    {
        if (_isCalculated)
        {
            return _result;
        }
        lock (_lockObject)
        {
            if (_isCalculated)
            {
                return _result;
            }
            
            _result = _supplier();
            _supplier = null;
            _isCalculated = true;
        }
        return _result;
    }
}