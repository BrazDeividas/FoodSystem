namespace FoodSystemAPI.Helpers;

public static class APIHttpClientFactory
{
    public static HttpClient Create(string host)
    {
        var httpClient = new HttpClient()
        {
            BaseAddress = new Uri($"https://{host}")
        };

        ConfigureHttpClient(httpClient);
        
        return httpClient;
    }

    internal static void ConfigureHttpClient(HttpClient httpClient)
    {
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new("application/json"));
    }
}