using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
 
namespace MatrixParallel
{
    public class Matrix
    {
        private int Rows { get; set; }
        private int Cols { get; set; }
        private long[,] Matr { get; set; }
 
        public Matrix(int rows, int cols)
        {
            if (rows <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rows), "Number of rows should be positive");
            }
            if (cols <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cols), "Number of columns should be positive");
            }
            
            var matr = new long [rows, cols];
            Rows = rows;
            Cols = cols;
            Matr = matr;
        }

        public Matrix(long[,] matrix)
        {
            Rows = matrix.GetLength(0);
            Cols = matrix.GetLength(1);
            Matr = matrix;
        }
 
        public static Matrix Generate (int rows, int columns)
        {
            var random = new Random();
            var resMatrix = new long [rows, columns];
            
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    resMatrix[i, j] = random.Next(50);
                }
            }
            Matrix result = new Matrix(resMatrix);
            return result;
        }
 
        public Matrix (string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            int standLength = lines[0].TrimEnd(' ').Split(' ').Length;

            long[,] matrix = new long [lines.Length, standLength];
 
            for (int i = 0; i < lines.Length; i++)
            {
                var row1 = lines[i].TrimEnd(' ');
                var row = row1.Split(' ');
                for (int j = 0; j < row.Length; ++j)
                {
                    Int64.TryParse(row[j], out var n);
                    matrix[i, j] = n;
                }
            }
            Rows = lines.Length;
            Cols = standLength;
            Matr = matrix;
        }

        public void WriteMatrix (string filePath)
        {
            string [] strAr = new string [Rows];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    strAr[i] += Matr[i, j] + " ";
                }

                strAr[i].TrimEnd(' ');
            }
            File.WriteAllLines(filePath, strAr);
        }

        public Matrix SeqMatrixMult (Matrix matr)
        {
            if (Cols != matr.Rows) 
            {
                throw new ArgumentOutOfRangeException(nameof(matr), "Matrices of these sizes cannot be multiplied");
            }
 
            Matrix res = new Matrix(Rows, matr.Cols);
 
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < matr.Cols; j++)
                {
                    for (int l = 0; l < Cols; l++)
                    {
                        res.Matr[i, j] += Matr[i, l] * matr.Matr[l, j];
                    }
                }
            }
            return res;
        }
        
 
        public Matrix ParMatrMatrix(Matrix matr)
        {
            if (Cols != matr.Rows) 
            {
                throw new ArgumentOutOfRangeException(nameof(matr), "Matrices of these sizes cannot be multiplied");
            }
 
            var threads = new Thread[Environment.ProcessorCount];
            var chunkSize = Rows / threads.Length + 1;
            Matrix res = new Matrix(Rows, matr.Cols);
 
            for (int i = 0; i < threads.Length; i++)
            {
                var localI = i;
                threads[i] = new Thread(() =>
                {
                    for (int j = localI * chunkSize; j < (localI + 1) * chunkSize && j < Rows; j++)
                    {
                        for (int k = 0; k < matr.Cols; k++)
                        {
                            for (int l = 0; l < Cols; l++)
                            {
                                res.Matr[j, k] += Matr[j, l] * matr.Matr[l, k];
                            }
                        }
                    }
                } );
 
            }
 
            foreach (var thread in  threads)
            {
                thread.Start();
            }
 
            foreach (var thread in  threads)
            {
                thread.Join();
            }
 
            return res;
        }
 
        public static bool MatrEqual (Matrix m1, Matrix m2) 
        {
            bool sizeEqual = (m1.Rows == m2.Rows) && (m1.Cols == m2.Cols);
            bool valEqual = true;
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Cols; j++)
                {
                    if (m1.Matr[i, j] != m2.Matr[i, j] ) {valEqual = false;}
                }
            }
            return sizeEqual && valEqual;
        }
 
    }
}