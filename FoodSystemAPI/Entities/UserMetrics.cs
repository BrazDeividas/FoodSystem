using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FoodSystemAPI.Entities;

public class UserMetrics
{
    public enum SexType
    {
        Unknown = 0,
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

    public int UserMetricsId { get; set; }

    public int UserId { get; set; }

    public SexType Sex { get; set; }

    [Range(1, 200)]
    public int Age { get; set; }

    [Range(1, 300)]
    public int Height { get; set; }

    [Range(1, int.MaxValue)]
    public int Weight { get; set; }

    public ActivityLevelType ActivityLevel { get; set; }
    
    [JsonIgnore]
    public virtual User User { get; set; } = null!;
} 