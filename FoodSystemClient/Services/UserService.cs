using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using FoodSystemClient.Authentication;
using FoodSystemClient.DTOs;
using FoodSystemClient.Models;
using FoodSystemClient.Wrappers;

namespace FoodSystemClient.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private TokenProvider _tokenProvider;

    public UserService(IHttpClientFactory clientFactory, TokenProvider tokenProvider)
    {
        _httpClient = clientFactory.CreateClient("FoodSystemAPI");
        _tokenProvider = tokenProvider;
    }

    public async Task<ClaimsPrincipal> LoginAsync(string username)
    {
        var response = await _httpClient.PostAsJsonAsync("api/User/login", new { Username = username });
        if(response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync().ContinueWith(task =>
            {
                _tokenProvider.AccessToken = task.Result;
                var identity = new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(_tokenProvider.AccessToken), "jwt");
                var principal = new ClaimsPrincipal(identity);
                return principal;
            });
        }
        return null!;
    }

    public async Task<UserMetrics> GetUserMetrics()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/User/metrics");
        request.Headers.Add("Authorization", $"Bearer {_tokenProvider.AccessToken}");
        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var responseBody = await response.Content.ReadFromJsonAsync<Response<UserMetrics>>();

        if (responseBody == null)
        {
            return null;
        }
        
        return responseBody.Data;
    }

    public async Task<bool> PostUserMetrics(UserMetrics userMetrics)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, "api/User/metrics");
        request.Headers.Add("Authorization", $"Bearer {_tokenProvider.AccessToken}");
        var userMetricsDto = new PostUserMetricsDto
        {
            Age = userMetrics.Age,
            Height = userMetrics.Height,
            Weight = userMetrics.Weight,
            Sex = userMetrics.Sex.ToString(),
            ActivityLevel = userMetrics.ActivityLevel.ToString()
        };

        request.Content = new StringContent(JsonSerializer.Serialize(userMetricsDto), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);

        if(response.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }
}