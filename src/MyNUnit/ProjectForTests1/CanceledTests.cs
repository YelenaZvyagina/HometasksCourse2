using MyNUnit;

namespace ProjectForTests1
{
    public class CanceledTests
    {
        public static int Count = 0;

        [Test]
        public void CorrectMethod1() => Count += 10;

        [Test]
        public void IncorrectMethod() => Count /= 0;
        
        [After]
        public void AfterMethod() => Count = 1;

        [Test]
        public void CorrectMethod2() => Count += 15;

        [Test]
        public void CorrectMethod3() => Count += 55;

    }
}