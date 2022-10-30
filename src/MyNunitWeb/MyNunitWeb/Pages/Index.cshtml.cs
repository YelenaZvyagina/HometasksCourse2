namespace MyNunitWeb.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyNunitWeb.Models;
using MyNUnit;
using System.Collections.Concurrent;
using MyNunitWeb.Data;

public class IndexModel : PageModel
{
    private readonly IWebHostEnvironment _environment;
    private readonly ApplicationDbContext _dbContext;

    [BindProperty]
    public List<string> LoadedFilePaths { get; set; } = new List<string>();

    public List<AssemblyModel> Assemblies { get; set; } = new List<AssemblyModel>();

    public IndexModel(IWebHostEnvironment environment, ApplicationDbContext dbContext)
    {
        _environment = environment;
        _dbContext = dbContext;
    }

    public void OnGet()
    {
    }

    public void OnPost(IFormFile[] dlls)
    {
        if (dlls != null && dlls.Length > 0)
        {
            foreach (IFormFile dll in dlls)
            {
                var path = Path.Combine(_environment.WebRootPath, "dlls", dll.FileName);
                using var stream = new FileStream(path, FileMode.Create);
                dll.CopyToAsync(stream);
                LoadedFilePaths.Add(path);
            }
        }
        return;
    }

    public void OnPostStartTesting()
    {
        ProcessTests();
        SaveResultsToDb();
    }

    private void ProcessTests()
    {
        var testAndAssemblies = RunTestsWithMyNuint(LoadedFilePaths);
        foreach (var assembly in testAndAssemblies)
        {
            var tests = new List<TestModel>();
            foreach (var test in assembly.Value.ToList())
            {
                tests.Add
                (
                    new TestModel
                    {
                        Name = test.TestName,
                        IgnoreReason = !string.IsNullOrEmpty(test.ErrorMessage) ? test.ErrorMessage : test.IgnoreReason,
                        ExecutionTime = test.ExecutionTime,
                        Result = test.Result.ToString()
                    }
                );
            }
            Assemblies.Add(new AssemblyModel { Name = assembly.Key, Tests = tests });
        }
    }

    private Dictionary<string, ConcurrentBag<TestState>> RunTestsWithMyNuint(List<string> testPaths)
    {
        var myNUnit = new MyNUnit();
        return myNUnit.GetTestResultsAndAssembliesByPath(testPaths);
    }

    private void SaveResultsToDb()
    {
        _dbContext.TestAssemblies.AddRange(Assemblies);
        foreach (var assembly in Assemblies)
        {
            _dbContext.TestResults.AddRange(assembly.Tests);
        }
        _dbContext.SaveChanges();
    }
}