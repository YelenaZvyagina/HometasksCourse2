using System;
using System.Diagnostics;

namespace MatrixParallel
{
    public class Measurements
    {
        public static long timer (Matrix matrix1, Matrix matrix2, Func<Matrix, Matrix, Matrix> fun)
        {
            var watch = new Stopwatch();
            watch.Start();
            var res = fun (matrix1, matrix2);
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public static void Measure (Func<Matrix, Matrix, Matrix> fun)
        {
            int minsize = 100;
            int maxSize = 2000;
            int step = 100;
            int mesNum = 15;

            for (int size = minsize; size <= maxSize; size += step)
            {
                long timeCounted = 0;
                long variance = 0;

                for (int mes = 0; mes <= mesNum; mes++)
                {
                    Matrix m1 = Matrix.Generate(size, size);
                    Matrix m2 = Matrix.Generate(size, size);
                    var time = timer(m1, m2, fun);
                    timeCounted += time;
                    variance += time * time;
                }
                
                timeCounted /= mesNum;
                variance /= mesNum;
                variance -= timeCounted * timeCounted;
                Console.WriteLine($"Measurements on matrix {size}x{size}:");
                Console.WriteLine($"Average time: {(double)(timeCounted) / 1000} seconds, standart deviation: {Math.Round(Math.Sqrt(variance) / 1000, 5)} seconds\n");
                
            }
        }
    }
}