using FoodSystemClient.Authentication;
using FoodSystemClient.Models;
using FoodSystemClient.Wrappers;

namespace FoodSystemClient.Services;

public class MealPlanService : IMealPlanService
{
    private readonly HttpClient _httpClient;
    private readonly TokenProvider _tokenProvider;

    public MealPlanService(IHttpClientFactory clientFactory, TokenProvider tokenProvider)
    {
        _httpClient = clientFactory.CreateClient("FoodSystemAPI");
        _tokenProvider = tokenProvider;
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_tokenProvider.AccessToken}");
    }

    public async Task<Response<MealPlan>> GenerateMealPlanAsync(int numberOfMeals, DateTime startDate, DateTime endDate)
    {
        var response = await _httpClient.PostAsJsonAsync("api/MealPlan", new { StartDate = startDate, EndDate = endDate, NumberOfMeals = numberOfMeals });
        return await response.Content.ReadFromJsonAsync<Response<MealPlan>>();
    }

    public async Task<Response<MealPlan>> GetCurrentMealPlanAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<Response<MealPlan>>("api/MealPlan");
        return response;
    }
}