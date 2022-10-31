namespace MyNunitWeb.Data;

using Microsoft.EntityFrameworkCore;
using MyNunitWeb.Models;

/// <summary>
/// Class for interaction with database
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<TestModel> TestResults { get; set; }
    public DbSet<AssemblyModel> TestAssemblies { get; set; }
}