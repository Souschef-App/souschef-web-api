
using System.ComponentModel.DataAnnotations;

namespace souschef.server.Data.Models
{
    public class Recipe
    {
        [Key]
        public Guid Id          { get; set; }
        public string? Name     { get; set; }
        public long Date        { get; set; }
        public float Duration   { get; set; }
        public int Serves       { get; set; }
        public int Difficulty   { get; set; }
        public Guid? OwnerId    { get; set; }
        public List<Task>? Tasks { get; set; }
        public List<Ingredient>? Ingredients { get; set; }
        public List<Kitchenware>? Kitchenware { get; set; }
    }
}   