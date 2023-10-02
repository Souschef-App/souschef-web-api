// DTOs/FavoriteRecipeDto.cs
public class FavoriteRecipeDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RecipeId { get; set; }
    public DateTime DateAdded { get; set; }
    public RecipeDto Recipe { get; set; }
    public UserDto User { get; set; }
}