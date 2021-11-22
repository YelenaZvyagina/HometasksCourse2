using System;
using System.Diagnostics;

namespace MatrixParallel
{
    /// <summary>
    /// Class for functions for measuring
    /// </summary>
    public static class Measurements
    {
        /// <summary>
        /// Calculates the time of multiplication 2 matrices the given way
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        private static long Timer (Matrix matrix1, Matrix matrix2, Func<Matrix, Matrix, Matrix> fun)
        {
            var watch = new Stopwatch();
            watch.Start();
            fun (matrix1, matrix2);
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
        
        /// <summary>
        /// Measures average time and variance and prints the result to console
        /// </summary>
        /// <param name="fun"></param>
        public static void Measure (Func<Matrix, Matrix, Matrix> fun)
        {
            const int minsize = 100;
            var maxSize = 2100;
            var step = 500;
            var mesNum = 10;

            for (int size = minsize; size <= maxSize; size += step)
            {
                long timeCounted = 0;
                long variance = 0;

                for (int mes = 1; mes <= mesNum; mes++)
                {
                    Matrix m1 = Matrix.Generate(size, size);
                    Matrix m2 = Matrix.Generate(size, size);
                    long time = Timer(m1, m2, fun);
                    timeCounted += time;
                    variance += time * time;
                }
                
                timeCounted /= mesNum;
                variance /= mesNum;
                variance -= timeCounted * timeCounted;
                
                Console.WriteLine($"Measurements on matrix {size}x{size}:");
                Console.WriteLine($"Average time: {(double)timeCounted / 1000} seconds, standard deviation: {Math.Round(Math.Sqrt(variance) / 1000, 5)} seconds\n");
            }
        }
    }
}