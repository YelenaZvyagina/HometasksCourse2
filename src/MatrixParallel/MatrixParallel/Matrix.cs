namespace MatrixParallel;

using System;
using System.IO;
using System.Text;
using System.Threading;


/// <summary>
/// Class for matrix object and operations on it
/// </summary>
public class Matrix
{
    private readonly int _rows;
    private readonly int _columns;
    private readonly long[,] _matrix;
 
    /// <summary>
    /// Constructor for Matrix by given parameters
    /// </summary>
    private Matrix(int rows, int columns)
    {
        if (rows <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rows), "Number of rows should be positive");
        }
        if (columns <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(columns), "Number of columns should be positive");
        }
            
        var matrix = new long[rows, columns];
        _rows = rows;
        _columns = columns;
        _matrix = matrix;
    }

    /// <summary>
    /// 2dArray to an object "Matrix"
    /// </summary>
    public Matrix(long[,] matrix)
    {
        _rows = matrix.GetLength(0);
        _columns = matrix.GetLength(1);
        _matrix = matrix;
    }
 
    /// <summary>
    /// Generates a Matrix with random elements by given parameters 
    /// </summary>
    public static Matrix Generate(int rows, int columns)
    {
        var random = new Random();
        var resultingMatrix = new long[rows, columns];
            
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                resultingMatrix[i, j] = random.Next(50);
            }
        }
        return new Matrix(resultingMatrix);
    }
 
    /// <summary>
    /// Constructor that gets a matrix from given file
    /// </summary>
    public Matrix(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var standardLength = lines[0].TrimEnd(' ').Split(' ').Length;

        var matrix = new long[lines.Length, standardLength];
 
        for (var i = 0; i < lines.Length; i++)
        {
            var row = lines[i].TrimEnd(' ').Split(' ');
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
        _columns = standardLength;
        _matrix = matrix;
    }

    /// <summary>
    /// Writes Matrix to a file
    /// </summary>
    public void WriteMatrix(string filePath) 
    {
        var stringBuilder = new StringBuilder(_columns);
        for (var i = 0; i < _rows; i++)
        {
            for (var j = 0; j < _columns; j++)
            {
                stringBuilder.Append(_matrix[i, j] + " ");
            }
            stringBuilder.Append('\n');
        }
        File.WriteAllText(filePath, stringBuilder.ToString());
    }

    /// <summary>
    /// Multiplies given Matrix to the second one sequentially
    /// </summary>
    public Matrix MultiplySequentially(Matrix matrix)
    {
        if (_columns != matrix._rows) 
        {
            throw new ArgumentOutOfRangeException(nameof(matrix), "Matrices of these sizes cannot be multiplied");
        }
 
        var resultingMatrix = new Matrix(_rows, matrix._columns);
        for (var i = 0; i < _rows; i++)
        {
            for (var j = 0; j < matrix._columns; j++)
            {
                for (var l = 0; l < _columns; l++)
                {
                    resultingMatrix._matrix[i, j] += _matrix[i, l] * matrix._matrix[l, j];
                }
            }
        }
        return resultingMatrix;
    }
        
    /// <summary>
    /// Multiplies given Matrix to the second one paralleled
    /// </summary>
    public Matrix MultiplyParalleled(Matrix matrix)
    {
        if (_columns != matrix._rows) 
        {
            throw new ArgumentOutOfRangeException(nameof(matrix), "Matrices of these sizes cannot be multiplied");
        }
            
        var threads = new Thread[Math.Min(Environment.ProcessorCount, _rows)];
        var chunkSize = _rows / threads.Length + 1;
        var resultingMatrix = new Matrix(_rows, matrix._columns);
 
        for (var i = 0; i < threads.Length; i++)
        {
            var localI = i;
            threads[i] = new Thread(() =>
            {
                for (var j = localI * chunkSize; j < (localI + 1) * chunkSize && j < _rows; j++)
                {
                    for (var k = 0; k < matrix._columns; k++)
                    {
                        for (var l = 0; l < _columns; l++)
                        {
                            resultingMatrix._matrix[j, k] += _matrix[j, l] * matrix._matrix[l, k];
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
        return resultingMatrix;
    }
 
    /// <summary>
    /// Checks whether 2 matrices are equal
    /// </summary>
    public static bool MatricesEqual(Matrix m1, Matrix m2) 
    {
        var sizeEqual = m1._rows == m2._rows && m1._columns == m2._columns;
        var valuesEqual = true;
        for (var i = 0; i < m1._rows; i++)
        {
            for (var j = 0; j < m1._columns; j++)
            {
                if (m1._matrix[i, j] != m2._matrix[i, j])
                {
                    valuesEqual = false;
                }
            }
        }
        return sizeEqual && valuesEqual;
    }
}
