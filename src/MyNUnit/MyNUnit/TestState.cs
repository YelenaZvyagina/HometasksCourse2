namespace MyNUnit;

/// <summary>
/// Determines state and properties of test
/// </summary>
public readonly struct TestState
{
    private readonly TestResult _result;
    private readonly string _errorMessage;
    private readonly string _testName;
    private readonly long _executionTime;
    private readonly string _ignoreReason;

    public TestState(string testName, string ignoreReason)
    {
        _testName = testName;
        _ignoreReason = ignoreReason;
        _errorMessage = "";
        _executionTime = 0;
        _result = TestResult.Ignored;
    }
    
    public TestState(string errorMessage, TestResult result, string testName, long executionTime)
    {
        _result = result;
        _errorMessage = errorMessage;
        _testName = testName;
        _executionTime = executionTime;
        _ignoreReason = "";
    }

    /// <summary>
    /// Prints result of test execution
    /// </summary>
    public void PrintTestState()
    {
        switch (_result)
        {
            case TestResult.Canceled:
            { 
                Console.WriteLine($"{_testName} was canceled because some of previous tests had failed");
                break;
            }
            case TestResult.Failed:
            {
                Console.WriteLine($"{_testName} failed with {_errorMessage}. Execution time {_executionTime}");
                break;
            }
            case TestResult.Success:
            {
                Console.WriteLine($"{_testName} was executed successfully! Execution time {_executionTime}");
                break;
            }
            case TestResult.Ignored:
            {
                Console.WriteLine($"Test {_testName} was ignored. Ignore reason: {_ignoreReason}");
                break;
            }
        }
    }
}