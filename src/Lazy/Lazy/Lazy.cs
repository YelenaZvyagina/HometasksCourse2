namespace Lazy;

using System;

/// <summary>
/// Class, implementing single-threaded lazy calculations for given function
/// </summary>
public class SingleThreadedLazy<T>: ILazy<T>
{
    private bool _isCalculated;
    private Func<T> _supplier;
    private T _result;
        
    public SingleThreadedLazy(Func<T> supplier)
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
        _result = _supplier();
        _isCalculated = true;
        _supplier = null;
        return _result;
    }
}

