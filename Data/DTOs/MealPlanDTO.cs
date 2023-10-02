public class MealDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<RecipeDto> Recipes { get; set; }
    // Add other meal properties you want to expose to the client
}
public class UpdateMealPlanDto
{
    public string Name { get; set; }
    public DateTime Date { get; set; }
    // Add properties for updating meals as needed
}