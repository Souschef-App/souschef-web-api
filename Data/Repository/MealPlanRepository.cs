using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Models;

public class MealPlanRepository : IMealPlanRepository
{
    private readonly AppDbContext _context;

    public MealPlanRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<MealPlan> GetMealPlanAsync(int mealPlanId)
    {
        return await _context.MealPlans
            .Include(mp => mp.Meals) // Include related meals
            .FirstOrDefaultAsync(mp => mp.Id == mealPlanId);
    }

    public async Task<IEnumerable<MealPlan>> GetAllMealPlansAsync()
    {
        return await _context.MealPlans
            .Include(mp => mp.Meals) // Include related meals
            .ToListAsync();
    }

    public async Task AddMealPlanAsync(MealPlan mealPlan)
    {
        _context.MealPlans.Add(mealPlan);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateMealPlanAsync(int mealPlanId, MealPlan updatedMealPlan)
    {
        var existingMealPlan = await _context.MealPlans.FindAsync(mealPlanId);
        if (existingMealPlan != null)
        {
            // Update properties of the existing meal plan based on updatedMealPlan
            existingMealPlan.Name = updatedMealPlan.Name;
            existingMealPlan.Date = updatedMealPlan.Date;

            // Update meals as needed
            existingMealPlan.Meals.Clear(); // Clear existing meals
            existingMealPlan.Meals.AddRange(updatedMealPlan.Meals); // Add updated meals

            // Update other properties as needed
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteMealPlanAsync(int mealPlanId)
    {
        var mealPlan = await _context.MealPlans.FindAsync(mealPlanId);
        if (mealPlan != null)
        {
            _context.MealPlans.Remove(mealPlan);
            await _context.SaveChangesAsync();
        }
    }

    // Implement other methods as needed
}
