using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyNUnit;
namespace MyNunitWeb.Pages;

using MyNunitWeb.Data;
using MyNunitWeb.Models;

public class HistoryModel : PageModel
{
    public List<TestModel> TestResults { get; set; }
    public List<AssemblyModel> TestsAssemblies { get; set; }

    private readonly ApplicationDbContext _dbContext;

    public HistoryModel(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void OnGet()
    {
        TestResults = _dbContext.TestResults.ToList();
        TestsAssemblies = _dbContext.TestAssemblies.ToList();

        foreach (var assembly in TestsAssemblies)
        {
            assembly.Tests = TestResults.Where(test => test.AssemblyModelId == assembly.Id).ToList();
        }
    }

    public int GetAmountOfTestsOfResult(AssemblyModel assembly, TestResult testResult)
    {
        return assembly.Tests.Where(test => test.Result.Equals(testResult.ToString())).Count();
    }
}
