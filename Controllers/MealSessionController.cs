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
    public async Task<ActionResult<IEnumerable<MealPlanDto>>> GetMealPlans()
    {
        var mealPlans = await _context.MealPlans.ToListAsync();
        var mealPlanDtos = mealPlans.Select(mp => new MealPlanDto
        {
            Id = mp.Id,
            Name = mp.Name,
            Date = mp.Date,
            Meals = mp.Meals.Select(m => new MealDto
            {
                Id = m.Id,
                Name = m.Name,
                // Map other meal properties as needed
            }).ToList(),
            // Map other properties as needed
        });

        return Ok(mealPlanDtos);
    }

    [HttpPost("mealplans")]
    public async Task<ActionResult<MealPlanDto>> CreateMealPlan(MealPlanDto mealPlanDto)
    {
        var mealPlan = new MealPlan
        {
            Name = mealPlanDto.Name,
            Date = mealPlanDto.Date,
            Meals = mealPlanDto.Meals.Select(mealDto => new Meal
            {
                Name = mealDto.Name,
                // Map other meal properties as needed
            }).ToList(),
            // Map other properties as needed
        };

        _context.MealPlans.Add(mealPlan);
        await _context.SaveChangesAsync();

        var savedMealPlanDto = new MealPlanDto
        {
            Id = mealPlan.Id,
            Name = mealPlan.Name,
            Date = mealPlan.Date,
            Meals = mealPlan.Meals.Select(meal => new MealDto
            {
                Id = meal.Id,
                Name = meal.Name,
                // will create meal properties if needed
            }).ToList(),
            // will create other properties if needed
        };

        return CreatedAtAction(nameof(GetMealPlans), savedMealPlanDto);
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
[HttpDelete("sessions/{id}")]
public async Task<IActionResult> DeleteSession(int id)
{
    var session = await _context.Sessions.FindAsync(id);

    if (session == null)
    {
        return NotFound();
    }

    _context.Sessions.Remove(session);
    await _context.SaveChangesAsync();

    return NoContent();
}
[HttpPut("sessions/{id}")]
public async Task<IActionResult> UpdateSession(int id, UpdateSessionDto updateSessionDto)
{
    var existingSession = await _context.Sessions.FindAsync(id);

    if (existingSession == null)
    {
        return NotFound();
    }

    // Update properties of the existing session based on sessionDto
    existingSession.Name = sessionDto.Name;
    existingSession.StartTime = sessionDto.StartTime;
    // Update other properties as needed

    await _context.SaveChangesAsync();

    return NoContent();
}
[HttpPut("mealplans/{id}")]
public async Task<IActionResult> UpdateMealPlan(int id, UpdateMealPlanDto updateMealPlanDto)
{
    var existingMealPlan = await _context.MealPlans.FindAsync(id);

    if (existingMealPlan == null)
    {
        return NotFound();
    }

    // Update properties of the existing meal plan based on updateMealPlanDto
    existingMealPlan.Name = updateMealPlanDto.Name;
    existingMealPlan.Date = updateMealPlanDto.Date;
    // Update meals as needed (e.g., map updateMealPlanDto properties to existingMealPlan.Meals)

    await _context.SaveChangesAsync();

    return NoContent();
}
[HttpDelete("mealplans/{id}")]
public async Task<IActionResult> DeleteMealPlan(int id)
{
    var mealPlan = await _context.MealPlans.FindAsync(id);

    if (mealPlan == null)
    {
        return NotFound();
    }

    _context.MealPlans.Remove(mealPlan);
    await _context.SaveChangesAsync();

    return NoContent();
}
}