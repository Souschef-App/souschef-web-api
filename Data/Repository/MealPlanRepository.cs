// Repositories/MealPlanRepository.cs
using Microsoft.EntityFrameworkCore;
using souschef.server.Data;
using souschef.server.Data.Models;

public class MealPlanRepository
{
    private readonly PostGresDBContext _context;
    public IEnumerable<MealPlan>? MealPlans => _context.MealPlans?.Include(m => m.Recipes).Include(m => m.ApplicationUser);

    public MealPlanRepository(PostGresDBContext context) { _context = context; }

    public bool Create(string name, DateTime dateTime, ApplicationUser user)
    {
        try
        {
            var mealPlan = new MealPlan { 
                Name = name, 
                Date = dateTime, 
                ApplicationUser = user, 
            };

            _context.AddAsync(mealPlan);
            _context.SaveChanges();

            return true;
        } 
        catch (Exception)
        {
            return false;
        }
    }

    public IEnumerable<MealPlan> GetAll()
    {
        return MealPlans?.ToList() ?? new List<MealPlan>();
    }

    public bool DeleteById(Guid id)
    {
        var mealplan = _context.MealPlans?.First(mealplan => mealplan.Id == id);

        if (mealplan != null)
        {
            _context.MealPlans?.Remove(mealplan);
            _context.SaveChanges();
            return true;
        }

        return false;
    }

    public bool AddRecipeToMealplanByID(Guid mealplanID, Guid recipeID)
    {
        var mealplan = MealPlans?.First(mealplan => mealplan.Id == mealplanID);
        var recipe = _context.Recipes?.First(recipe => recipe.Id == recipeID);

        if (mealplan != null && recipe != null)
        {
            mealplan.Recipes.Add(recipe);
            _context.SaveChanges();
            return true;
        }

        return false;
    }
}
