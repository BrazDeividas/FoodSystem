using FoodSystemClient.Authentication;
using FoodSystemClient.Models;
using FoodSystemClient.Wrappers;

namespace FoodSystemClient.Services;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;
    private readonly TokenProvider _tokenProvider;

    public CategoryService(IHttpClientFactory clientFactory, TokenProvider tokenProvider)
    {
        _httpClient = clientFactory.CreateClient("FoodSystemAPI");
        _tokenProvider = tokenProvider;
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_tokenProvider.AccessToken}");
    }

    public async Task<Response<IEnumerable<Category>>> GetCategories()
    {
        return await _httpClient.GetFromJsonAsync<Response<IEnumerable<Category>>>("api/Category");
    }
}