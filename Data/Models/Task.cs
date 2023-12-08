
using System.ComponentModel.DataAnnotations;


namespace souschef.server.Data.Models
{
    public class Task
    {
        [Key]
        public Guid Id { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }

        public List<Ingredient>? Ingredients { get; set; }
        public List<Kitchenware>? Kitchenware { get; set; }

        public float Duration { get; set; }
        public int Difficulty { get; set; }

        // public int Order { get; set; }
        public List<Dependency>? Dependencies { get; set; }
        public bool IsBackground { get; set; }

        // public int Points { get; set; }
        // public bool Finished { get; set; }
        // public bool InProgress { get; set; }
        // public ApplicationUser? Assignee { get; set; }
    }
}