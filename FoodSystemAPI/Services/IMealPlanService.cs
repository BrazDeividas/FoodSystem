using FoodSystemAPI.Entities;

namespace FoodSystemAPI.Services;

public interface IMealPlanService
{
    Task<MealPlan> GetCurrentMealPlanAsync(int userId);
    Task<MealPlan> PlanMealAsync(UserMetrics userMetrics, int numberOfMeals, DateTime startDate, DateTime endDate);
    double CalculateCaloricNeeds(UserMetrics userMetrics);
}