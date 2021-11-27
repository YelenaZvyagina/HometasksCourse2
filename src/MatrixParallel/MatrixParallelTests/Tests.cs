using System;
using NUnit.Framework;
using MatrixParallel;
namespace MatrixParallelTests
{
    public class Tests
    {
        [Test]
        public void MultipliesCorrectly()
        {
            long[,] matrix1 =
            {
                { 1, 9 },
                { 12, 8 }
            };

            long[,] matrix2 =
            {
                { 5, 6 },
                { 4, 15 }
            };

            long[,] resMatrix =
            {
                { 41, 141 },
                { 92, 192 }
            };

            var m1 = new Matrix(matrix1);
            var m2 = new Matrix(matrix2);
            var expRes = new Matrix(resMatrix);

            var actResSeq = m1.SeqMatrixMult(m2);
            var actResPar = m1.ParMatrixMult(m2);

            Assert.IsTrue(Matrix.MatrixEqual(expRes, actResSeq));
            Assert.IsTrue(Matrix.MatrixEqual(expRes, actResPar));
        }

        [Test]
        public void TestsOnRandomMatrices()
        {
            var rand = new Random();
            for (var i = 0; i < 50; i++)
            {
                var matrix1Rows = rand.Next(20) + 1;
                var matrix1Cols = rand.Next(20) + 1;
                var matrix2Rows = rand.Next(20) + 1;
                var matrix1 = Matrix.Generate(matrix1Rows, matrix1Cols);
                var matrix2 = Matrix.Generate(matrix1Cols, matrix2Rows);

                var parRes = matrix1.ParMatrixMult(matrix2);
                var seqRes = matrix1.SeqMatrixMult(matrix2);

                Assert.IsTrue(Matrix.MatrixEqual(seqRes, parRes));
            }
        }

        [Test]
        public void MultiplicationExceptions()
        {
            var matrix1 = Matrix.Generate(5, 12);
            var matrix2 = Matrix.Generate(8, 10);
            var exSeq = Assert.Throws<ArgumentOutOfRangeException>(() => matrix1.SeqMatrixMult(matrix2));
            var exPar = Assert.Throws<ArgumentOutOfRangeException>(() => matrix1.SeqMatrixMult(matrix2));
            Assert.That(exSeq is { Message: "Matrices of these sizes cannot be multiplied (Parameter 'matrix')" });
            Assert.That(exPar is { Message: "Matrices of these sizes cannot be multiplied (Parameter 'matrix')" });
        }

        [Test]
        public void FilesFunctionsTests()
        {
            var matrix1 = Matrix.Generate(8, 8);
            var path = System.IO.Path.GetTempPath() + Guid.NewGuid() + ".txt";
            matrix1.WriteMatrix(path);
            var matrix2 = new Matrix(path);
            Assert.IsTrue(Matrix.MatrixEqual(matrix1, matrix2));
        }
    }
}