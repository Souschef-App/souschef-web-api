// Models/Session.cs

using souschef.server.Data.Models;
using System.ComponentModel.DataAnnotations;

public class MealSession
{
    [Key]
    public Guid Id { get; set; }
    public DateTime DateTime { get; set; }

    public string? SessionCode { get; set; }
    public string? ServerIp { get; set; }

    public MealPlan? Plan { get; set; }
    public List<MealSessionUser>? Users { get; set; }
}
