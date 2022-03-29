namespace MyNUnit;
    
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SearchOption = System.IO.SearchOption;
    
public class MyNUnit
{
    private static IEnumerable<Assembly> GetAssemblies(string path)
    {
        var paths = Directory.GetFileSystemEntries(path, "*.dll", SearchOption.AllDirectories);
        return paths.Select(Assembly.LoadFrom).ToList();
    }
    
    public void RunAllByThePath(string path)
    {
        var types = GetAssemblies(path).SelectMany(a => a.GetTypes()).ToList();
        if (!types.Any())
        {
            Console.WriteLine("There are no assemblies to run in this search path");
            return;
        }
        Parallel.ForEach(types, RunMethodsInOneCLass);
    }

    private static void RunMethodsInOneCLass(Type type)
    {
        MyTestClass myTestClass = new(type);
        myTestClass.RunTestClass();
        myTestClass.PrintReport();
    }
}
