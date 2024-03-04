using FoodSystemClient.Models;
using FoodSystemClient.Wrappers;

namespace FoodSystemClient.Services;

public interface IMealPlanService
{
    Task<Response<MealPlan>> GetCurrentMealPlanAsync();
    Task<Response<MealPlan>> GenerateMealPlanAsync(int numberOfMeals, DateTime startDate, DateTime endDate);
}