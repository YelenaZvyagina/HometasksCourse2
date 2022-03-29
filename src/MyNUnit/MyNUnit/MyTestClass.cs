namespace MyNUnit

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


public class MyTestClass
{
    private readonly IEnumerable<MethodInfo> _testMethods;
    private readonly IEnumerable<MethodInfo> _afterClassMethods;
    private readonly IEnumerable<MethodInfo> _beforeClassMethods;
    private readonly IEnumerable<MethodInfo> _afterMethods;
    private readonly IEnumerable<MethodInfo> _beforeMethods;
    private bool _needsToStop;
    private readonly ConcurrentBag<TestState> _testStates = new();

    public MyTestClass(Type type)
    {
        _testMethods = type.GetMethods().Where(m => m.GetCustomAttributes(typeof(Test), false).Length > 0 && IsMethodOk(m, false)); 
        _afterMethods = type.GetMethods().Where(m => m.GetCustomAttributes(typeof(After), false).Length > 0 && IsMethodOk(m, false));
        _beforeMethods = type.GetMethods().Where(m => m.GetCustomAttributes(typeof(Before), false).Length > 0 && IsMethodOk(m, false));
        _afterClassMethods = type.GetMethods().Where(m => m.GetCustomAttributes(typeof(AfterClass), false).Length > 0 && IsMethodOk(m, true));
        _beforeClassMethods = type.GetMethods().Where(m => m.GetCustomAttributes(typeof(BeforeClass), false).Length > 0 && IsMethodOk(m, true));
    }

    private static bool IsMethodOk (MethodInfo methodInfo, bool shouldBeStatic)
    {
        var isVoidAndParameterless = methodInfo.ReturnType == typeof(void) && methodInfo.GetParameters().Length == 0;
        if (shouldBeStatic)
        {
            return isVoidAndParameterless && methodInfo.IsStatic;
        }
        return isVoidAndParameterless && !methodInfo.IsStatic;
    }
    
    public void RunTestClass()
    { 
        Parallel.ForEach(_beforeClassMethods, CheckAndExecute);
        Parallel.ForEach(_testMethods, RunTestMethod);
        Parallel.ForEach(_afterClassMethods, CheckAndExecute);
    }
    
    private void RunTestMethod(MethodInfo method)
    {
        Parallel.ForEach(_beforeMethods, CheckAndExecute);
        CheckAndExecute(method);
        Parallel.ForEach(_afterMethods, CheckAndExecute);
    }
    
    private void CheckAndExecute(MethodInfo method)
    {
        bool isTest = false;
        var attribute = method.GetCustomAttribute<Test>();
        if (attribute != null)
        {
            isTest = true;
            if (attribute.Ignore != null)
            { 
                _testStates.Add(new TestState(method.Name, attribute.Ignore)); 
                return;
            }
        }
        if (_needsToStop)
        {
            _testStates.Add(new TestState($"Test {method.Name} was canceled", TestResult.Canceled, method.Name, 0));
        }
        else
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                object obj = method.IsStatic ? null : Activator.CreateInstance(method.DeclaringType);
                method.Invoke(obj, null);
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                _testStates.Add(new TestState("", TestResult.Success, method.Name, elapsedMs));
            }
            catch (Exception exception)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                if (isTest)
                {
                    if (attribute.Expected == exception.GetType())
                    {
                        _testStates.Add(new TestState("", TestResult.Success, method.Name, elapsedMs ));
                    }
                }
                else
                {
                   _needsToStop = true;
                    _testStates.Add(new TestState(exception.Message, TestResult.Failed, method.Name, elapsedMs)); 
                }
            }
        }
    }

    public void PrintReport()
    {
        foreach (var test in _testStates)
        {
            test.PrintTestState();
        }
    }
}
