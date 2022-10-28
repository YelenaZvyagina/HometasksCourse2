using Microsoft.EntityFrameworkCore;
using MyNunitWeb.Models;

namespace MyNunitWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // table inside db
        public DbSet<TestModel> TestResults { get; set; }
        public DbSet<AssemblyModel> TestAssmblies { get; set; }
    }
}
