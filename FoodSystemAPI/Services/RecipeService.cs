using System.Text;
using System.Text.Json;
using AutoMapper;
using FoodSystemAPI.DTOs;
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

    public RecipeService(IHttpClientFactory httpClientFactory, IMapper mapper, ICacheService cacheService)
    {
        _apiClient = httpClientFactory.CreateClient("api-1");
        _internalApiClient = httpClientFactory.CreateClient("api-internal");
        _mapper = mapper;
        _cacheService = cacheService;
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
            var responseAPI1 = await _apiClient.GetFromJsonAsync<RapidAPIDto_1>($"?q={searchQuery}");
            if(!responseAPI1.d.IsNullOrEmpty())
            {
                var internalRecipesToSend = _mapper.Map<IEnumerable<SendServerRecipeDto>>(responseAPI1.d);
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