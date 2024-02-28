namespace FoodSystemClient.DTOs;

public class ResponseLoginDto
{
    public string tokenType { get; set; } = null!;
    public string accessToken { get; set; } = null!;
    public int expiresIn { get; set; }
    public string refreshToken { get; set; } = null!;
}