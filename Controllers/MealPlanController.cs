using Microsoft.AspNetCore.Mvc;
using souschef.server.Data.DTOs;
using souschef.server.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

[Route("api/mealplans")]
[ApiController]
public class MealPlanController : ControllerBase
{
    private readonly MealPlanRepository _repository;

    public MealPlanController(MealPlanRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<MealPlanDto>> GetMealPlans()
    {
        var mealPlans = _repository.GetAll();
        var mealPlanDtos = mealPlans.Select(plan => new MealPlanDto
        {
            Id = plan.Id,
            Date = plan.Date,
            // Map other properties here
            Name = plan.Name,
            Recipes = plan.Recipes.ToArray()
        }).ToList();

        return Ok(mealPlanDtos);
    }

    [HttpGet("{id}")]
    public ActionResult<MealPlanDto> GetMealPlan(Guid id)
    {
        var mealPlan = _repository.Get(id);
        if (mealPlan == null)
            return NotFound();

        var mealPlanDto = new MealPlanDto
        {
            Id = mealPlan.Id,
            Date = mealPlan.Date,
            // Map other properties here,
            Name = mealPlan.Name,
            Recipes = mealPlan.Recipes.ToArray()
        };

        return Ok(mealPlanDto);
    }

    [HttpPost]
    public ActionResult<MealPlanDto> CreateMealPlan(MealPlan model)
    {
        // You can perform validation here if needed
        var createdMealPlan = _repository.Create(model);

        var mealPlanDto = new MealPlanDto
        {
            Id = createdMealPlan.Id,
            Name = createdMealPlan.Name,
            Date = createdMealPlan.Date
        };

        return CreatedAtAction(nameof(GetMealPlan), new { id = mealPlanDto.Id }, mealPlanDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateMealPlan(Guid id, MealPlan model)
    {
        var existingMealPlan = _repository.Get(id);
        if (existingMealPlan == null)
            return NotFound();

        if (model.Name != null)
            existingMealPlan.Name = model.Name;

        if (model.Date != null)
            existingMealPlan.Date = model.Date;
        // Update other properties as needed

        _repository.Update(existingMealPlan);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMealPlan(Guid id)
    {
        var mealPlan = _repository.Get(id);
        if (mealPlan == null)
            return NotFound();

        _repository.Delete(id);
        return NoContent();
    }

    [HttpGet("{id}/recipes")]
    public ActionResult<MealPlanRecipe> GetMealPlanRecipes(Guid id)
    {
        var existingMealPlan = _repository.Get(id);
        if (existingMealPlan == null)
            return NotFound();
        return Ok(_repository.GetMealPlanRecipes(id));
    }


    [HttpPost("{id}/recipes/{type}/{recipeId}")]
    public IActionResult AddRecipeToMealPlan(Guid id, string type, Guid recipeId)
    {
        var existingMealPlan = _repository.Get(id);
        if (existingMealPlan == null)
            return NotFound();
        _repository.AddRecipeToMealPlan(id, type, recipeId);
        return Ok();
    }

    [HttpDelete("{id}/recipes/{recipeId}")]
    public IActionResult DeleteRecipeFromMealPlan(Guid id, Guid recipeId)
    {
        var existingMealPlan = _repository.Get(id);
        if (existingMealPlan == null)
            return NotFound();
        _repository.DeleteRecipeFromMealPlan(id, recipeId);
        return Ok();
    }
}
