namespace MyNUnit; 

using System;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class Test : Attribute
{
    public Type? Expected { get; }
    public string? Ignore { get; }

    public Test(string ignore) => Ignore = ignore;

    public Test(Type expected) => Expected = expected;

    public Test()
    {
    }
}
