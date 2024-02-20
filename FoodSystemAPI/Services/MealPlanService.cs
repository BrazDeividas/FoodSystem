using FoodSystemAPI.Entities;

namespace FoodSystemAPI.Services;

public class MealPlanService : IMealPlanService
{
    public void PlanMealAsync(UserMetrics userMetrics)
    {
        throw new NotImplementedException();
    }

    public double CalculateCaloricNeeds(UserMetrics userMetrics)
    {
        double bmr;
        
        switch(userMetrics.Sex)
        {   
            case UserMetrics.SexType.Male:
                bmr = (9.99 * userMetrics.Weight) + (6.25 * userMetrics.Height) - (4.92 * userMetrics.Age) + 5;
                return bmr * userMetrics.ActivityLevel switch
                {
                    UserMetrics.ActivityLevelType.Sedentary => 1.2,
                    UserMetrics.ActivityLevelType.LightlyActive => 1.375,
                    UserMetrics.ActivityLevelType.ModeratelyActive => 1.55,
                    UserMetrics.ActivityLevelType.VeryActive => 1.725,
                    UserMetrics.ActivityLevelType.ExtraActive => 1.9
                };
            
            case UserMetrics.SexType.Female:
                bmr = (9.99 * userMetrics.Weight) + (6.25 * userMetrics.Height) - (4.92 * userMetrics.Age) - 161;
                return bmr * userMetrics.ActivityLevel switch
                {
                    UserMetrics.ActivityLevelType.Sedentary => 1.2,
                    UserMetrics.ActivityLevelType.LightlyActive => 1.375,
                    UserMetrics.ActivityLevelType.ModeratelyActive => 1.55,
                    UserMetrics.ActivityLevelType.VeryActive => 1.725,
                    UserMetrics.ActivityLevelType.ExtraActive => 1.9
                };
            
            default:
                return 0;
        }
    }
}