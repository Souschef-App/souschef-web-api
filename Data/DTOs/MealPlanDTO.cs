using souschef.server.Data.Models;

public class MealPlanDTO
{
    public string Id { get; set; }
    public long Date { get; set; }
    public string Name { get; set; }
    public string HostId { get; set; }
    public int OccasionType { get; set; } = 0;
    public Recipe[] Recipes { get; set; }
}
