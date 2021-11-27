using System;

namespace MatrixParallel
{
    internal static class Program
    {
        /// <summary>
        /// Reads 2 matrices from text files and prints result of multiplication to new file
        /// </summary>
        private static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Expected arguments: path to the first matrix file, path to the second matrix file");
                return;
            }

            var matrix1 = new Matrix(args[0]);
            var matrix2 = new Matrix(args[1]);
            var result = matrix1.ParMatrixMult(matrix2);
            result.WriteMatrix("../../../Result.txt");
        }
    }
}