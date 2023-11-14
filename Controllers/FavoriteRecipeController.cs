using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

[Route("api/favoriterecipes")]
[ApiController]
public class FavoriteRecipeController : ControllerBase
{
    private readonly FavoriteRecipeRepository _repository;

    public FavoriteRecipeController(FavoriteRecipeRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{id}")]
    public ActionResult<FavoriteRecipe> GetFavoriteRecipeList(Guid id)
    {
        return Ok(_repository.GetFavoriteRecipes(id));
    }

    [HttpPost("{id}/{recipeId}")]
    public ActionResult<FavoriteRecipe> AddFavoriteRecipe(Guid id, Guid recipeId)
    {
        Console.WriteLine(id + " " + recipeId);
        _repository.AddRecipeToFavorite(id, recipeId);
        return Ok();
    }

    [HttpDelete("{id}/{recipeId}")]
    public ActionResult<FavoriteRecipe> DeleteFavoriteRecipe(Guid id, Guid recipeId)
    {
        _repository.DeleteRecipeFromFavorite(id, recipeId);
        return Ok();
    }
}
