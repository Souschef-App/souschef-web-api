public interface IFavoriteRepository
{
    Task<FavoriteRecipe> GetFavoriteRecipeAsync(int favoriteRecipeId);
    Task<IEnumerable<FavoriteRecipe>> GetAllFavoriteRecipesAsync();
    Task AddFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe);
    Task UpdateFavoriteRecipeAsync(int favoriteRecipeId, FavoriteRecipe updatedFavoriteRecipe);
    Task DeleteFavoriteRecipeAsync(int favoriteRecipeId);
}