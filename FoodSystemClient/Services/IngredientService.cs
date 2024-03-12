using System.Diagnostics;
using System.Text;
using System.Text.Json;
using FoodSystemClient.Authentication;
using FoodSystemClient.DTOs;
using FoodSystemClient.Models;
using FoodSystemClient.Wrappers;
using Microsoft.VisualBasic;

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

    public async Task RemoveIngredientsFromUser(IEnumerable<int> ingredientIds)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri(_httpClient.BaseAddress!, "api/Ingredient/RemoveFromUser"),
            Content = new StringContent(JsonSerializer.Serialize(ingredientIds), Encoding.UTF8, "application/json")
        };

        await _httpClient.SendAsync(request);
    }

    public async Task<Response<IEnumerable<Ingredient>>> GetOwnedIngredients()
    {
        return await _httpClient.GetFromJsonAsync<Response<IEnumerable<Ingredient>>>("api/Ingredient/byUser");
    }

    public async Task AddNewIngredientAsync(PostIngredientDto ingredient)
    {
        return;
    }
}