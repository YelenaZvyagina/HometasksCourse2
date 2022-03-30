namespace MyNUnit;

using System;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class Before : Attribute
{
    public Before() {}
}
