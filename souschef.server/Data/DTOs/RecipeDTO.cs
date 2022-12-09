﻿

using souschef.server.Data.Models;

namespace souschef.server.Data.DTOs
{
    public class RecipeDTO
    {
        public string? OwnerId      { get; set; }
        public string? Name         { get; set; }
        public Step[]? Steps        { get; set; }
        public DateTime DateCreated { get; set; }

    }

    public class Step
    {
        public Ingredient[]? Ingredients  { get; set; }
        public Kitchenware[]? KitchenWare { get; set; }
        public string? Instructions       { get; set; }
        public int Difficulty             { get; set; }
        public float TimeEstimate         { get; set; }
        public string? VideoURL           { get; set; }
    }
}
