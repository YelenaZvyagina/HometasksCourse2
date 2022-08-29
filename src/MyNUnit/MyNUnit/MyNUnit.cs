using System.Collections.Concurrent;
using System.Reflection;

namespace MyNUnit;

/// <summary>
/// Simple utility for launching custom tests
/// </summary>
public class MyNUnit
{
    private static IEnumerable<Assembly> GetAssemblies(string path)
    {
        var paths = Directory.GetFileSystemEntries(path, "*.dll", SearchOption.AllDirectories);
        var assemblies = new ConcurrentBag<Assembly>();
        Parallel.ForEach(paths, p =>
        {
            assemblies.Add(Assembly.LoadFrom(p));
        });
        return assemblies;
    }
    
    /// <summary>
    /// Runs all tests in specified directory
    /// </summary>
    public void RunAllByThePath(string path)
    {
        var types = GetAssemblies(path).SelectMany(a => a.GetTypes()).ToList();
        if (!types.Any())
        {
            Console.WriteLine("There are no assemblies to run in this search path");
            return;
        }
        Parallel.ForEach(types, RunMethodsInOneClass);
    }

    private static void RunMethodsInOneClass(Type type)
    {
        MyTestClass myTestClass = new(type);
        myTestClass.RunTestClass();
    }
}
