using System;

namespace Lazy
{
    /// <summary>
    /// Class, implementing multi-threaded lazy calculations for given function
    /// </summary>
    public class LazyMultiThreaded<T> : ILazy<T>
    {
        private bool _isCalculated;
        private T? _result;
        private readonly Func<T> _supplier;
        private readonly object _lockObject = new();

        public LazyMultiThreaded(Func<T> supplier)
        {
            ArgumentNullException.ThrowIfNull(supplier);
            _supplier = supplier;
        }
        
        public T Get()
        {
            if (_isCalculated) return _result;
            lock (_lockObject)
            {
                if(_isCalculated) return _result;
                _isCalculated = true;
                _result = _supplier();
            }
            return _result;
        }
    }
}