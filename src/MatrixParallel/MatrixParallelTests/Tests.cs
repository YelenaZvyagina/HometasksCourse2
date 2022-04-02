namespace MatrixParallelTests;

using System;
using NUnit.Framework;
using MatrixParallel;

public class Tests
{
    [Test]
    public void MultipliesCorrectly()
    {
        long[,] rawMatrix1 =
        {
            { 1, 9 },
            { 12, 8 }
        };

        long[,] rawMatrix2 =
        {
            { 5, 6 },
            { 4, 15 }
        };

        long[,] rawMatrix3 =
        {
            { 41, 141 },
            { 92, 192 }
        };

        var matrix1 = new Matrix(rawMatrix1);
        var matrix2 = new Matrix(rawMatrix2);
        var expectedResult = new Matrix(rawMatrix3);

        var actualSequentialResult = matrix1.MultiplySequentially(matrix2);
        var actualParallelResult = matrix1.MultiplyParalleled(matrix2);

        Assert.IsTrue(Matrix.MatricesEqual(expectedResult, actualSequentialResult));
        Assert.IsTrue(Matrix.MatricesEqual(expectedResult, actualParallelResult));
    }

    [Test]
    public void TestsOnRandomMatrices()
    {
        var random = new Random();
        for (var i = 0; i < 50; i++)
        {
            var matrix1Rows = random.Next(20) + 1;
            var matrix1Cols = random.Next(20) + 1;
            var matrix2Rows = random.Next(20) + 1;
            var matrix1 = Matrix.Generate(matrix1Rows, matrix1Cols);
            var matrix2 = Matrix.Generate(matrix1Cols, matrix2Rows);

            var parallelResult = matrix1.MultiplyParalleled(matrix2);
            var sequentialResult = matrix1.MultiplySequentially(matrix2);

            Assert.IsTrue(Matrix.MatricesEqual(sequentialResult, parallelResult));
        }
    }

    [Test]
    public void MultiplicationExceptions()
    {
        var matrix1 = Matrix.Generate(5, 12);
        var matrix2 = Matrix.Generate(8, 10);
        var sequentialException = Assert.Throws<ArgumentOutOfRangeException>(() => matrix1.MultiplySequentially(matrix2));
        var parallelException = Assert.Throws<ArgumentOutOfRangeException>(() => matrix1.MultiplySequentially(matrix2));
        Assert.That(sequentialException is { Message: "Matrices of these sizes cannot be multiplied (Parameter 'matrix')" });
        Assert.That(parallelException is { Message: "Matrices of these sizes cannot be multiplied (Parameter 'matrix')" });
    }

    [Test]
    public void FilesFunctionsTests()
    {
        var matrix1 = Matrix.Generate(8, 8);
        var path = System.IO.Path.GetTempPath() + Guid.NewGuid() + ".txt";
        matrix1.WriteMatrix(path);
        var matrix2 = new Matrix(path);
        Assert.IsTrue(Matrix.MatricesEqual(matrix1, matrix2));
    }
}
