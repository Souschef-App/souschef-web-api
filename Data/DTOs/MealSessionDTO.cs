using souschef.server.Data.Models;

public class MealSessionDto
{
    public Guid Id { get; set; }
    public DateTime DateTime { get; set; }

    public MealPlan Plan { get; set; }
    public List<MealSessionUser> Users { get; set; }
}
