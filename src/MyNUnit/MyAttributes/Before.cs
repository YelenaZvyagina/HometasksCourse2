namespace MyNUnit;

/// <summary>
/// Class for before attribute for tests
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class Before : Attribute
{
}
