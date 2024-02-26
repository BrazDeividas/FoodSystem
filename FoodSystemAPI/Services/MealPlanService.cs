using Azure;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Repositories;

namespace FoodSystemAPI.Services;

public class MealPlanService : IMealPlanService
{
    private readonly IRepository<MealPlan> _mealPlanRepository;

    private readonly HttpClient _internalApiClient;

    public MealPlanService(IRepository<MealPlan> mealPlanRepository, IHttpClientFactory httpClientFactory)
    {
        _mealPlanRepository = mealPlanRepository;
        _internalApiClient = httpClientFactory.CreateClient("api-internal");
    }

    public async void PlanMealAsync(UserMetrics userMetrics, int numberOfMeals)
    {
        var mealPlan = await _mealPlanRepository.GetAll(x => x.UserId == userMetrics.UserId);
        if (mealPlan.Any())
        {
            return;
        }

        var neededCalories = CalculateCaloricNeeds(userMetrics);

        var mealPlanEntity = new MealPlan
        {
            UserId = userMetrics.UserId,
            TotalCalories = (int)double.Ceiling(neededCalories)
        };

        

        var response = await _internalApiClient.GetFromJsonAsync<Response<IEnumerable<ReceiveServerRecipeDto>>>("api/Recipe?search=healthy");
    }

    public double CalculateCaloricNeeds(UserMetrics userMetrics) // Mifflin-St Jeor Formula + Harris Benedict Equation for Caloric needs
    {
        double bmr = (9.99 * userMetrics.Weight) + (6.25 * userMetrics.Height) - (4.92 * userMetrics.Age);
        bmr = userMetrics.Sex == UserMetrics.SexType.Male
        ? bmr + 5
        : bmr - 161;
        return bmr * userMetrics.ActivityLevel switch
        {
            UserMetrics.ActivityLevelType.Sedentary => 1.2,
            UserMetrics.ActivityLevelType.LightlyActive => 1.375,
            UserMetrics.ActivityLevelType.ModeratelyActive => 1.55,
            UserMetrics.ActivityLevelType.VeryActive => 1.725,
            UserMetrics.ActivityLevelType.ExtraActive => 1.9
        };
    }
}