public class MealPlan
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public List<Recipe> Recipes { get; set; }
}