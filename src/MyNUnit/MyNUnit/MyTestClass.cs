using System.Collections.Concurrent;
using System.Reflection;

namespace MyNUnit;

/// <summary>
/// Processes custom tests from one testclass
/// </summary>
public class MyTestClass
{
    private readonly IEnumerable<MethodInfo> _testMethods;
    private readonly IEnumerable<MethodInfo> _afterClassMethods;
    private readonly IEnumerable<MethodInfo> _beforeClassMethods;
    private readonly IEnumerable<MethodInfo> _afterMethods;
    private readonly IEnumerable<MethodInfo> _beforeMethods;
    private bool _needsToStop;
    private readonly ConcurrentBag<TestState> _testStates = new();
    private readonly Type _classType;

    public MyTestClass(Type type)
    {
        _classType = type;
        _testMethods = type.GetMethods()
            .Where(m => m.GetCustomAttributes(typeof(Test), false).Length > 0 && IsMethodOk(m, false));
        _afterMethods = type.GetMethods()
            .Where(m => m.GetCustomAttributes(typeof(After), false).Length > 0 && IsMethodOk(m, false));
        _beforeMethods = type.GetMethods()
            .Where(m => m.GetCustomAttributes(typeof(Before), false).Length > 0 && IsMethodOk(m, false));
        _afterClassMethods = type.GetMethods().Where(m =>
            m.GetCustomAttributes(typeof(AfterClass), false).Length > 0 && IsMethodOk(m, true));
        _beforeClassMethods = type.GetMethods().Where(m =>
            m.GetCustomAttributes(typeof(BeforeClass), false).Length > 0 && IsMethodOk(m, true));
    }

    private static bool IsMethodOk(MethodInfo methodInfo, bool shouldBeStatic)
    {
        var isVoidAndParameterless = methodInfo.ReturnType == typeof(void) && methodInfo.GetParameters().Length == 0;
        if (shouldBeStatic) return isVoidAndParameterless && methodInfo.IsStatic;
        return isVoidAndParameterless && !methodInfo.IsStatic;
    }

    /// <summary>
    /// Runs all methods in this testclass
    /// </summary>
    public void RunTestClass()
    {
        foreach (var beforeClassMethod in _beforeClassMethods) 
        {
            CheckAndExecute(beforeClassMethod);
        }
        
        Parallel.ForEach(_testMethods, RunTestMethod);
        
        foreach (var afterClassMethod in _afterClassMethods) 
        {
            CheckAndExecute(afterClassMethod);
        }
        
        foreach (var test in _testStates) 
        {
            test.PrintTestState();
        }
    }

    private void RunTestMethod(MethodInfo method)
    {
        Parallel.ForEach(_beforeMethods, CheckAndExecute);
        CheckAndExecute(method);
        Parallel.ForEach(_afterMethods, CheckAndExecute);
    }

    private void CheckAndExecute(MethodInfo method)
    {
        var isTest = false;
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
                var obj = method.IsStatic ? null : Activator.CreateInstance(_classType);
                method.Invoke(obj, null);
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                if (attribute != null && attribute.Expected != null)
                {
                    _testStates.Add(new TestState($"Expected {attribute.Expected} exception", TestResult.Failed, method.Name, elapsedMs));
                }
                
                _testStates.Add(new TestState("", TestResult.Success, method.Name, elapsedMs));
            }
            catch (Exception exception)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                if (isTest)
                {
                    if (attribute != null && attribute.Expected == exception.GetType())
                        _testStates.Add(new TestState("", TestResult.Success, method.Name, elapsedMs));
                }
                else
                {
                    _needsToStop = true;
                    _testStates.Add(new TestState(exception.Message, TestResult.Failed, method.Name, elapsedMs));
                }
            }
        }
    }

    /// <summary>
    /// prints report about all runned tests in this testclass
    /// </summary>
    public void PrintReport()
    {
        foreach (var test in _testStates) 
        {
            test.PrintTestState();
        }
    }
}