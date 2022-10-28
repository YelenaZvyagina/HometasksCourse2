using MyNUnit;

namespace MyNunitWeb
{
    public static class Helper
    {
        public static string GetTableRowType(string state)
        {
            var testResult = (TestResult)Enum.Parse(typeof(TestResult), state);
            if (testResult == TestResult.Success)
            {
                return "table-success";
            }
            else if (testResult == TestResult.Failed)
            {
                return "table-danger";
            }
            else if (testResult == TestResult.Ignored)
            {
                return "table-primary";
            }
            else
            {
                return "table-warning";
            }
        }
    }
}
