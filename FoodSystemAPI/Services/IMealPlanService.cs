using FoodSystemAPI.Entities;

namespace FoodSystemAPI.Services;

public interface IMealPlanService
{
    void PlanMealAsync(UserMetrics userMetrics);

    double CalculateCaloricNeeds(UserMetrics userMetrics);
}