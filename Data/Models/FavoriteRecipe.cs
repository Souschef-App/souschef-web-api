using souschef.server.Data;
using souschef.server.Data.Models;
using System.ComponentModel.DataAnnotations;

// Models/FavoriteRecipe.cs

public class FavoriteRecipe
{
    [Key]
    public Guid Id { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public Recipe Recipe { get; set; }
}
