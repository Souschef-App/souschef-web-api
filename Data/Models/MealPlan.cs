// Models/MealPlan.cs
using System.ComponentModel.DataAnnotations;
using souschef.server.Data.Models;

public class MealPlan
{
    [Key]
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public List<Recipe> Recipes { get; set; } = new List<Recipe>();
    public ApplicationUser? ApplicationUser { get; set; }
}
