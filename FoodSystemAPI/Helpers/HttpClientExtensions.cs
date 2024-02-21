namespace FoodSystemAPI.Helpers;

public static class HttpClientExtensions
{
    public static HttpClient AddRapidAPIHeaders(this HttpClient httpClient, string host, string apiKey)
    {
        var headers = httpClient.DefaultRequestHeaders;
        headers.Add("X-RapidAPI-Host", host);
        headers.Add("X-RapidAPI-Key", apiKey);
        return httpClient;
    }
}