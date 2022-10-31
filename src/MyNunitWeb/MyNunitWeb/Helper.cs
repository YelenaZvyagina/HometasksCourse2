namespace MyNunitWeb;

using MyNUnit;

/// <summary>
/// Class for common used helping methods
/// </summary>
public static class Helper
{
    public static string GetTableRowType(string state)
    {
        var testResult = (TestResult)Enum.Parse(typeof(TestResult), state);
        switch (testResult)
        {
            case TestResult.Success: return "table-success";
            case TestResult.Failed: return "table-danger";
            case TestResult.Ignored: return "table-primary";
            default: return "table-warning";
        }
    }
}
