using ProjectForCustomTests;

namespace MyNUnitTests;

using NUnit.Framework;

public class Tests
{
    private readonly MyNUnit.MyNUnit _myNUnit = new();
    private const string PathForTestProject = "../../../../ProjectForTests1";

    [SetUp]
    public void Setup()
    {
        _myNUnit.RunAllByThePath(PathForTestProject);
    }
    
    [Test]
    public void TestForMethodsOrder()
    {
        var resultingCount = TestClass.Count;
        Assert.AreEqual(21, resultingCount);
    }

    [Test]
    public void TestsCancellingAfterOneOfThemFailed()
    {
        var countFromCanceled = CanceledTests.Count;
        Assert.IsTrue(countFromCanceled == 0);
    }
}


