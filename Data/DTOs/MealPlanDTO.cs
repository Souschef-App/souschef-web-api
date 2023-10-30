using souschef.server.Data.Models;

public class MealPlanDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }

    public string Name { get; set; }

    public MealPlanRecipe[] Recipes { get; set; }
}
