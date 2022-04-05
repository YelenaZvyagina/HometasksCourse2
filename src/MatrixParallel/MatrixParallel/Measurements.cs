namespace MatrixParallel;

using System;
using System.Diagnostics;


/// <summary>
/// Class for functions for measuring
/// </summary>
public static class Measurements
{
    /// <summary>
    /// Calculates the time of multiplication 2 matrices the given way
    /// </summary>
    private static long Timer(Matrix matrix1, Matrix matrix2, Func<Matrix, Matrix, Matrix> multiplyingFunction)
    {
        var watch = new Stopwatch();
        watch.Start();
        multiplyingFunction(matrix1, matrix2);
        watch.Stop();
        return watch.ElapsedMilliseconds;
    }
        
    /// <summary>
    /// Measures average time and variance and prints the result to console
    /// </summary>
    public static void Measure(Func<Matrix, Matrix, Matrix> multiplyingFunction)
    {
        const int minSize = 100;
        const int maxSize = 2100;
        const int step = 500;
        const int numberOfMeasurements = 10;

        for (var size = minSize; size <= maxSize; size += step)
        {
            var timeCounted = 0L;
            var variance = 0L;

            for (var measurement = 1; measurement <= numberOfMeasurements; measurement++)
            {
                var matrix1 = Matrix.Generate(size, size);
                var matrix2 = Matrix.Generate(size, size);
                var time = Timer(matrix1, matrix2, multiplyingFunction);
                timeCounted += time;
                variance += time * time;
            }
                
            timeCounted /= numberOfMeasurements;
            variance /= numberOfMeasurements;
            variance -= timeCounted * timeCounted;
                
            Console.WriteLine($"Measurements on matrix {size}x{size}:");
            Console.WriteLine($"Average time: {(double)timeCounted / 1000} seconds, standard deviation: {Math.Round(Math.Sqrt(variance) / 1000, 5)} seconds\n");
        }
    }
}
