using System.Text;
using System.Text.Json;
using AutoMapper;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Wrappers;
using Microsoft.IdentityModel.Tokens;

namespace FoodSystemAPI.Services;

public class RecipeService : IRecipeService
{
    private readonly HttpClient _apiClient;

    private readonly HttpClient _internalApiClient;

    private readonly IMapper _mapper;

    public RecipeService(IHttpClientFactory httpClientFactory, IMapper mapper)
    {
        _apiClient = httpClientFactory.CreateClient("api-1");
        _internalApiClient = httpClientFactory.CreateClient("api-internal");
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReceiveServerRecipeDto>> GetRecipesAsync(string searchQuery)
    {
        var response = await _internalApiClient.GetFromJsonAsync<PagedResponse<IEnumerable<ReceiveServerRecipeDto>>>($"api/Recipe?search={searchQuery}");
        if (!response.Data.IsNullOrEmpty())
        {
            return response.Data;
        }
        else
        {
            var responseAPI1 = await _apiClient.GetFromJsonAsync<RapidAPIDto_1>($"?q={searchQuery}");
            //var responseAPI1 = JsonSerializer.Deserialize<RapidAPIDto_1>(File.ReadAllText("beef-api-call-example.json"));
            if(!responseAPI1.d.IsNullOrEmpty())
            {
                var internalRecipesToSend = _mapper.Map<IEnumerable<SendServerRecipeDto>>(responseAPI1.d);
                File.WriteAllText("test.json", JsonSerializer.Serialize(internalRecipesToSend));
                var internalResult = await _internalApiClient.PostAsJsonAsync("api/Recipe", internalRecipesToSend);
                if (internalResult.IsSuccessStatusCode)
                {
                    var internalRecipes = await internalResult.Content.ReadFromJsonAsync<PagedResponse<IEnumerable<ReceiveServerRecipeDto>>>();
                    return internalRecipes.Data;
                }
            }
        }
        return null;
    }

    public async Task<IEnumerable<ReceiveServerRecipeDto>> GetRecipesByIngredientsAsync(string ingredients)
    {
        var response = await _internalApiClient.GetFromJsonAsync<PagedResponse<IEnumerable<ReceiveServerRecipeDto>>>($"api/Recipe/byIngredients?ingredients={ingredients}");
        return response.Data;
    }
}