using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Repositories;
using FoodSystemAPI.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace FoodSystemAPI.Services;

public class MealPlanService : IMealPlanService
{
    private readonly IRepository<MealPlan> _mealPlanRepository;
    
    private readonly HttpClient _internalApiClient;

    private readonly IRecipeService _recipeService;

    public MealPlanService(IRepository<MealPlan> mealPlanRepository, IHttpClientFactory httpClientFactory, IRecipeService recipeService)
    {
        _mealPlanRepository = mealPlanRepository;
        _internalApiClient = httpClientFactory.CreateClient("api-internal");
        _recipeService = recipeService;
    }

    public async Task<MealPlan> GetCurrentMealPlanAsync(int userId)
    {
        var mealPlan = _mealPlanRepository.IncludeMultipleQueryable(
            _mealPlanRepository.GetAllQueryable(x => x.UserId == userId && x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now),
            x => x.MealPlanItems
        );
        var mealPlanFirst = await mealPlan.FirstOrDefaultAsync();
        var recipes = await _recipeService.GetSavedRecipesAsync(x => mealPlanFirst.MealPlanItems.Select(i => i.RecipeId).Contains(x.RecipeId));
        mealPlanFirst.MealPlanItems.ToList().ForEach(x => x.Recipe = x.Recipe ?? recipes.FirstOrDefault(r => r.RecipeId == x.RecipeId));
        return mealPlanFirst;
    }

    public async Task<MealPlan> PlanMealAsync(UserMetrics userMetrics, int numberOfMeals, DateTime startDate, DateTime endDate)
    {
        var neededCalories = CalculateCaloricNeeds(userMetrics);

        var days = (int)double.Ceiling((endDate - startDate).TotalDays);

        var response = await _internalApiClient.GetFromJsonAsync<Response<IEnumerable<ReceiveServerRecipeDto>>>($"api/Recipe/byFilter?calorieSum={(int)neededCalories}&numberOfMeals={numberOfMeals}&days={days}");

        if (response.Data.Count() < numberOfMeals) //TODO: fetch more recipes later
        {
            return null;
        }

        var recipeEntities = await _recipeService.AddRecipesForUserAsync(response.Data, userMetrics.UserId);

        var mealPlanEntity = new MealPlan
        {
            UserId = userMetrics.UserId,
            TotalCalories = (int)double.Ceiling(neededCalories),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        foreach (var recipe in recipeEntities)
        {
            mealPlanEntity.MealPlanItems.Add(new MealPlanItem
            {
                RecipeId = recipe.RecipeId,
                MealPlanId = mealPlanEntity.MealPlanId
            });
        }

        await _mealPlanRepository.Add(mealPlanEntity);
        _mealPlanRepository.Save();

        return mealPlanEntity;
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
            UserMetrics.ActivityLevelType.ExtraActive => 1.9,
        };
    }
}