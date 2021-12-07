using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;

namespace Test2
{
    internal static class Program 
    {
        /// <summary>
        /// Comparing which version is faster, printing hash
        /// </summary>
        public static void Main(string [] args)
        {
            var filePath = args[0];

            string Compare(string filepath)
            {
                var timer = new Stopwatch();
                timer.Start();
                CheckSum.CheckSumSimple(filePath);
                timer.Stop();
                var singleThreaded = timer.ElapsedMilliseconds;
                var singleRes = BitConverter.ToString(CheckSum.CheckSumSimple(filePath));
            
                timer.Start();
                var a = CheckSum.CheckSumMultiThreaded(filePath);
                a.Wait();
                timer.Stop();
                var multithreading = timer.ElapsedMilliseconds;

                return singleThreaded > multithreading ? $"SingleThreaded realization is quicker, Hash is {singleRes}" : $"Multithreading realization is quicker";
            }
            
            Compare(filePath);
        }
    }
}


