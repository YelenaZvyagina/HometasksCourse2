namespace Lazy;

/// <summary>
/// Interface, describing classes for lazy calculations
/// </summary>
public interface ILazy<out T>
{
    /// <summary>
    /// Method to get the result of calculation
    /// </summary>
    T? Get();
}
