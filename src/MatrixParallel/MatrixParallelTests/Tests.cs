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

            long[,] resMatr =
            {
                { 41, 141 },
                { 92, 192 }
            };

            Matrix m1 = new Matrix(matrix1);
            Matrix m2 = new Matrix(matrix2);
            Matrix expRes = new Matrix(resMatr);

            Matrix actResSeq = m1.SeqMatrixMult(m2);
            Matrix actResPar = m1.ParMatrMatrix(m2);

            Assert.IsTrue(Matrix.MatrEqual(expRes, actResSeq));
            Assert.IsTrue(Matrix.MatrEqual(expRes, actResPar));
        }

        [Test]
        public void TestsOnRandomMatrices()
        {
            var rand = new Random();
            for (int i = 0; i < 50; i++)
            {
                var m1R = rand.Next(20) + 1;
                var m1C = rand.Next(20) + 1;
                var m2R = rand.Next(20) + 1;
                Matrix m1 = Matrix.Generate(m1R, m1C);
                Matrix m2 = Matrix.Generate(m1C, m2R);

                Matrix parRes = m1.ParMatrMatrix(m2);
                Matrix seqRes = m1.SeqMatrixMult(m2);

                Assert.IsTrue(Matrix.MatrEqual(seqRes, parRes));

            }
        }

        [Test]
        public void MultiplicationExceptions()
        {
            Matrix m1 = Matrix.Generate(5, 12);
            Matrix m2 = Matrix.Generate(8, 10);
            var exSeq = Assert.Throws<ArgumentOutOfRangeException>(() => m1.SeqMatrixMult(m2));
            var exPar = Assert.Throws<ArgumentOutOfRangeException>(() => m1.SeqMatrixMult(m2));
            Assert.That(exSeq is { Message: "Matrices of these sizes cannot be multiplied (Parameter 'matr')" });
            Assert.That(exPar is { Message: "Matrices of these sizes cannot be multiplied (Parameter 'matr')" });
        }

        [Test]
        public void FilesFunctionsTests()
        {
            Matrix m1 = Matrix.Generate(8, 8);
            string path = System.IO.Path.GetTempPath() + Guid.NewGuid() + ".txt";
            m1.WriteMatrix(path);
            Matrix m2 = new Matrix(path);
            Assert.IsTrue(Matrix.MatrEqual(m1, m2));
        }
    }
}