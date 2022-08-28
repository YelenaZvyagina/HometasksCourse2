namespace MyNUnit;

/// <summary>
/// Class for after attribute for tests
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class After : Attribute
{
}