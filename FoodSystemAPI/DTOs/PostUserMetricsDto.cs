namespace FoodSystemAPI.DTOs;

public class PostUserMetricsDto
{
    public int UserId { get; set; }
    public int Weight { get; set; }
    public int Height { get; set; }
    public int Age { get; set; }
    public string Sex { get; set; } = null!;
    public string ActivityLevel { get; set; } = null!;
}