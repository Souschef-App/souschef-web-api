// Repositories/FavoriteRecipeRepository.cs

using System;
using System.Collections.Generic;
using System.Linq;

public class FavoriteRecipeRepository
{
    private readonly List<FavoriteRecipe> _favoriteRecipes = new List<FavoriteRecipe>();
    private int _nextId = 1;

    public FavoriteRecipe Create(FavoriteRecipe favoriteRecipe)
    {
        favoriteRecipe.Id = _nextId++;
        _favoriteRecipes.Add(favoriteRecipe);
        return favoriteRecipe;
    }

    public FavoriteRecipe? Get(int id)
    {
        return _favoriteRecipes.FirstOrDefault(fr => fr.Id == id);
    }

    public IEnumerable<FavoriteRecipe> GetAll()
    {
        return _favoriteRecipes;
    }

    public void Update(FavoriteRecipe favoriteRecipe)
    {
        var existingRecipe = Get(favoriteRecipe.Id);
        if (existingRecipe != null)
        {
            existingRecipe.Name = favoriteRecipe.Name;
            existingRecipe.Description = favoriteRecipe.Description;
            existingRecipe.IsFavorite = favoriteRecipe.IsFavorite;
        }
    }

    public void Delete(int id)
    {
        var favoriteRecipe = Get(id);
        if (favoriteRecipe != null)
        {
            _favoriteRecipes.Remove(favoriteRecipe);
        }
    }
}
