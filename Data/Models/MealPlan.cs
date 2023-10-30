// Models/MealPlan.cs
using System.ComponentModel.DataAnnotations;
using souschef.server.Data.Models;

public class MealPlan
{
    [Key]
    public Guid Id { get; set; }
    public DateTime Date { get; set; }

    public string Name { get; set; }

    public List<MealPlanRecipe>? Recipes { get; set; }
    //public bool IsFavorite { get; set; } // New property for indicating if it's a favorite
}
