using System.Text;
using System.Text.Json;
using AutoMapper;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.DTOs.Tasty;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Filters;
using FoodSystemAPI.Wrappers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace FoodSystemAPI.Services;

public class RecipeService : IRecipeService
{
    private readonly HttpClient _apiClient;

    private readonly HttpClient _internalApiClient;

    private readonly IMapper _mapper;

    private readonly ICacheService _cacheService;
    private readonly IIngredientService _ingredientService;

    public RecipeService(IHttpClientFactory httpClientFactory, IMapper mapper, ICacheService cacheService, IIngredientService ingredientService)
    {
        _apiClient = httpClientFactory.CreateClient("api-2");
        _internalApiClient = httpClientFactory.CreateClient("api-internal");
        _mapper = mapper;
        _cacheService = cacheService;
        _ingredientService = ingredientService;
    }

    public async Task<IEnumerable<ReceiveServerRecipeDto>> GetRecipesAsync(string searchQuery, PaginationFilter paginationFilter)
    {
        var cachedEntities = await _cacheService.Get<IEnumerable<ReceiveServerRecipeDto>>($"{typeof(ReceiveServerRecipeDto)}-{searchQuery}");
        
        if(cachedEntities != null)
        {
            return cachedEntities.Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize);
        }


        var response = await _internalApiClient.GetFromJsonAsync<Response<IEnumerable<ReceiveServerRecipeDto>>>($"api/Recipe?search={searchQuery}");
        var result = response.Data;
        if (response.Data.IsNullOrEmpty())
        {
            //var responseAPI1 = await _apiClient.GetFromJsonAsync<RapidAPIDto_1>($"?q={searchQuery}");
            var responseAPI = await _apiClient.GetFromJsonAsync<Root>($"?from=0&size=20&q={searchQuery}");

            if(!responseAPI.results.IsNullOrEmpty())
            {
                var internalRecipesToSend = _mapper.Map<IEnumerable<SendServerRecipeDto>>(responseAPI.results);
                
                foreach(SendServerRecipeDto recipe in internalRecipesToSend)
                {
                    var ingredients = await _ingredientService.MatchIngredients(recipe.Ingredients);
                    recipe.Ingredients = ingredients.Select(x => x.Description).ToList();
                    recipe.IngredientIds = ingredients.Select(x => x.IngredientId).ToList();
                }

                var internalResult = await _internalApiClient.PostAsJsonAsync("api/Recipe", internalRecipesToSend);
                if (internalResult.IsSuccessStatusCode)
                {
                    var internalRecipes = await internalResult.Content.ReadFromJsonAsync<Response<IEnumerable<ReceiveServerRecipeDto>>>();
                    result = internalRecipes.Data;
                }
            }
            else // nothing from external api
            {
                return null;
            }
        }

        _cacheService.Set($"{typeof(ReceiveServerRecipeDto)}-{searchQuery}", result, new MemoryCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
        });

        return result.Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize);
    }

    public async Task<IEnumerable<ReceiveServerRecipeDto>> GetRecipesByIngredientsAsync(string ingredients)
    {
        var response = await _internalApiClient.GetFromJsonAsync<Response<IEnumerable<ReceiveServerRecipeDto>>>($"api/Recipe/byIngredients?ingredients={ingredients}");
        return response.Data;
    }
}