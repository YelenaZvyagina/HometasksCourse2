namespace MyNUnit;

using System;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class After : Attribute
{
    public After() {} 
}