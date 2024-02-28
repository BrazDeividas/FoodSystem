namespace FoodSystemClient.Models;

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

    public SexType Sex { get; set; }
    public int Age { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public ActivityLevelType ActivityLevel { get; set; }
}