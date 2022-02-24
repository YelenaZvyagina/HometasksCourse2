using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Test3
{
    /// <summary>
    /// Blocking priority queue class
    /// </summary>
    public class BlockingQueue<T>
    {
        private SortedDictionary<int, Queue<T>> _queue = new SortedDictionary<int, Queue<T>>();
        private readonly object _locker = new object();

        /// <summary>
        /// Adds element with given priority
        /// </summary>
        public void Enqueue(int priority, T value)
        {
            lock (_locker)
            {
                if (_queue.ContainsKey(priority))
                {
                    _queue[priority].Enqueue(value);
                }
                else
                {
                    var q = new Queue<T>();
                    q.Enqueue(value);
                    _queue.Add(priority, q);
                }
                Monitor.PulseAll(_locker);
            }
        }

        /// <summary>
        /// returns the max priority's element and deletes it from queue 
        /// </summary>
        public T Dequeue()
        {
            lock (_locker)
            {
                if (_queue.Count == 0)
                {
                    Monitor.Wait(_locker);
                }
                
                var maxPriority = _queue.Keys.Last();
                var res = _queue[maxPriority].Dequeue();

                if (_queue[maxPriority].Count == 0)
                {
                    _queue.Remove(maxPriority);
                }
                return res;
            }
        }

        /// <summary>
        /// Size of queue
        /// </summary>
        public int Size()
        {
            int size;
            lock (_locker)
            {
                size = _queue.Count;
            }
            return size;
        }
    }
}

