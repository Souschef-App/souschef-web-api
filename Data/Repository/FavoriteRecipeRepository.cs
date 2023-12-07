/*// Repositories/FavoriteRecipeRepository.cs

using souschef.server.Data;
using souschef.server.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

public class FavoriteRecipeRepository
{
    private readonly PostGresDBContext _context;
    public FavoriteRecipeRepository(PostGresDBContext context) { _context = context; }

    public IEnumerable<FavoriteRecipe> GetFavoriteRecipes(Guid userId)
    {
        return _context.ApplicationUsers.Include(c => c.FavoriteRecipes).ThenInclude(x => x.Recipe).FirstOrDefault(c => c.Id.Equals(userId.ToString())).FavoriteRecipes;
    }

    public void AddRecipeToFavorite(Guid userId, Guid recipeId)
    {
        ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(c => c.Id.Equals(userId.ToString()));
        Recipe recipe = _context.Recipes.FirstOrDefault(c => c.Id.Equals(recipeId));
        FavoriteRecipe favRecipe = new FavoriteRecipe();
        favRecipe.Recipe = recipe;
        favRecipe.ApplicationUser = user;
        _context.FavoriteRecipes.Add(favRecipe);
        _context.SaveChanges();
    }

    public void DeleteRecipeFromFavorite(Guid userId, Guid recipeId)
    {
        ApplicationUser user = _context.ApplicationUsers.Include(c => c.FavoriteRecipes).ThenInclude(x => x.Recipe).FirstOrDefault(c => c.Id.Equals(userId.ToString()));
        FavoriteRecipe favRecipe = user.FavoriteRecipes.Find(c => c.Recipe.Id.Equals(recipeId));
        _context.FavoriteRecipes.Remove(favRecipe);
        _context.SaveChanges();
    }
}
*/