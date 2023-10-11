using souschef.server.Data;

// Models/FavoriteRecipe.cs

public class FavoriteRecipe
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsFavorite { get; set; } // New property for indicating if it's a favorite
}
