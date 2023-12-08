

using System.ComponentModel.DataAnnotations;

namespace souschef.server.Data.Models
{
    public class Dependency
    {
        [Key]
        public Guid ID { get; set; }
        public Guid DependencyID { get; set; }
        public string? Title { get; set; }
    }
}