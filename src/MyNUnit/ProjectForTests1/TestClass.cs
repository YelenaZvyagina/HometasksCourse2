using MyNUnit;

namespace ProjectForTests1
{
    public class TestClass
    {
        public static int Count;

        [BeforeClass]
        public static void BeforeClassMethod() => Count = 101;

        [Before]
        public void BeforeMethod() => Count += 1;

        [After]
        public void AfterMethod() => Count -= 2;

        [Test]
        public void JustTest1() => Count += 2;

        [Test]
        public void JustTest2() => Count += 4;

        [AfterClass]
        public static void AfterClassMethod() => Count /= 5; 
    }
}
