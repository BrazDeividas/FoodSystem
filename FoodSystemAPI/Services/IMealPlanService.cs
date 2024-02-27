using FoodSystemAPI.Entities;

namespace FoodSystemAPI.Services;

public interface IMealPlanService
{
    Task<MealPlan> PlanMealAsync(UserMetrics userMetrics, int numberOfMeals);
    double CalculateCaloricNeeds(UserMetrics userMetrics);
}