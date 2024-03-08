using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;

namespace FoodSystemAPI.Services;

public interface IMealPlanService
{
    Task<SendClientMealPlanDto?> GetCurrentMealPlanAsync(int userId);
    Task<MealPlan> PlanMealAsync(UserMetrics userMetrics, int numberOfMeals, DateTime startDate, DateTime endDate);
    double CalculateCaloricNeeds(UserMetrics userMetrics);
}