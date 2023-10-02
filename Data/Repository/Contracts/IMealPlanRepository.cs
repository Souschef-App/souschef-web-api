public interface IMealPlanRepository
{
    Task<MealPlan> GetMealPlanAsync(int mealPlanId);
    Task<IEnumerable<MealPlan>> GetAllMealPlansAsync();
    Task AddMealPlanAsync(MealPlan mealPlan);
    Task UpdateMealPlanAsync(int mealPlanId, MealPlan updatedMealPlan);
    Task DeleteMealPlanAsync(int mealPlanId);
}