

using souschef.server.Data.Models;

namespace souschef.server.Data.DTOs
{
    public class RecipeDTO
    {
        public string? OwnerId            { get; set; }
        public string? Name               { get; set; }
        public float?  Duration           { get; set; }
        public TaskDTO[]? Tasks           { get; set; }
        public DateTime DateCreated       { get; set; }
        public int Difficulty             { get; set; }
        public int Serves                 { get; set; }
        public Ingredient[]? Ingredients  { get; set; }
        public Kitchenware[]? KitchenWare { get; set; }
    }

    public class TaskDTO
    {
        public string? Title              { get; set; }
        public Ingredient[]? Ingredients  { get; set; }
        public Kitchenware[]? KitchenWare { get; set; }
        public string? Instructions       { get; set; }
        public int Difficulty             { get; set; }
        public int Order                  { get; set; }
        public int[]? Dependencies        { get; set; }
        public float Duration             { get; set; }
        public string? VideoURL           { get; set; }
    }
}
