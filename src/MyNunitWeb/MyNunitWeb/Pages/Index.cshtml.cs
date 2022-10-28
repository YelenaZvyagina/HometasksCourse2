using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyNunitWeb.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using MyNUnit;
using System.Collections.Concurrent;
using MyNunitWeb.Data;

namespace MyNunitWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IHostingEnvironment _environment;
        private ApplicationDbContext _dbContext;

        [BindProperty]
        public List<string> LoadedFilePaths { get; set; } = new List<string>();

        public List<AssemblyModel> Assemblies { get; set; } = new List<AssemblyModel>();

        public IndexModel(ILogger<IndexModel> logger, IHostingEnvironment environment, ApplicationDbContext dbContext)
        {
            _logger = logger;
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
            foreach(var a in testAndAssemblies)
            {
                var tests = new List<TestModel>();
                foreach (var test in a.Value.ToList())
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
                Assemblies.Add
                ( new AssemblyModel { Name = a.Key, Tests = tests} );
            }
        }
    
        private Dictionary<string, ConcurrentBag<TestState>> RunTestsWithMyNuint(List<string> testPaths)
        {
            var myNUnit = new MyNUnit.MyNUnit();
            return myNUnit.GetTestResultsAndAssembliesByPath(testPaths);
        }

        private void SaveResultsToDb()
        {
            _dbContext.TestAssmblies.AddRange(Assemblies);
            foreach( var assembly in Assemblies)
            {
                _dbContext.TestResults.AddRange(assembly.Tests);
            }
            _dbContext.SaveChanges();
        }
    }
}