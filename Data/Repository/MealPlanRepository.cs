// Repositories/MealPlanRepository.cs

using System;
using System.Collections.Generic;
using System.Linq;

public class MealPlanRepository
{
    private readonly List<MealPlan> _mealPlans = new List<MealPlan>();
    private int _nextId = 1;

    public MealPlan Create(MealPlan mealPlan)
    {
        mealPlan.Id = _nextId++;
        _mealPlans.Add(mealPlan);
        return mealPlan;
    }

    public MealPlan? Get(int id)
    {
        return _mealPlans.FirstOrDefault(mp => mp.Id == id);
    }

    public IEnumerable<MealPlan> GetAll()
    {
        return _mealPlans;
    }

    public void Update(MealPlan mealPlan)
    {
        var existingMealPlan = Get(mealPlan.Id);
        if (existingMealPlan != null)
        {
            existingMealPlan.Date = mealPlan.Date;
        }
    }

    public void Delete(int id)
    {
        var mealPlan = Get(id);
        if (mealPlan != null)
        {
            _mealPlans.Remove(mealPlan);
        }
    }
}
