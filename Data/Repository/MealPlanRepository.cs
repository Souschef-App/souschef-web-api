// Repositories/MealPlanRepository.cs
using Microsoft.EntityFrameworkCore;
using souschef.server.Data;
using souschef.server.Data.Models;
using souschef.server.Data.Repository;

public class MealPlanRepository
{
    private readonly PostGresDBContext _context;
    public IEnumerable<MealPlan>? MealPlans => _context.MealPlans.Include(c => c.Recipes).ThenInclude(x => x.Recipe);

    public MealPlanRepository(PostGresDBContext context) { _context = context; }

    public MealPlan Create(MealPlan mealPlan)
    {
        var res = _context.MealPlans.Add(mealPlan);
        _context.SaveChanges();
        return res.Entity;
    }

    public MealPlan? Get(Guid id)
    {
        return MealPlans.FirstOrDefault(c => c.Id == id);
    }

    public IEnumerable<MealPlan> GetAll()
    {
        return MealPlans;
    }

    public void Update(MealPlan mealPlan)
    {
        _context.MealPlans.Update(mealPlan);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        _context.MealPlans.Remove(Get(id));
        _context.SaveChanges();
    }

    public IEnumerable<MealPlanRecipe> GetMealPlanRecipes(Guid id)
    {
        return MealPlans.FirstOrDefault(c => c.Id == id).Recipes;
    }

    public void AddRecipeToMealPlan(Guid id, string type, Guid recipeId)
    {
        MealPlan mealPlan = Get(id);
        MealPlanRecipe mealPlanRecipe = new MealPlanRecipe();
        mealPlanRecipe.Recipe = _context.Recipes.FirstOrDefault(c => c.Id == recipeId);
        mealPlanRecipe.MealType = type;
        mealPlanRecipe.Order = 0;
        mealPlanRecipe.MealPlan = mealPlan;

        _context.MealPlanRecipes.Add(mealPlanRecipe);
        _context.SaveChanges();
    }

    public void DeleteRecipeFromMealPlan(Guid id, Guid recipeId)
    {
        MealPlan mealPlan = Get(id);
        MealPlanRecipe mealPlanRecipe = mealPlan.Recipes.Find(c => c.Id.Equals(recipeId));
        _context.MealPlanRecipes.Remove(mealPlanRecipe);
        _context.SaveChanges();
    }
}
