using FoodSystemAPI.Entities;

namespace FoodSystemAPI.Services;

public class MealPlanService : IMealPlanService
{
    public void PlanMealAsync(UserMetrics userMetrics)
    {
        throw new NotImplementedException();
    }

    public double CalculateCaloricNeeds(UserMetrics userMetrics) // Mifflin-St Jeor Formula + Harris Benedict Equation for Caloric needs
    {
        double bmr = (9.99 * userMetrics.Weight) + (6.25 * userMetrics.Height) - (4.92 * userMetrics.Age);
        bmr = userMetrics.Sex == UserMetrics.SexType.Male
        ? bmr + 5
        : bmr - 161;
        return bmr * userMetrics.ActivityLevel switch
        {
            UserMetrics.ActivityLevelType.Sedentary => 1.2,
            UserMetrics.ActivityLevelType.LightlyActive => 1.375,
            UserMetrics.ActivityLevelType.ModeratelyActive => 1.55,
            UserMetrics.ActivityLevelType.VeryActive => 1.725,
            UserMetrics.ActivityLevelType.ExtraActive => 1.9
        };
    }
}