namespace MyNunitWeb.Models;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Model for assembly with tests
/// </summary>
public class AssemblyModel
{
    [Key]
    public int Id { get; init; }
    public string Name { get; init; }
    public List<TestModel> Tests { get; set; }
}
