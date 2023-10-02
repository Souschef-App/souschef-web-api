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

// Endpoint for managing favorite recipes
    [HttpGet("favoriterecipes")]
    public async Task<ActionResult<IEnumerable<FavoriteRecipeDto>>> GetFavoriteRecipes()
    {
        var favoriteRecipes = await _favoriteRepository.GetAllFavoriteRecipesAsync();
        var favoriteRecipeDtos = favoriteRecipes.Select(fr => new FavoriteRecipeDto
        {
            Id = fr.Id,
            UserId = fr.UserId,
            RecipeId = fr.RecipeId,
            DateAdded = fr.DateAdded,
            // Map other properties as needed
        });

        return Ok(favoriteRecipeDtos);
    }


 [HttpPost("favoriterecipes")]
    public async Task<ActionResult<FavoriteRecipeDto>> AddFavoriteRecipe(FavoriteRecipeDto favoriteRecipeDto)
    {
        var favoriteRecipe = new FavoriteRecipe
        {
            UserId = favoriteRecipeDto.UserId,
            RecipeId = favoriteRecipeDto.RecipeId,
            DateAdded = DateTime.UtcNow,
            // Map other properties as needed
        };

        await _favoriteRepository.AddFavoriteRecipeAsync(favoriteRecipe);

        var savedFavoriteRecipeDto = new FavoriteRecipeDto
        {
            Id = favoriteRecipe.Id,
            UserId = favoriteRecipe.UserId,
            RecipeId = favoriteRecipe.RecipeId,
            DateAdded = favoriteRecipe.DateAdded,
            // Map other properties as needed
        };

        return CreatedAtAction(nameof(GetFavoriteRecipes), savedFavoriteRecipeDto);
    }

    [HttpPut("favoriterecipes/{id}")]
    public async Task<IActionResult> UpdateFavoriteRecipe(int id, FavoriteRecipeDto updatedFavoriteRecipeDto)
    {
        var existingFavoriteRecipe = await _favoriteRepository.GetFavoriteRecipeAsync(id);

        if (existingFavoriteRecipe == null)
        {
            return NotFound();
        }

        // Update properties of the existing favorite recipe based on updatedFavoriteRecipeDto
        existingFavoriteRecipe.UserId = updatedFavoriteRecipeDto.UserId;
        existingFavoriteRecipe.RecipeId = updatedFavoriteRecipeDto.RecipeId;
        // Update other properties as needed

        await _favoriteRepository.UpdateFavoriteRecipeAsync(id, existingFavoriteRecipe);

        return NoContent();
    }

    [HttpDelete("favoriterecipes/{id}")]
    public async Task<IActionResult> DeleteFavoriteRecipe(int id)
    {
        var existingFavoriteRecipe = await _favoriteRepository.GetFavoriteRecipeAsync(id);

        if (existingFavoriteRecipe == null)
        {
            return NotFound();
        }

        await _favoriteRepository.DeleteFavoriteRecipeAsync(id);

        return NoContent();
    }

    // Endpoint for managing meal plans
    [HttpGet("mealplans")]
    public async Task<ActionResult<IEnumerable<MealPlanD>>> GetMealPlans()
    {
        var mealPlans = await _mealPlanRepository.GetAllMealPlansAsync();
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

        await _mealPlanRepository.AddMealPlanAsync(mealPlan);

        var savedMealPlanDto = new MealPlanDto
        {
            Id = mealPlan.Id,
            Name = mealPlan.Name,
            Date = mealPlan.Date,
            Meals = mealPlan.Meals.Select(meal => new MealDto
            {
                Id = meal.Id,
                Name = meal.Name,
                // Map other meal properties as needed
            }).ToList(),
            // Map other properties as needed
            
        };
}

 [HttpGet("sessions")]
    public async Task<ActionResult<IEnumerable<SessionDto>>> GetSessions()
    {
        var sessions = await _sessionRepository.GetAllSessionsAsync();
        var sessionDtos = sessions.Select(s => new SessionDto
        {
            Id = s.Id,
            Name = s.Name,
            StartTime = s.StartTime,
            // Map other properties as needed
        });

        return Ok(sessionDtos);
    }
    [HttpPost("sessions")]
    public async Task<ActionResult<SessionDto>> CreateSession(SessionDto sessionDto)
    {
        var session = new Session
        {
            Name = sessionDto.Name,
            StartTime = sessionDto.StartTime,
            // Map other properties as needed
        };

        await _sessionRepository.AddSessionAsync(session);

        var savedSessionDto = new SessionDto
        {
            Id = session.Id,
            Name = session.Name,
            StartTime = session.StartTime,
            // Map other properties as needed
        };

        return CreatedAtAction(nameof(GetSessions), savedSessionDto);
    }

     [HttpPut("sessions/{id}")]
    public async Task<IActionResult> UpdateSession(int id, SessionDto updatedSessionDto)
    {
        var existingSession = await _sessionRepository.GetSessionAsync(id);

        if (existingSession == null)
        {
            return NotFound();
        }

        // Update properties of the existing session based on updatedSessionDto
        existingSession.Name = updatedSessionDto.Name;
        existingSession.StartTime = updatedSessionDto.StartTime;
        // Update other properties as needed

        await _sessionRepository.UpdateSessionAsync(id, existingSession);

        return NoContent();
    }

    [HttpDelete("sessions/{id}")]
    public async Task<IActionResult> DeleteSession(int id)
    {
        var existingSession = await _sessionRepository.GetSessionAsync(id);

        if (existingSession == null)
        {
            return NotFound();
        }

        await _sessionRepository.DeleteSessionAsync(id);

        return NoContent();
    }
}