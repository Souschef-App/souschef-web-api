using souschef.server.Data;

public class FavoriteRecipe{

    public int Id {get; set;}
    public string recipeName {get; set;}
    public bool IsFavorite {get; set;} // New property for indicating if it's a favorite
}
