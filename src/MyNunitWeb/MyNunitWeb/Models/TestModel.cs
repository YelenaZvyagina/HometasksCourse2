using MyNUnit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyNunitWeb.Models
{
    public class TestModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; init; }
        public string Result { get; init; }
        public string IgnoreReason { get; init; }
        public long ExecutionTime { get; init; }

        [ForeignKey("AssemblyModel.Id")]
        public int AssemblyModelId { get; init; }
    }
}
