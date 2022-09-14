namespace MyNUnit;

/// <summary>
/// Class for afterclass attribute for tests
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class AfterClass : Attribute
{
}