using System.ComponentModel.DataAnnotations;

namespace MyNunitWeb.Models
{
    public class AssemblyModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TestModel> Tests { get; set; }
    }
}
