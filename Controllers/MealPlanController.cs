using Microsoft.AspNetCore.Identity;
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
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly MealPlanRepository _repository;

    public MealPlanController(UserManager<ApplicationUser> userManager, MealPlanRepository repository)
    {
        _userManager = userManager;
        _repository = repository;
    }

    [HttpGet("get-all")]
    public ActionResult<IEnumerable<MealPlanDTO>> GetAllMealPlans()
    {
        var mealPlans = _repository.GetAll();
        var mealPlanDTOs = mealPlans.Select(mealplan => new MealPlanDTO
        {
            Id = mealplan.Id.ToString(),
            Name = mealplan.Name,
            Date = mealplan.Date.Ticks,
            Recipes = mealplan.Recipes.ToArray(),
            HostId = mealplan?.ApplicationUser?.Id?.ToString() ?? ""
        });

        return Ok(mealPlanDTOs);
    }

    [HttpPost("create")]
    public async Task<bool> CreateMealPlan([FromBody] MealPlanCreateDTO mealplanCreateDTO)
    {
        // Get ApplicationUser from ID
        ApplicationUser user = await _userManager.FindByIdAsync(mealplanCreateDTO.OwnerID);

        if (user != null)
        {
            var name = mealplanCreateDTO.Name;
            var date = new DateTime(mealplanCreateDTO.Date);
            return _repository.Create(name, date, user);
        }

        return false;
    }

    [HttpPost("delete")]
    public IActionResult DeleteMealPlan([FromQuery] string id)
    {
        var success = _repository.DeleteById(new Guid(id));

        if (success)
        {
            return Ok();
        }

        return new ContentResult
        {
            StatusCode = 400,
            Content = "Failed to delete mealplan"
        };
    }

    [HttpPost("add-recipe")]
    public IActionResult AddRecipeToMealPlan([FromQuery] string mealplanID, [FromQuery] string recipeID)
    {
        var success = _repository.AddRecipeToMealplanByID(new Guid(mealplanID), new Guid(recipeID));

        if (success)
        {
            return Ok();
        }

        return new ContentResult
        {
            StatusCode = 400,
            Content = "Failed to add recipe to mealplan"
        };
    }

    /*[HttpPut("{id}")]
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
    }*/
}
