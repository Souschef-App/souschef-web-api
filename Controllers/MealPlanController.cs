using Microsoft.AspNetCore.Mvc;
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
            Date = plan.Date
            // Map other properties here
        }).ToList();

        return Ok(mealPlanDtos);
    }

    [HttpGet("{id}")]
    public ActionResult<MealPlanDto> GetMealPlan(int id)
    {
        var mealPlan = _repository.Get(id);
        if (mealPlan == null)
            return NotFound();

        var mealPlanDto = new MealPlanDto
        {
            Id = mealPlan.Id,
            Date = mealPlan.Date
            // Map other properties here
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
            Date = createdMealPlan.Date
        };

        return CreatedAtAction(nameof(GetMealPlan), new { id = mealPlanDto.Id }, mealPlanDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateMealPlan(int id, MealPlan model)
    {
        var existingMealPlan = _repository.Get(id);
        if (existingMealPlan == null)
            return NotFound();

        existingMealPlan.Date = model.Date;
        // Update other properties as needed

        _repository.Update(existingMealPlan);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMealPlan(int id)
    {
        var mealPlan = _repository.Get(id);
        if (mealPlan == null)
            return NotFound();

        _repository.Delete(id);
        return NoContent();
    }
}
