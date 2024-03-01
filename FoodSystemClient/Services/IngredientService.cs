using System.Diagnostics;
using System.Text;
using FoodSystemClient.Authentication;
using FoodSystemClient.Models;
using FoodSystemClient.Wrappers;

namespace FoodSystemClient.Services;

public class IngredientService : IIngredientService
{
    private readonly HttpClient _httpClient;
    private readonly TokenProvider _tokenProvider;

    public IngredientService(IHttpClientFactory clientFactory, TokenProvider tokenProvider)
    {
        _httpClient = clientFactory.CreateClient("FoodSystemAPI");
        _tokenProvider = tokenProvider;
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_tokenProvider.AccessToken}");
    }

    public async Task<PagedResponse<IEnumerable<Ingredient>>> GetIngredientsByPage(int pageNumber, int pageSize)
    {
        return await _httpClient.GetFromJsonAsync<PagedResponse<IEnumerable<Ingredient>>>($"api/Ingredient?PageNumber={pageNumber}&PageSize={pageSize}");
    }
    public async Task<PagedResponse<IEnumerable<Ingredient>>> GetIngredientsByPage(int pageNumber, int pageSize, string search)
    {
        if(string.IsNullOrEmpty(search))
        {
            return await _httpClient.GetFromJsonAsync<PagedResponse<IEnumerable<Ingredient>>>($"api/Ingredient?PageNumber={pageNumber}&PageSize={pageSize}");
        }

        return await _httpClient.GetFromJsonAsync<PagedResponse<IEnumerable<Ingredient>>>($"api/Ingredient?PageNumber={pageNumber}&PageSize={pageSize}&search={search}");  
    }

    public async Task AddIngredientsToUser(IEnumerable<int> ingredientIds)
    {
        await _httpClient.PostAsJsonAsync("api/Ingredient/AddToUser", ingredientIds);
    }

    public async Task<Response<IEnumerable<Ingredient>>> GetOwnedIngredients()
    {
        return await _httpClient.GetFromJsonAsync<Response<IEnumerable<Ingredient>>>("api/Ingredient/byUser");
    }
}