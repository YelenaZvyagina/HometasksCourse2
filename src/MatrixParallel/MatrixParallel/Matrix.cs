using System;
using System.IO;
using System.Text;
using System.Threading;

namespace MatrixParallel
{
    /// <summary>
    /// Class for matrix object and operations on it
    /// </summary>
    public class Matrix
    {
        private readonly int _rows;
        private readonly int _cols;
        private readonly long[,] _matrix;
 
        /// <summary>
        /// Constructor for Matrix by given parameters
        /// </summary>
        private Matrix(int rows, int cols)
        {
            if (rows <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rows), "Number of rows should be positive");
            }
            if (cols <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cols), "Number of columns should be positive");
            }
            
            var matrix = new long[rows, cols];
            _rows = rows;
            _cols = cols;
            _matrix = matrix;
        }

        /// <summary>
        /// 2dArray to an object "Matrix"
        /// </summary>
        public Matrix(long[,] matrix)
        {
            _rows = matrix.GetLength(0);
            _cols = matrix.GetLength(1);
            _matrix = matrix;
        }
 
        /// <summary>
        /// Generates a Matrix with random elements by given parameters 
        /// </summary>
        public static Matrix Generate(int rows, int columns)
        {
            var random = new Random();
            var resMatrix = new long[rows, columns];
            
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    resMatrix[i, j] = random.Next(50);
                }
            }
            return new Matrix(resMatrix);
        }
 
        /// <summary>
        /// Constructor that gets a matrix from given file
        /// </summary>
        public Matrix(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var standLength = lines[0].TrimEnd(' ').Split(' ').Length;

            var matrix = new long[lines.Length, standLength];
 
            for (var i = 0; i < lines.Length; i++)
            {
                var row1 = lines[i].TrimEnd(' ');
                var row = row1.Split(' ');
                for (var j = 0; j < row.Length; ++j)
                {
                    if (!long.TryParse(row[j], out var n))
                    {
                        throw new InvalidDataException("Matrix in a file should be comprised of integers");
                    }
                    matrix[i, j] = n;
                }
            } 
            _rows = lines.Length;
            _cols = standLength;
            _matrix = matrix;
        }

        /// <summary>
        /// Writes Matrix to a file
        /// </summary>
        public void WriteMatrix(string filePath) 
        {
            var stringBuilder = new StringBuilder(_cols);
            for (var i = 0; i < _rows; i++)
            {
                for (var j = 0; j < _cols; j++)
                {
                    stringBuilder.Append(_matrix[i, j]+ " ");
                }
                stringBuilder.Append('\n');
            }
            File.WriteAllText(filePath,stringBuilder.ToString());
        }

        /// <summary>
        /// Multiplies given Matrix to the second one sequentially
        /// </summary>
        public Matrix SeqMatrixMult(Matrix matrix)
        {
            if (_cols != matrix._rows) 
            {
                throw new ArgumentOutOfRangeException(nameof(matrix), "Matrices of these sizes cannot be multiplied");
            }
 
            var res = new Matrix(_rows, matrix._cols);
            for (var i = 0; i < _rows; i++)
            {
                for (var j = 0; j < matrix._cols; j++)
                {
                    for (var l = 0; l < _cols; l++)
                    {
                        res._matrix[i, j] += _matrix[i, l] * matrix._matrix[l, j];
                    }
                }
            }
            return res;
        }
        
        /// <summary>
        /// Multiplies given Matrix to the second one paralleled
        /// </summary>
        public Matrix ParMatrixMult(Matrix matrix)
        {
            if (_cols != matrix._rows) 
            {
                throw new ArgumentOutOfRangeException(nameof(matrix), "Matrices of these sizes cannot be multiplied");
            }
            
            var threads = new Thread[Math.Min(Environment.ProcessorCount, _rows)];
            var chunkSize = _rows / threads.Length + 1;
            var res = new Matrix(_rows, matrix._cols);
 
            for (var i = 0; i < threads.Length; i++)
            {
                var localI = i;
                threads[i] = new Thread(() =>
                {
                    for (var j = localI * chunkSize; j < (localI + 1) * chunkSize && j < _rows; j++)
                    {
                        for (var k = 0; k < matrix._cols; k++)
                        {
                            for (var l = 0; l < _cols; l++)
                            {
                                res._matrix[j, k] += _matrix[j, l] * matrix._matrix[l, k];
                            }
                        }
                    }
                });
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
 
        /// <summary>
        /// Checks whether 2 matrices are equal
        /// </summary>
        public static bool MatrixEqual(Matrix m1, Matrix m2) 
        {
            var sizeEqual = m1._rows == m2._rows && m1._cols == m2._cols;
            var valEqual = true;
            for (var i = 0; i < m1._rows; i++)
            {
                for (var j = 0; j < m1._cols; j++)
                {
                    if (m1._matrix[i, j] != m2._matrix[i, j])
                    {
                        valEqual = false;
                    }
                }
            }
            return sizeEqual && valEqual;
        }
    }
}