using System.ComponentModel.DataAnnotations;

namespace FoodSystemClient.Models;

public class UserMetrics
{
    public enum SexType
    {
        Male = 1,
        Female = 2
    };

    public enum ActivityLevelType
    {
        Sedentary = 1,
        LightlyActive = 2,
        ModeratelyActive = 3,
        VeryActive = 4,
        ExtraActive = 5
    };

    [Required]
    public SexType Sex { get; set; }
    [Required]
    [Range(1, 120)]
    public int Age { get; set; }
    [Required]
    [Range(1, 300)]
    public int Height { get; set; }
    [Required]
    [Range(1, 500)]
    public int Weight { get; set; }
    [Required]
    public ActivityLevelType ActivityLevel { get; set; }
}