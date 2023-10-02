using Microsoft.AspNetCore.Mvc;
using RecipeAPI.Models;
using RecipeAPI.Repositories;
using souschef.server.Data.Repository.Contracts;
using System;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class MealSessionController : ControllerBase
{
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IMealPlanRepository _mealPlanRepository;

     public MealSessionController(
        IFavoriteRepository favoriteRepository,
        ISessionRepository sessionRepository,
        IMealPlanRepository mealPlanRepository)
    {
        _favoriteRepository = favoriteRepository;
        _sessionRepository = sessionRepository;
        _mealPlanRepository = mealPlanRepository;
    }

    //API endpoints

[HttpGet("recipes/favorites")]
public IActionResult GetFavoriteRecipes()
{
    var favoriteRecipes = _recipeRepository.GetFavoriteRecipes(); // Implement this method in your repository
    return Ok(favoriteRecipes);
}

[HttpGet("sessions")]
    public IActionResult GetSessions()
    {
        var sessions = _sessionRepository.GetAllSessions();
        return Ok(sessions);
    }

     [HttpPost("sessions")]
    public IActionResult CreateSession([FromBody] Session session)
    {
        if (session == null)
        {
            return BadRequest("Invalid data");
        }

        _sessionRepository.AddSession(session);
        return CreatedAtAction(nameof(GetSessions), new { id = session.Id }, session);
    }

[HttpGet("mealplans")]
    public IActionResult GetMealPlans()
    {
        var mealPlans = _mealPlanRepository.GetAllMealPlans();
        return Ok(mealPlans);
    }

[HttpPost("mealplans")]
    public IActionResult CreateMealPlan([FromBody] MealPlan mealPlan)
    {
        if (mealPlan == null)
        {
            return BadRequest("Invalid data");
        }

        _mealPlanRepository.AddMealPlan(mealPlan);
        return CreatedAtAction(nameof(GetMealPlans), new { id = mealPlan.Id }, mealPlan);
    }

[HttpPost("mealplans/{mealPlanId}/addrecipe")]
    public IActionResult AddRecipeToMealPlan(int mealPlanId, [FromBody] Recipe recipe)
    {
        var mealPlan = _mealPlanRepository.GetMealPlanById(mealPlanId);
        if (mealPlan == null)
        {
            return NotFound("Meal plan not found");
        }

        if (recipe == null)
        {
            return BadRequest("Invalid data");
        }

        mealPlan.Recipes.Add(recipe);
        _mealPlanRepository.UpdateMealPlan(mealPlan);

        return Ok(mealPlan);
    }

[HttpPost("sessions/{sessionId}/setdatetime")]
public IActionResult SetSessionDateTime(int sessionId, [FromBody] DateTime sessionDateTime)
{
    var session = _sessionRepository.GetSessionById(sessionId);
    if (session == null)
    {
        return NotFound("Session not found");
    }

    session.Date = sessionDateTime.Date;
    session.Time = sessionDateTime.TimeOfDay;
    _sessionRepository.UpdateSession(session);

    return Ok(session);
}
}