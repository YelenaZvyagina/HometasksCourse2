namespace MyNUnit;

/// <summary>
/// Determines state and properties of test
/// </summary>
public readonly struct TestState
{
    public readonly TestResult Result;
    public readonly string ErrorMessage;
    public readonly string TestName;
    public readonly long ExecutionTime;
    public readonly string IgnoreReason;

    public TestState(string testName, string ignoreReason)
    {
        TestName = testName;
        IgnoreReason = ignoreReason;
        ErrorMessage = "";
        ExecutionTime = 0;
        Result = TestResult.Ignored;
    }
    
    public TestState(string errorMessage, TestResult result, string testName, long executionTime)
    {
        Result = result;
        ErrorMessage = errorMessage;
        TestName = testName;
        ExecutionTime = executionTime;
        IgnoreReason = "";
    }

    /// <summary>
    /// Prints result of test execution
    /// </summary>
    public void PrintTestState()
    {
        switch (Result)
        {
            case TestResult.Canceled:
            { 
                Console.WriteLine($"{TestName} was canceled because some of previous tests had failed");
                break;
            }
            case TestResult.Failed:
            {
                Console.WriteLine($"{TestName} failed with {ErrorMessage}. Execution time {ExecutionTime}");
                break;
            }
            case TestResult.Success:
            {
                Console.WriteLine($"{TestName} was executed successfully! Execution time {ExecutionTime}");
                break;
            }
            case TestResult.Ignored:
            {
                Console.WriteLine($"Test {TestName} was ignored. Ignore reason: {IgnoreReason}");
                break;
            }
        }
    }
}