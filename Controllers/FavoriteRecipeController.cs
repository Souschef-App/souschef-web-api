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

    [HttpGet]
    public ActionResult<IEnumerable<FavoriteRecipeDto>> GetFavoriteRecipes()
    {
        var favoriteRecipes = _repository.GetAll();
        var favoriteRecipeDtos = favoriteRecipes.Select(recipe => new FavoriteRecipeDto
        {
            Id = recipe.Id,
            Name = recipe.Name,
            Description = recipe.Description,
            IsFavorite = recipe.IsFavorite
        }).ToList();

        return Ok(favoriteRecipeDtos);
    }

    [HttpGet("{id}")]
    public ActionResult<FavoriteRecipeDto> GetFavoriteRecipe(int id)
    {
        var favoriteRecipe = _repository.Get(id);
        if (favoriteRecipe == null)
            return NotFound();

        var favoriteRecipeDto = new FavoriteRecipeDto
        {
            Id = favoriteRecipe.Id,
            Name = favoriteRecipe.Name,
            Description = favoriteRecipe.Description,
            IsFavorite = favoriteRecipe.IsFavorite
            // Map other properties here
        };

        return Ok(favoriteRecipeDto);
    }

    [HttpPost]
    public ActionResult<FavoriteRecipeDto> CreateFavoriteRecipe(FavoriteRecipe model)
    {
        var createdRecipe = _repository.Create(model);

        var favoriteRecipeDto = new FavoriteRecipeDto
        {
            Id = createdRecipe.Id,
            Name = createdRecipe.Name,
            Description = createdRecipe.Description,
            IsFavorite = createdRecipe.IsFavorite
            // Map other properties here
        };

        return CreatedAtAction(nameof(GetFavoriteRecipe), new { id = favoriteRecipeDto.Id }, favoriteRecipeDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateFavoriteRecipe(int id, FavoriteRecipe model)
    {
        var existingRecipe = _repository.Get(id);
        if (existingRecipe == null)
            return NotFound();

        existingRecipe.Name = model.Name;
        existingRecipe.Description = model.Description;
        existingRecipe.IsFavorite = model.IsFavorite;
        // Update other properties as needed

        _repository.Update(existingRecipe);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteFavoriteRecipe(int id)
    {
        var favoriteRecipe = _repository.Get(id);
        if (favoriteRecipe == null)
            return NotFound();

        _repository.Delete(id);
        return NoContent();
    }
}
