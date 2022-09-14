using System;
using MyNUnit;

namespace ProjectForCustomTests;

public class IgnoredAndExpectedTests
{
    [Test("I don't want to run this test")]
    public void IgnoredTest() => throw new Exception();

    [Test(typeof(DivideByZeroException))]
    public void ThrowingMethod() => throw new DivideByZeroException();
}
