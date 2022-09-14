namespace MyNUnit; 

/// <summary>
/// Class for test attribute for tests
/// </summary>
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
