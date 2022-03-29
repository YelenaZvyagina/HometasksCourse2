namespace ProjectForTests1;

using System;
using MyNUnit;


public class IgnoredAndExpectedTests
{
    public static int CountForIgnore = 1;

    [Test("I don't want to run this test")]
    public void IgnoredTest() => CountForIgnore *= 100000;

    [Test]
    public void NotIgnoredTest() => CountForIgnore += 1;

    [Test(typeof(DivideByZeroException))]
    public void ThrowingMethod() => CountForIgnore /= 0;

    [AfterClass]
    public static void AfterClassMethod() => CountForIgnore *= 2;
}
