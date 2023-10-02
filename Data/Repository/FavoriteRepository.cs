using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Models;

public class FavoriteRepository : IFavoriteRepository
{
    private readonly AppDbContext _context;

    public FavoriteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FavoriteRecipe> GetFavoriteRecipeAsync(int favoriteRecipeId)
    {
        return await _context.FavoriteRecipes.FindAsync(favoriteRecipeId);
    }

    public async Task<IEnumerable<FavoriteRecipe>> GetAllFavoriteRecipesAsync()
    {
        return await _context.FavoriteRecipes.ToListAsync();
    }

    public async Task AddFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe)
    {
        _context.FavoriteRecipes.Add(favoriteRecipe);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFavoriteRecipeAsync(int favoriteRecipeId, FavoriteRecipe updatedFavoriteRecipe)
    {
        var existingFavoriteRecipe = await _context.FavoriteRecipes.FindAsync(favoriteRecipeId);
        if (existingFavoriteRecipe != null)
        {
            
            existingFavoriteRecipe.UserId = updatedFavoriteRecipe.UserId;
            existingFavoriteRecipe.RecipeId = updatedFavoriteRecipe.RecipeId;
        
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteFavoriteRecipeAsync(int favoriteRecipeId)
    {
        var favoriteRecipe = await _context.FavoriteRecipes.FindAsync(favoriteRecipeId);
        if (favoriteRecipe != null)
        {
            _context.FavoriteRecipes.Remove(favoriteRecipe);
            await _context.SaveChangesAsync();
        }
    }
}
