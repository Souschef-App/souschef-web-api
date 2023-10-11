// Models/MealPlan.cs

using souschef.server.Data.Models;

public class MealPlan
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public List<Recipe>? Recipe { get; set; }
    //public bool IsFavorite { get; set; } // New property for indicating if it's a favorite
}
