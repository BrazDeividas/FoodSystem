namespace FoodSystemClient.Authentication;

public class TokenProvider
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public int ExpiresIn { get; set; }
}