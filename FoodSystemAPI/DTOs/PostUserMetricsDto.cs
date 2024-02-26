using System.ComponentModel.DataAnnotations;

namespace FoodSystemAPI.DTOs;

public class PostUserMetricsDto
{
    public int UserId { get; set; }
    [Range(1, 500)]
    public int Weight { get; set; }
    [Range(1, 300)]
    public int Height { get; set; }
    [Range(1, 200)]
    public int Age { get; set; }
    public string Sex { get; set; } = null!;
    public string ActivityLevel { get; set; } = null!;
}