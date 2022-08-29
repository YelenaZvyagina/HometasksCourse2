using System.Threading;
using MyNUnit;

namespace ProjectForCustomTests;

public class CanceledTests
{
    public static int Count = 0;

    [Before]
    public void CorrectMethod1() => Count = 10;

    [Test]
    public void IncorrectMethod() => Count /= 0;
        
    [AfterClass]
    public void AfterMethod() => Count = 1;
}
