public class MealDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<RecipeDto> Recipes { get; set; }
}
public class UpdateMealPlanDto
{
    public string Name { get; set; }
    public DateTime Date { get; set; }
}