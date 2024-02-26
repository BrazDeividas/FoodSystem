using FoodSystemAPI.Entities;

namespace FoodSystemAPI.Services;

public interface IMealPlanService
{
    void PlanMealAsync(UserMetrics userMetrics, int numberOfMeals);
    double CalculateCaloricNeeds(UserMetrics userMetrics);
}